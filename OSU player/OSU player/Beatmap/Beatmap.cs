using System.Collections.Generic;
using System.IO;
using System;
using System.Security.Cryptography;
namespace OSU_player.OSUFiles
{
    [Serializable]
    public class Beatmap : IComparable<Beatmap>
    {
        private string osb;
        private string[] Rawdata = new string[(int)OSUfile.OSUfilecount];
        private string backgroundOffset = "";
        //diff-wide storyboard
        [NonSerialized]
        public OSUFiles.StoryBoard.StoryBoard SB;
        public int offset { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool haveSB { get; private set; }
        public bool haveVideo { get; private set; }
        public string Background { get; set; }
        public string Video { get; private set; }
        public int VideoOffset { get; private set; }
        [NonSerialized]
        public bool detailed = false;
        [NonSerialized]
        public List<Timing> Timingpoints;
        [NonSerialized]
        public List<HitObject> HitObjects;
        public string hash { get; set; }
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
                if (Rawdata[(int)OSUfile.PreviewTime] != null)
                {
                    return Convert.ToInt32(Rawdata[(int)OSUfile.PreviewTime]);
                }
                else
                {
                    return 0;
                }
            }
        }
        public string SampleSet
        {
            get
            {
                if (Rawdata[(int)OSUfile.SampleSet] != null)
                {
                    return Rawdata[(int)OSUfile.SampleSet];
                }
                else
                {
                    return "Normal";
                }
            }
        }
        public int mode
        {
            set { Rawdata[(int)OSUfile.Mode] = value.ToString(); }
        }
        public modes Mode
        {
            get
            {
                if (Rawdata[(int)OSUfile.Mode] != null)
                {
                    return (modes)Enum.Parse(typeof(modes), Rawdata[(int)OSUfile.Mode]);
                }
                else
                {
                    return modes.Osu;
                }
            }
        }
        public string Artist
        {
            get
            {
                if (Rawdata[(int)OSUfile.ArtistUnicode] != null)
                {
                    return Rawdata[(int)OSUfile.ArtistUnicode];
                }
                else
                {
                    return (Rawdata[(int)OSUfile.Artist]);
                }
            }
            set
            { Rawdata[(int)OSUfile.ArtistUnicode] = value; }
        }
        public string ArtistRomanized
        {
            get
            {
                if (Rawdata[(int)OSUfile.Artist] != null)
                {
                    return Rawdata[(int)OSUfile.Artist];
                }
                else
                {
                    return "<unknown artist>";
                }
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
                if (Rawdata[(int)OSUfile.TitleUnicode] != null)
                {
                    return Rawdata[(int)OSUfile.TitleUnicode];
                }
                else
                {
                    return (TitleRomanized);
                }
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
                if (Rawdata[(int)OSUfile.Title] != null)
                {
                    return Rawdata[(int)OSUfile.Title];
                }
                else
                {
                    return "<unknown title>";
                }
            }
            set
            {
                Rawdata[(int)OSUfile.Title] = value;
            }
        }
        public string Creator { get { return Rawdata[(int)OSUfile.Creator]; } set { Rawdata[(int)OSUfile.Creator] = value; } }
        public string tags { get { return Rawdata[(int)OSUfile.Tags]; } set { Rawdata[(int)OSUfile.Tags] = value; } }
        public string Version { get { return Rawdata[(int)OSUfile.Version]; } set { Rawdata[(int)OSUfile.Version] = value; } }
        public string Source
        {
            get
            {
                if (Rawdata[(int)OSUfile.Source] != null)
                {
                    return Rawdata[(int)OSUfile.Source];
                }
                else
                {
                    return "<unknown source>";
                }
            }
            set { Rawdata[(int)OSUfile.Source] = value; }
        }
        public int beatmapId
        {
            get
            {
                if (Rawdata[(int)OSUfile.BeatmapID] != null)
                {
                    return Convert.ToInt32(Rawdata[(int)OSUfile.BeatmapID]);
                }
                else
                {
                    return 0;
                }
            }
            set { Rawdata[(int)OSUfile.BeatmapID] = value.ToString(); }
        }
        public int beatmapsetId
        {
            get
            {
                if (Rawdata[(int)OSUfile.BeatmapSetID] != null)
                {
                    return Convert.ToInt32(Rawdata[(int)OSUfile.BeatmapSetID]);
                }
                else
                {
                    return -1;
                }
            }
            set { Rawdata[(int)OSUfile.BeatmapSetID] = value.ToString(); }
        }
        public int HPDrainRate
        {
            get
            {
                if (Rawdata[(int)OSUfile.HPDrainRate] != null)
                {
                    return Convert.ToInt32(Rawdata[(int)OSUfile.HPDrainRate]);
                }
                else
                {
                    return 5;
                }
            }
        }
        public int CircleSize
        {
            get
            {
                if (Rawdata[(int)OSUfile.CircleSize] != null)
                {
                    return Convert.ToInt32(Rawdata[(int)OSUfile.CircleSize]);
                }
                else
                {
                    return 5;
                }
            }
        }
        public int OverallDifficulty
        {
            get
            {
                if (Rawdata[(int)OSUfile.OverallDifficulty] != null)
                {
                    return Convert.ToInt32(Rawdata[(int)OSUfile.OverallDifficulty]);
                }
                else
                {
                    return 5;
                }
            }
        }
        public int ApproachRate
        {
            get
            {
                if (Rawdata[(int)OSUfile.ApproachRate] != null)
                {
                    return Convert.ToInt32(Rawdata[(int)OSUfile.ApproachRate]);
                }
                else
                {
                    return Convert.ToInt32(Rawdata[(int)OSUfile.OverallDifficulty]);
                }
            }
        }
        public double SliderMultiplier
        {
            get
            {
                if (Rawdata[(int)OSUfile.SliderMultiplier] != null)
                {
                    return Convert.ToDouble(Rawdata[(int)OSUfile.SliderMultiplier]);
                }
                else
                {
                    return 1;
                }
            }
        }
        public double SliderTickRate
        {
            get
            {
                if (Rawdata[(int)OSUfile.SliderTickRate] != null)
                {
                    return Convert.ToDouble(Rawdata[(int)OSUfile.SliderTickRate]);
                }
                else
                {
                    return 1;
                }
            }
        }
        #endregion
        private string picknext(ref string str)
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
        private ObjectFlag check(string op)
        {
            int tmp = Convert.ToInt32(op);
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
        private HitObject setobject(string row)
        {
            HitObject NHit = new HitObject();
            string tmpop = "";
            NHit.x = Convert.ToInt32(picknext(ref row));
            NHit.y = Convert.ToInt32(picknext(ref row));
            NHit.starttime = Convert.ToInt32(picknext(ref row));
            NHit.type = check(picknext(ref row));
            switch (NHit.type)
            {
                case ObjectFlag.Normal:
                    NHit.allhitsound = Convert.ToInt32(picknext(ref row));
                    tmpop = picknext(ref row);
                    if (tmpop != "")
                    {
                        if (tmpop.Length > 3)
                        {
                            NHit.sample = new CSample
                                ((int)Char.GetNumericValue(tmpop[0]), (int)Char.GetNumericValue(tmpop[4]));
                            NHit.A_sample = new CSample((int)Char.GetNumericValue(tmpop[2]),
                                (int)Char.GetNumericValue(tmpop[4]));
                            if (tmpop.Length < 7) { tmpop = tmpop + ":0:"; }
                            NHit.S_Volume = (int)Char.GetNumericValue(tmpop[6]);
                        }
                        else
                        {
                            NHit.sample = new CSample((int)Char.GetNumericValue(tmpop[0]),
                                (int)Char.GetNumericValue(tmpop[2]));
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
                    break;
                case ObjectFlag.Spinner:
                    NHit.allhitsound = Convert.ToInt32(picknext(ref row));
                    NHit.EndTime = Convert.ToInt32(picknext(ref row));
                    tmpop = picknext(ref row);
                    if (tmpop != "")
                    {
                        if (tmpop.Length > 3)
                        {
                            NHit.sample = new CSample((int)Char.GetNumericValue(tmpop[0]),
                                (int)Char.GetNumericValue(tmpop[4]));
                            NHit.A_sample = new CSample((int)Char.GetNumericValue(tmpop[2]),
                                (int)Char.GetNumericValue(tmpop[4]));
                            if (tmpop.Length < 7) { tmpop = tmpop + ":0:"; }
                            NHit.S_Volume = (int)Char.GetNumericValue(tmpop[6]);
                        }
                        else
                        {
                            NHit.sample = new CSample((int)Char.GetNumericValue(tmpop[0]),
                                (int)Char.GetNumericValue(tmpop[2]));
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
                    break;
                case ObjectFlag.Slider:
                    NHit.allhitsound = Convert.ToInt32(picknext(ref row));
                    tmpop = picknext(ref row);
                    //ignore all anthor
                    tmpop = picknext(ref row);
                    if (tmpop != "") { NHit.repeatcount = Convert.ToInt32(tmpop); }
                    else { NHit.repeatcount = 1; }
                    tmpop = picknext(ref row);
                    if (tmpop != "") { NHit.length = Convert.ToDouble(tmpop); }
                    else { NHit.length = 0; }
                    tmpop = picknext(ref row);
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
                    tmpop = picknext(ref row);
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
                    tmpop = picknext(ref row);
                    if (tmpop != "")
                    {
                        if (tmpop.Length > 3)
                        {
                            NHit.sample = new CSample((int)Char.GetNumericValue(tmpop[0]),
                                (int)Char.GetNumericValue(tmpop[4]));
                            NHit.A_sample = new CSample((int)Char.GetNumericValue(tmpop[2]),
                                (int)Char.GetNumericValue(tmpop[4]));
                            if (tmpop.Length < 7) { tmpop = tmpop + ":0:"; }
                            NHit.S_Volume = (int)Char.GetNumericValue(tmpop[6]);
                        }
                        else
                        {
                            NHit.sample = new CSample((int)Char.GetNumericValue(tmpop[0]),
                                (int)Char.GetNumericValue(tmpop[2]));
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
        private Timing settiming(string row)
        {
            Timing Ntiming = new Timing();
            string tmpop = "";
            Ntiming.offset = (int)(Convert.ToDouble(picknext(ref row)));
            Ntiming.bpm = Convert.ToDouble(picknext(ref row));
            tmpop = picknext(ref row);
            if (tmpop == "") { Ntiming.meter = 4; }
            else { Ntiming.meter = Convert.ToInt32(tmpop); }
            tmpop = picknext(ref row);
            if (tmpop == "") { Ntiming.sample = new CSample((int)TSample.Normal, 0); }
            else { Ntiming.sample = new CSample(Convert.ToInt32(tmpop), Convert.ToInt32(picknext(ref row))); }
            tmpop = picknext(ref row);
            if (tmpop == "") { Ntiming.volume = 1.0f; }
            else { Ntiming.volume = Convert.ToSingle(tmpop) / 100; }
            tmpop = picknext(ref row);
            if (tmpop == "") { Ntiming.type = 1; }
            else { Ntiming.type = Convert.ToInt32(tmpop); }
            tmpop = picknext(ref row);
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
        private enum osuFileScanStatus
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
        public void GetDetail(string osb_F)
        {
            osb = osb_F;
            Path = System.IO.Path.Combine(Location, Name);
            StreamReader reader;
            osuFileScanStatus position = osuFileScanStatus.VERSION_UNKNOWN;
            try
            {
                using (reader = new StreamReader(Path))
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
                            position = (osuFileScanStatus)Enum.Parse(typeof(osuFileScanStatus), (row.Substring(1, row.Length - 2).ToUpper()));
                            continue;
                        }
                        switch (position)
                        {
                            case osuFileScanStatus.VERSION_UNKNOWN:
                                Rawdata[(int)OSUfile.FileVersion] = row.Substring(17);
                                break;
                            case osuFileScanStatus.GENERAL:
                            case osuFileScanStatus.METADATA:
                            case osuFileScanStatus.DIFFICULTY:
                                string[] s = row.Split(new char[] { ':' }, 2);
                                try { Rawdata[(int)(OSUfile)Enum.Parse(typeof(OSUfile), (s[0].Trim()))] = s[1].Trim(); }
                                catch { }
                                break;
                            case osuFileScanStatus.EVENTS:
                                if (row.StartsWith("0,0,"))
                                {
                                    string str = row.Substring(5, row.Length - 6);
                                    if (str.Contains("\""))
                                    {
                                        backgroundOffset = str.Substring(str.IndexOf("\"") + 2);
                                        str = str.Substring(0, str.IndexOf("\""));
                                    }
                                    Background = System.IO.Path.Combine(Location, str);
                                }
                                else if (row.StartsWith("1,") || row.StartsWith("Video"))
                                {
                                    haveVideo = true;
                                    string[] vdata = row.Split(new char[] { ',' });
                                    VideoOffset = Convert.ToInt32(vdata[1]);
                                    Video = vdata[2].Substring(1, System.Convert.ToInt32(vdata[2].Length - 2));
                                }
                                else if (row.StartsWith("3,") || row.StartsWith("2,")) { break; }
                                else { haveSB = true; }
                                break;
                            case osuFileScanStatus.TIMINGPOINTS:
                                Timingpoints.Add(settiming(row));
                                break;
                            case osuFileScanStatus.HITOBJECTS:
                                HitObjects.Add(setobject(row));
                                break;
                        }
                    }
                }
                if (osb != null) { haveSB = true; }
                detailed = true;
            }
            catch (SystemException e)
            {
                Console.WriteLine(e.StackTrace);
                throw (new FormatException("Failed to read .osu file" + this.Path, e));
            }
        }
        public void Getsb()
        {
            if (haveSB)
            {
                SB = new OSUFiles.StoryBoard.StoryBoard(Path, osb);
            }
        }
        public Beatmap()
        {
            Background = "";
            haveSB = false;
            haveVideo = false;
        }
        public Beatmap(string path, string location)
        {
            Path = path;
            Location = location;
        }
        public int CompareTo(Beatmap other)
        {
            if (this.Mode < other.Mode) { return -1; }
            if (this.Mode > other.Mode) { return 1; }
            if (this.HitObjects.Count == other.HitObjects.Count) { return 0; }
            if (this.HitObjects.Count > other.HitObjects.Count) { return -1; } else { return 1; }
        }
        /// <summary>
        /// 判断相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(Beatmap obj)
        {
            Beatmap b = obj;
            if ((b.beatmapId == beatmapId) && (beatmapId != 0))
            { return true; }
            return this.ToString().Equals(b.ToString()) && this.Creator.Equals(b.Creator);
        }
        public override string ToString() { return Version; }
        public string NameToString()
        {
            return (Name.Substring(0, Name.LastIndexOf('.')));
        }
        public string GetHash()
        {
            if (hash != null) { return hash; }
            string strHashData = "";
            byte[] arrHashValue;
            using (MD5 md5Hash = MD5.Create())
            {
                using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    arrHashValue = md5Hash.ComputeHash(fs);
                }
                strHashData = BitConverter.ToString(arrHashValue);
                strHashData = strHashData.Replace("-", "");
            }
            hash = strHashData.ToLower();
            return hash;
        }
    }
}
