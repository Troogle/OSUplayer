using System.Collections.Generic;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
using Mp3Lib;

namespace OSUplayer.OsuFiles
{
    [Serializable]
    public class Beatmap : IComparable<Beatmap>
    {
        private string _osb;
        private readonly string[] Rawdata = new string[(int)OSUfile.OSUfilecount];
        //diff-wide storyboard
        [NonSerialized]
        public StoryBoard.StoryBoard SB;
        public int Totalhitcount { get; set; }
        public int Offset { get; set; }
        public string Location { get; set; }
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
        public string FileVersion { get { return Rawdata[(int)OSUfile.FileVersion]; } }
        public string Audio
        {
            get { return System.IO.Path.Combine(Location, Rawdata[(int)OSUfile.AudioFilename]); }
            set { Rawdata[(int)OSUfile.AudioFilename] = value; }
        }
        public int Previewtime
        {
            get
            {
                return Rawdata[(int)OSUfile.PreviewTime] != null ? Convert.ToInt32(Rawdata[(int)OSUfile.PreviewTime]) : 0;
            }
        }
        public string SampleSet
        {
            get
            {
                return Rawdata[(int)OSUfile.SampleSet] ?? "Normal";
            }
        }
        public Modes Mode
        {
            get
            {
                if (Rawdata[(int)OSUfile.Mode] != null)
                {
                    return (Modes)Enum.Parse(typeof(Modes), Rawdata[(int)OSUfile.Mode]);
                }
                else
                {
                    return Modes.Osu;
                }
            }
            set
            {
                Rawdata[(int)OSUfile.Mode] = value.ToString();
            }
        }
        public string Artist
        {
            get
            {
                return Rawdata[(int)OSUfile.ArtistUnicode] ?? (Rawdata[(int)OSUfile.Artist]);
            }
            set
            { Rawdata[(int)OSUfile.ArtistUnicode] = value; }
        }
        public string ArtistRomanized
        {
            get
            {
                return Rawdata[(int)OSUfile.Artist] ?? "<unknown artist>";
            }
            set
            {
                Rawdata[(int)OSUfile.Artist] = value;
            }
        }
        public string Title
        {
            get
            {
                return Rawdata[(int)OSUfile.TitleUnicode] ?? (TitleRomanized);
            }
            set
            {
                Rawdata[(int)OSUfile.TitleUnicode] = value;
            }
        }
        public string TitleRomanized
        {
            get
            {
                return Rawdata[(int)OSUfile.Title] ?? "<unknown title>";
            }
            set
            {
                Rawdata[(int)OSUfile.Title] = value;
            }
        }
        public string Creator { get { return Rawdata[(int)OSUfile.Creator]; } set { Rawdata[(int)OSUfile.Creator] = value; } }
        public string Tags { get { return Rawdata[(int)OSUfile.Tags]; } set { Rawdata[(int)OSUfile.Tags] = value; } }
        public string Version { get { return Rawdata[(int)OSUfile.Version]; } set { Rawdata[(int)OSUfile.Version] = value; } }
        public string Source
        {
            get
            {
                return Rawdata[(int)OSUfile.Source] ?? "<unknown source>";
            }
            set { Rawdata[(int)OSUfile.Source] = value; }
        }
        public int BeatmapID
        {
            get
            {
                return Rawdata[(int)OSUfile.BeatmapID] != null ? Convert.ToInt32(Rawdata[(int)OSUfile.BeatmapID]) : 0;
            }
            set { Rawdata[(int)OSUfile.BeatmapID] = value.ToString(); }
        }
        public int BeatmapsetID
        {
            get
            {
                return Rawdata[(int)OSUfile.BeatmapSetID] != null
                    ? Convert.ToInt32(Rawdata[(int)OSUfile.BeatmapSetID])
                    : -1;
            }
            set { Rawdata[(int)OSUfile.BeatmapSetID] = value.ToString(); }
        }
        public double HPDrainRate
        {
            get
            {
                return Rawdata[(int)OSUfile.HPDrainRate] != null ? Convert.ToDouble(Rawdata[(int)OSUfile.HPDrainRate]) : 5;
            }
        }
        public double CircleSize
        {
            get
            {
                return Rawdata[(int)OSUfile.CircleSize] != null ? Convert.ToDouble(Rawdata[(int)OSUfile.CircleSize]) : 5;
            }
        }
        public double OverallDifficulty
        {
            get
            {
                return Rawdata[(int)OSUfile.OverallDifficulty] != null ? Convert.ToDouble(Rawdata[(int)OSUfile.OverallDifficulty]) : 5;
            }
        }
        public double ApproachRate
        {
            get
            {
                return Rawdata[(int)OSUfile.ApproachRate] != null ? Convert.ToDouble(Rawdata[(int)OSUfile.ApproachRate]) : OverallDifficulty;
            }
        }
        public double SliderMultiplier
        {
            get
            {
                return Rawdata[(int)OSUfile.SliderMultiplier] != null ? Convert.ToDouble(Rawdata[(int)OSUfile.SliderMultiplier]) : 1;
            }
        }
        public double SliderTickRate
        {
            get
            {
                return Rawdata[(int)OSUfile.SliderTickRate] != null ? Convert.ToDouble(Rawdata[(int)OSUfile.SliderTickRate]) : 1;
            }
        }
        #endregion
        private static string Picknext(ref string str)
        {
            string ret = "";
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

        private static void SetAdditionalObject(ref string row, ref HitObject NHit, out string tmpop, string picknext)
        {
            tmpop = picknext;
            if (tmpop != "")
            {
                if (tmpop.Length > 3)
                {
                    NHit.sample = new CSample((int)Char.GetNumericValue(tmpop[0]), (int)Char.GetNumericValue(tmpop[4]));
                    NHit.A_sample = new CSample((int)Char.GetNumericValue(tmpop[2]), (int)Char.GetNumericValue(tmpop[4]));
                    if (tmpop.Length < 7)
                    {
                        tmpop = tmpop + ":0:";
                    }
                    NHit.S_Volume = (int)Char.GetNumericValue(tmpop[6]);
                }
                else
                {
                    NHit.sample = new CSample((int)Char.GetNumericValue(tmpop[0]), (int)Char.GetNumericValue(tmpop[2]));
                    NHit.A_sample = new CSample(0, 0);
                    NHit.S_Volume = 0;
                }
            }
            else
            {
                NHit.sample = new CSample(0, 0);
                NHit.A_sample = new CSample(0, 0);
                NHit.S_Volume = 0;
            }
        }
        private static HitObject Setobject(string row)
        {
            var NHit = new HitObject();
            string tmpop = "";
            NHit.x = Convert.ToInt32(Picknext(ref row));
            NHit.y = Convert.ToInt32(Picknext(ref row));
            NHit.starttime = Convert.ToInt32(Picknext(ref row));
            NHit.type = Check(Picknext(ref row));
            switch (NHit.type)
            {
                case ObjectFlag.Normal:
                    NHit.allhitsound = Convert.ToInt32(Picknext(ref row));
                    SetAdditionalObject(ref row, ref NHit, out tmpop, Picknext(ref row));
                    break;
                case ObjectFlag.Spinner:
                    NHit.allhitsound = Convert.ToInt32(Picknext(ref row));
                    NHit.EndTime = Convert.ToInt32(Picknext(ref row));
                    SetAdditionalObject(ref row, ref NHit, out tmpop, Picknext(ref row));
                    break;
                case ObjectFlag.Slider:
                    NHit.allhitsound = Convert.ToInt32(Picknext(ref row));
                    tmpop = Picknext(ref row);
                    //ignore all anthor
                    tmpop = Picknext(ref row);
                    if (tmpop != "") { NHit.repeatcount = Convert.ToInt32(tmpop); }
                    else { NHit.repeatcount = 1; }
                    tmpop = Picknext(ref row);
                    if (tmpop != "") { NHit.length = Convert.ToDouble(tmpop); }
                    else { NHit.length = 0; }
                    tmpop = Picknext(ref row);
                    NHit.Hitsounds = new int[NHit.repeatcount + 1];
                    NHit.samples = new CSample[NHit.repeatcount + 1];
                    string[] split = tmpop.Split(new char[] { '|' });
                    if (split.Length != 1)
                    {
                        for (int i = 0; i <= NHit.repeatcount; i++)
                        {
                            NHit.Hitsounds[i] = Convert.ToInt32(split[i]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= NHit.repeatcount; i++)
                        {
                            NHit.Hitsounds[i] = 1;
                        }
                    }
                    tmpop = Picknext(ref row);
                    split = tmpop.Split(new char[] { '|' });
                    if (split.Length != 1)
                    {
                        for (int i = 0; i <= NHit.repeatcount; i++)
                        {
                            NHit.samples[i] = new CSample(Convert.ToInt32(split[i][0]),
                                Convert.ToInt32(split[i][2]));
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= NHit.repeatcount; i++)
                        {
                            NHit.samples[i] = new CSample(0, 0);
                        }
                    }
                    SetAdditionalObject(ref row, ref NHit, out tmpop, Picknext(ref row));
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
            return NHit;
        }
        private static Timing Settiming(string row)
        {
            var Ntiming = new Timing();
            string tmpop = "";
            Ntiming.offset = (int)(Convert.ToDouble(Picknext(ref row)));
            Ntiming.bpm = Convert.ToDouble(Picknext(ref row));
            tmpop = Picknext(ref row);
            if (tmpop == "") { Ntiming.meter = 4; }
            else { Ntiming.meter = Convert.ToInt32(tmpop); }
            tmpop = Picknext(ref row);
            if (tmpop == "") { Ntiming.sample = new CSample((int)TSample.Normal, 0); }
            else { Ntiming.sample = new CSample(Convert.ToInt32(tmpop), Convert.ToInt32(Picknext(ref row))); }
            tmpop = Picknext(ref row);
            if (tmpop == "") { Ntiming.volume = 1.0f; }
            else { Ntiming.volume = Convert.ToSingle(tmpop) / 100; }
            tmpop = Picknext(ref row);
            if (tmpop == "") { Ntiming.type = 1; }
            else { Ntiming.type = Convert.ToInt32(tmpop); }
            tmpop = Picknext(ref row);
            if (tmpop == "") { Ntiming.kiai = 0; }
            else { Ntiming.kiai = Convert.ToInt32(tmpop); }
            if (Ntiming.type == 1)
            {
                Ntiming.bpm = 60000 / Ntiming.bpm;
            }
            else
            {
                Ntiming.bpm = -100 / Ntiming.bpm;
            }
            return Ntiming;
        }
        private enum OsuFileScanStatus
        {
            VERSION_UNKNOWN,
            GENERAL,
            EDITOR,
            METADATA,
            DIFFICULTY,
            VARIABLES,
            EVENTS,
            TIMINGPOINTS,
            COLOURS,
            HITOBJECTS
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
            var position = OsuFileScanStatus.VERSION_UNKNOWN;
            try
            {
                using (var reader = new StreamReader(Path))
                {
                    Timingpoints = new List<Timing>();
                    HitObjects = new List<HitObject>();
                    string row;
                    while (!reader.EndOfStream)
                    {
                        row = reader.ReadLine();
                        if (row.Trim() == "") { continue; }
                        if (row.StartsWith("//") || row.Length == 0) { continue; }
                        if (row.StartsWith("["))
                        {
                            position = (OsuFileScanStatus)Enum.Parse(typeof(OsuFileScanStatus), (row.Substring(1, row.Length - 2).ToUpper()));
                            continue;
                        }
                        switch (position)
                        {
                            case OsuFileScanStatus.VERSION_UNKNOWN:
                                Rawdata[(int)OSUfile.FileVersion] = row.Substring(17);
                                break;
                            case OsuFileScanStatus.GENERAL:
                            case OsuFileScanStatus.METADATA:
                            case OsuFileScanStatus.DIFFICULTY:
                                var s = row.Split(new char[] { ':' }, 2);
                                try { Rawdata[(int)(OSUfile)Enum.Parse(typeof(OSUfile), (s[0].Trim()))] = s[1].Trim(); }
                                catch { }
                                break;
                            case OsuFileScanStatus.EVENTS:
                                if (row.StartsWith("0,0,"))
                                {
                                    string str = row.Substring(5, row.Length - 6);
                                    if (str.Contains("\""))
                                    {
                                        str = str.Substring(0, str.IndexOf("\""));
                                    }
                                    Background = System.IO.Path.Combine(Location, str);
                                }
                                else if (row.StartsWith("1,") || row.StartsWith("Video"))
                                {
                                    HaveVideo = true;
                                    string[] vdata = row.Split(new char[] { ',' });
                                    VideoOffset = Convert.ToInt32(vdata[1]);
                                    Video = System.IO.Path.Combine(Location, vdata[2].Substring(1, vdata[2].Length - 2));
                                }
                                else if (row.StartsWith("3,") || row.StartsWith("2,")) { break; }
                                else { HaveSB = true; }
                                break;
                            case OsuFileScanStatus.TIMINGPOINTS:
                                Timingpoints.Add(Settiming(row));
                                break;
                            case OsuFileScanStatus.HITOBJECTS:
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
        public override bool Equals(Object obj)
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
            //return (Name.Substring(0, Name.LastIndexOf('.')));
            return (String.Format("{0} - {1} ({2}) [{3}]", Artist, Title, Version, Creator));
        }
        public string GetHash()
        {
            if (Hash != null) { return Hash; }
            if (Path == "" || !File.Exists(Path)) { return ""; }
            string strHashData = "";
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
            return string.Join("_", filename.Split(System.IO.Path.GetInvalidFileNameChars()));
        }
        public void SaveAudio(string toLocation)
        {
            var ext = System.IO.Path.GetExtension(Rawdata[(int)OSUfile.AudioFilename]);
            var toName = System.IO.Path.Combine(toLocation, GetSafeFilename(Title + ext));
            if (File.Exists(toName)) toName = System.IO.Path.Combine(toLocation, GetSafeFilename(Title + "[" + Version + "]" + ext));
            File.Copy(Audio, toName, true);
            try
            {
                var file = new Mp3File(toName);
                file.TagHandler.Artist = Artist;
                file.TagHandler.Title = Title;
                file.Update();
            }
            catch (Exception)
            {
                File.Copy(Audio, toName, true);
            }
        }
    }
}
