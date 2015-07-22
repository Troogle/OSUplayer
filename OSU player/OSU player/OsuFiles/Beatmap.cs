using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Mp3Lib;
using static System.String;

namespace OSUplayer.OsuFiles
{
    [Serializable]
    public class Beatmap : IComparable<Beatmap>
    {
        private string _osb;
        private readonly string[] _rawdata = new string[(int)OSUfile.OSUfilecount];
        //diff-wide storyboard
        [NonSerialized]
        public StoryBoard.StoryBoard SB;
        public int Totalhitcount { get; set; }
        public int Offset { get; set; }
        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                var raw = System.IO.Path.Combine(Properties.Settings.Default.OSUpath, value);
                _location = !Directory.Exists(raw) ? System.IO.Path.Combine(System.IO.Path.Combine(Properties.Settings.Default.OSUpath, "Songs"), value) : raw;
            }
        }

        public string Name { get; set; }
        public string Path { get; private set; }
        public bool HaveSB { get; private set; }
        public bool HaveVideo { get; private set; }
        public string Background { get; set; }
        public string Video { get; private set; }
        public int VideoOffset { get; private set; }
        [NonSerialized]
        public bool Detailed = false;
        [NonSerialized]
        public List<Timing> Timingpoints;
        [NonSerialized]
        public List<HitObject> HitObjects;
        public string Hash { get; set; }
        #region map属性的获取接口
        public string FileVersion => _rawdata[(int)OSUfile.FileVersion];

        public string Audio
        {
            get { return System.IO.Path.Combine(Location, _rawdata[(int)OSUfile.AudioFilename]); }
            set { _rawdata[(int)OSUfile.AudioFilename] = value; }
        }
        public int Previewtime => _rawdata[(int)OSUfile.PreviewTime] != null ? Convert.ToInt32(_rawdata[(int)OSUfile.PreviewTime]) : 0;

        public string SampleSet => _rawdata[(int)OSUfile.SampleSet] ?? "Normal";

        public Modes Mode
        {
            get
            {
                if (_rawdata[(int)OSUfile.Mode] != null)
                {
                    return (Modes)Enum.Parse(typeof(Modes), _rawdata[(int)OSUfile.Mode]);
                }
                return Modes.Osu;
            }
            set
            {
                _rawdata[(int)OSUfile.Mode] = value.ToString();
            }
        }
        public string Artist
        {
            get
            {
                return _rawdata[(int)OSUfile.ArtistUnicode] ?? (_rawdata[(int)OSUfile.Artist]);
            }
            set
            { _rawdata[(int)OSUfile.ArtistUnicode] = value; }
        }
        public string ArtistRomanized
        {
            get
            {
                return _rawdata[(int)OSUfile.Artist] ?? "<unknown artist>";
            }
            set
            {
                _rawdata[(int)OSUfile.Artist] = value;
            }
        }
        public string Title
        {
            get
            {
                return _rawdata[(int)OSUfile.TitleUnicode] ?? (TitleRomanized);
            }
            set
            {
                _rawdata[(int)OSUfile.TitleUnicode] = value;
            }
        }
        public string TitleRomanized
        {
            get
            {
                return _rawdata[(int)OSUfile.Title] ?? "<unknown title>";
            }
            set
            {
                _rawdata[(int)OSUfile.Title] = value;
            }
        }
        public string Creator { get { return _rawdata[(int)OSUfile.Creator]; } set { _rawdata[(int)OSUfile.Creator] = value; } }
        public string Tags { get { return _rawdata[(int)OSUfile.Tags]; } set { _rawdata[(int)OSUfile.Tags] = value; } }
        public string Version { get { return _rawdata[(int)OSUfile.Version]; } set { _rawdata[(int)OSUfile.Version] = value; } }
        public string Source
        {
            get
            {
                return _rawdata[(int)OSUfile.Source] ?? "<unknown source>";
            }
            set { _rawdata[(int)OSUfile.Source] = value; }
        }
        public int BeatmapID
        {
            get
            {
                return _rawdata[(int)OSUfile.BeatmapID] != null ? Convert.ToInt32(_rawdata[(int)OSUfile.BeatmapID]) : 0;
            }
            set { _rawdata[(int)OSUfile.BeatmapID] = value.ToString(); }
        }
        public int BeatmapsetID
        {
            get
            {
                return _rawdata[(int)OSUfile.BeatmapSetID] != null
                    ? Convert.ToInt32(_rawdata[(int)OSUfile.BeatmapSetID])
                    : -1;
            }
            set { _rawdata[(int)OSUfile.BeatmapSetID] = value.ToString(); }
        }
        public double HPDrainRate => _rawdata[(int)OSUfile.HPDrainRate] != null ? Convert.ToDouble(_rawdata[(int)OSUfile.HPDrainRate]) : 5;

        public double CircleSize => _rawdata[(int)OSUfile.CircleSize] != null ? Convert.ToDouble(_rawdata[(int)OSUfile.CircleSize]) : 5;

        public double OverallDifficulty => _rawdata[(int)OSUfile.OverallDifficulty] != null ? Convert.ToDouble(_rawdata[(int)OSUfile.OverallDifficulty]) : 5;

        public double ApproachRate => _rawdata[(int)OSUfile.ApproachRate] != null ? Convert.ToDouble(_rawdata[(int)OSUfile.ApproachRate]) : OverallDifficulty;

        public double SliderMultiplier => _rawdata[(int)OSUfile.SliderMultiplier] != null ? Convert.ToDouble(_rawdata[(int)OSUfile.SliderMultiplier]) : 1;

        public double SliderTickRate => _rawdata[(int)OSUfile.SliderTickRate] != null ? Convert.ToDouble(_rawdata[(int)OSUfile.SliderTickRate]) : 1;

        #endregion
        private static string Picknext(ref string str)
        {
            string ret;
            if (!str.Contains(","))
            {
                ret = str;
                str = "";
            }
            else
            {
                ret = str.Substring(0, str.IndexOf(","));
                str = str.Substring(str.IndexOf(",") + 1);
            }
            return ret;
        }
        private static ObjectFlag Check(string op)
        {
            var tmp = Convert.ToInt32(op);
            if (((tmp >> 0) & 1) == 1)
            {
                return ObjectFlag.Normal;
            }
            if (((tmp >> 1) & 1) == 1)
            {
                return ObjectFlag.Slider;
            }
            if (((tmp >> 3) & 1) == 1)
            {
                return ObjectFlag.Spinner;
            }
            return ObjectFlag.ColourHax;
        }

        private static void SetAdditionalObject(ref HitObject nHit, out string tmpop, string picknext)
        {
            tmpop = picknext;
            if (tmpop != "")
            {
                if (tmpop.Length > 3)
                {
                    nHit.sample = new CSample((int)Char.GetNumericValue(tmpop[0]), (int)Char.GetNumericValue(tmpop[4]));
                    nHit.A_sample = new CSample((int)Char.GetNumericValue(tmpop[2]), (int)Char.GetNumericValue(tmpop[4]));
                    if (tmpop.Length < 7)
                    {
                        tmpop = tmpop + ":0:";
                    }
                    nHit.S_Volume = (int)Char.GetNumericValue(tmpop[6]);
                }
                else
                {
                    nHit.sample = new CSample((int)Char.GetNumericValue(tmpop[0]), (int)Char.GetNumericValue(tmpop[2]));
                    nHit.A_sample = new CSample(0, 0);
                    nHit.S_Volume = 0;
                }
            }
            else
            {
                nHit.sample = new CSample(0, 0);
                nHit.A_sample = new CSample(0, 0);
                nHit.S_Volume = 0;
            }
        }
        private static HitObject Setobject(string row)
        {
            var nHit = new HitObject();
            string tmpop;
            nHit.x = Convert.ToInt32(Picknext(ref row));
            nHit.y = Convert.ToInt32(Picknext(ref row));
            nHit.starttime = Convert.ToInt32(Picknext(ref row));
            nHit.type = Check(Picknext(ref row));
            switch (nHit.type)
            {
                case ObjectFlag.Normal:
                    nHit.allhitsound = Convert.ToInt32(Picknext(ref row));
                    SetAdditionalObject(ref nHit, out tmpop, Picknext(ref row));
                    break;
                case ObjectFlag.Spinner:
                    nHit.allhitsound = Convert.ToInt32(Picknext(ref row));
                    nHit.EndTime = Convert.ToInt32(Picknext(ref row));
                    SetAdditionalObject(ref nHit, out tmpop, Picknext(ref row));
                    break;
                case ObjectFlag.Slider:
                    nHit.allhitsound = Convert.ToInt32(Picknext(ref row));
                    Picknext(ref row);
                    //ignore all anthor
                    tmpop = Picknext(ref row);
                    nHit.repeatcount = tmpop != "" ? Convert.ToInt32(tmpop) : 1;
                    tmpop = Picknext(ref row);
                    nHit.length = tmpop != "" ? Convert.ToDouble(tmpop) : 0;
                    tmpop = Picknext(ref row);
                    nHit.Hitsounds = new int[nHit.repeatcount + 1];
                    nHit.samples = new CSample[nHit.repeatcount + 1];
                    var split = tmpop.Split('|');
                    if (split.Length != 1)
                    {
                        for (var i = 0; i <= nHit.repeatcount; i++)
                        {
                            nHit.Hitsounds[i] = Convert.ToInt32(split[i]);
                        }
                    }
                    else
                    {
                        for (var i = 0; i <= nHit.repeatcount; i++)
                        {
                            nHit.Hitsounds[i] = 1;
                        }
                    }
                    tmpop = Picknext(ref row);
                    split = tmpop.Split('|');
                    if (split.Length != 1)
                    {
                        for (var i = 0; i <= nHit.repeatcount; i++)
                        {
                            nHit.samples[i] = new CSample(Convert.ToInt32(split[i][0]),
                                Convert.ToInt32(split[i][2]));
                        }
                    }
                    else
                    {
                        for (var i = 0; i <= nHit.repeatcount; i++)
                        {
                            nHit.samples[i] = new CSample(0, 0);
                        }
                    }
                    SetAdditionalObject(ref nHit, out tmpop, Picknext(ref row));
                    //HitSound,(SampleSet&Group|SampleSet&Group),(All SampleSet&Addition)
                    //SampleSet&Addition 只有sample没有setcount
                    //All SampleSet&Addition 啥都有
                    //数值1为不反复
                    //Length=长度（乘以每小节时间60000/BPM再乘以滑条速度SliderMultiplier为滑条时间长度）
                    //HitSound S1|S2|S3|S4......Sn 计算公式n=RepeatCount+1
                    break;
                default:
                    //Throw New FormatException("Failed to read .osu file")
                    //this is for mania
                    break;
            }
            return nHit;
        }
        private static Timing Settiming(string row)
        {
            var ntiming = new Timing
            {
                offset = (int)(Convert.ToDouble(Picknext(ref row))),
                bpm = Convert.ToDouble(Picknext(ref row))
            };
            var tmpop = Picknext(ref row);
            ntiming.meter = tmpop == "" ? 4 : Convert.ToInt32(tmpop);
            tmpop = Picknext(ref row);
            ntiming.sample = tmpop == "" ? new CSample((int)TSample.Normal, 0) : new CSample(Convert.ToInt32(tmpop), Convert.ToInt32(Picknext(ref row)));
            tmpop = Picknext(ref row);
            ntiming.volume = tmpop == "" ? 1.0f : Convert.ToSingle(tmpop) / 100;
            tmpop = Picknext(ref row);
            ntiming.type = tmpop == "" ? 1 : Convert.ToInt32(tmpop);
            tmpop = Picknext(ref row);
            ntiming.kiai = tmpop == "" ? 0 : Convert.ToInt32(tmpop);
            if (ntiming.type == 1)
            {
                ntiming.bpm = 60000 / ntiming.bpm;
            }
            else
            {
                ntiming.bpm = -100 / ntiming.bpm;
            }
            return ntiming;
        }
        private enum FileSection
        {
            VersionUnknown,
            General,
            Editor,
            Metadata,
            Difficulty,
            Variables,
            Events,
            TimingPoints,
            Colours,
            HitObjects,
            DontCare
        }
        public void Setsb(string osbF)
        {
            _osb = osbF;
        }
        public void GetDetail()
        {
            Path = System.IO.Path.Combine(Location, Name);
            if (!File.Exists(Path))
            {
                Timingpoints = new List<Timing>();
                HitObjects = new List<HitObject>();
                Detailed = true;
                return;
            }
            var position = FileSection.VersionUnknown;
            try
            {
                using (var reader = new StreamReader(Path))
                {
                    Timingpoints = new List<Timing>();
                    HitObjects = new List<HitObject>();
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        if (row == null || row.Trim() == "") { continue; }
                        if (row.StartsWith("//") || row.Length == 0) { continue; }
                        if (row.StartsWith("["))
                        {
                            try { position = (FileSection)Enum.Parse(typeof(FileSection), (row.Substring(1, row.Length - 2)), true);}
                            catch (ArgumentException) { position=FileSection.DontCare;}
                            continue;
                        }
                        switch (position)
                        {
                            case FileSection.VersionUnknown:
                                _rawdata[(int)OSUfile.FileVersion] = row.Substring(17);
                                break;
                            case FileSection.General:
                            case FileSection.Metadata:
                            case FileSection.Difficulty:
                                var s = row.Split(new[] { ':' }, 2);
                                try { _rawdata[(int)(OSUfile)Enum.Parse(typeof(OSUfile), (s[0].Trim()))] = s[1].Trim(); }
                                catch (ArgumentException) { }
                                break;
                            case FileSection.Events:
                                if (row.StartsWith("0,0,"))
                                {
                                    var str = row.Substring(5, row.Length - 6);
                                    if (str.Contains("\""))
                                    {
                                        str = str.Substring(0, str.IndexOf("\""));
                                    }
                                    Background = System.IO.Path.Combine(Location, str);
                                }
                                else if (row.StartsWith("1,") || row.StartsWith("Video"))
                                {
                                    HaveVideo = true;
                                    var vdata = row.Split(',');
                                    VideoOffset = Convert.ToInt32(vdata[1]);
                                    Video = System.IO.Path.Combine(Location, vdata[2].Substring(1, vdata[2].Length - 2));
                                }
                                else if (row.StartsWith("3,") || row.StartsWith("2,")) { }
                                else { HaveSB = true; }
                                break;
                            case FileSection.TimingPoints:
                                Timingpoints.Add(Settiming(row));
                                break;
                            case FileSection.HitObjects:
                                HitObjects.Add(Setobject(row));
                                break;
                        }
                    }
                }
                if (_osb != null) { HaveSB = true; }
                Detailed = true;
            }
            catch (SystemException e)
            {
                Console.WriteLine(e.StackTrace);
                throw (new FormatException("Failed to read .osu file" + Path, e));
            }
        }
        public void Getsb()
        {
            if (HaveSB)
            {
                SB = new StoryBoard.StoryBoard(Path, _osb, Location);
            }
        }
        public Beatmap()
        {
            Background = "";
            HaveSB = false;
            HaveVideo = false;
        }
        public Beatmap(string path, string location)
        {
            Path = path;
            Location = location;
        }
        public int CompareTo(Beatmap other)
        {
            if (Mode < other.Mode) { return -1; }
            if (Mode > other.Mode) { return 1; }
            if (Totalhitcount == other.Totalhitcount) { return 0; }
            if (Totalhitcount > other.Totalhitcount) { return -1; } else { return 1; }
        }
        /// <summary>
        /// 判断相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var map = (Beatmap)obj;
            if ((map.BeatmapID == BeatmapID) && (BeatmapID != 0))
            { return true; }
            return ToString().Equals(obj.ToString()) && Creator.Equals(map.Creator);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString() { return Version; }
        public string NameToString()
        {
            return ($"{Artist} - {Title} ({Version}) [{Creator}]");
        }
        public string GetHash()
        {
            if (Hash != null) { return Hash; }
            if (IsNullOrEmpty(Path)) { Path = System.IO.Path.Combine(Location, Name); }
            if (!File.Exists(Path)) { Hash = ""; }
            string strHashData;
            using (var md5Hash = MD5.Create())
            {
                using (var fs = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var arrHashValue = md5Hash.ComputeHash(fs);
                    strHashData = BitConverter.ToString(arrHashValue);
                    strHashData = strHashData.Replace("-", "");
                }
            }
            Hash = strHashData.ToLower();
            return Hash;
        }
        private static string GetSafeFilename(string filename)
        {
            return Join("_", filename.Split(System.IO.Path.GetInvalidFileNameChars()));
        }
        public void SaveAudio(string toLocation)
        {
            var ext = System.IO.Path.GetExtension(_rawdata[(int)OSUfile.AudioFilename]);
            var toName = System.IO.Path.Combine(toLocation, GetSafeFilename(Title + ext));
            if (File.Exists(toName)) toName = System.IO.Path.Combine(toLocation, GetSafeFilename(Title + "[" + Version + "]" + ext));
            File.Copy(Audio, toName, true);
            try
            {
                var file = new Mp3File(toName)
                {
                    TagHandler =
                    {
                        Artist = Artist,
                        Title = Title
                    }
                };
                file.Update();
            }
            catch (Exception)
            {
                File.Copy(Audio, toName, true);
            }
        }
    }
}
