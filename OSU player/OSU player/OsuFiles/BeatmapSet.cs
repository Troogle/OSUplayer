using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
namespace OSUplayer.OsuFiles
{
    /// <summary>
    /// OSU文件内属性
    /// </summary>
    public enum OSUfile
    {
        FileVersion,
        AudioFilename,
        AudioHash,
        AudioLeadIn,
        PreviewTime,
        Countdown,
        SampleSet,
        StackLeniency,
        Mode,
        LetterboxInBreaks,
        StoryFireInFront,
        EpilepsyWarning,
        CountdownOffset,
        WidescreenStoryboard,
        EditorBookmarks,
        EditorDistanceSpacing,
        UseSkinSprites,
        OverlayPosition,
        SkinPreference,
        SpecialStyle,
        CustomSamples,
        Title,
        TitleUnicode,
        Artist,
        ArtistUnicode,
        Creator,
        Version,
        Source,
        Tags,
        BeatmapID,
        BeatmapSetID,
        HPDrainRate,
        CircleSize,
        OverallDifficulty,
        ApproachRate,
        SliderMultiplier,
        SliderTickRate,
        OSUfilecount
    }
    /// <summary>
    /// HitObject的类别
    /// </summary>
    public enum ObjectFlag
    {
        Normal = 1,
        Slider = 2,
        NewCombo = 4,
        NormalNewCombo = 5,
        SliderNewCombo = 6,
        Spinner = 8,
        SpinnerNewCombo = 12,
        ColourHax = 112,
        Hold = 128,
        ManiaLong = 128
    }
    /// <summary>
    /// 打击音效，可以叠加
    /// </summary>
    public enum HitSound
    {
        Normal = 0,
        Whistle = 2,
        Finish = 4,
        Clap = 8
    }
    /// <summary>
    /// 打击音效组的前缀
    /// </summary>
    public enum TSample
    {
        None = 0,
        Normal = 1,
        Soft = 2,
        Drum = 3
    }
    /// <summary>
    /// 打击音效组
    /// </summary>
    public struct CSample
    {
        public int sample;
        public int sampleset;
        public CSample(int sample, int sampleset)
        {
            this.sampleset = sampleset;
            this.sample = sample;
        }
        public static bool operator ==(CSample a, CSample b)
        {
            return a.sample == b.sample && a.sampleset == b.sampleset;
        }
        public static bool operator !=(CSample a, CSample b)
        {
            return !(a == b);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return (CSample)obj == this;
        }
    }
    public enum Modes
    {
        Osu = 0,
        Taiko = 1,
        CTB = 2,
        Mania = 3
    }
    public enum Mods
    {
        NF,
        EZ,
        NV,
        HD,
        HR,
        SD,
        DT,
        Relax,
        HT,
        NC,
        FL,
        Auto,
        SO,
        Autopilot,
        PF,
        Key4,
        Key5,
        Key6,
        Key7,
        Key8,
        Fadein,
        Random,
        Cinema,
        Target,
        LastMod
    }
    /// <summary>
    /// Timing Points
    /// </summary>
    public struct Timing
    {
        public int offset;
        public double bpm;
        public int meter;
        public CSample sample;
        public float volume;
        public int type;
        public int kiai;
    }
    public struct note
    {
        public int x;
        public int y;
    }
    /// <summary>
    /// Hitobjects
    /// </summary>
    public struct HitObject
    {
        public int x;
        public int y;
        public int starttime;
        public ObjectFlag type;
        public int allhitsound;
        public int EndTime;
        public CSample sample;
        public CSample A_sample;
        public float S_Volume;
        public char slidertype;
        public int repeatcount;
        public double length;
        public int[] Hitsounds;
        public CSample[] samples;
    }
    [Serializable]
    public class BeatmapSet
    {
        public string location;
        public string OsbPath;
        public int count; //number of diffs
        private string name;
        public int setid;
        [NonSerialized()]
        public bool Detailed = false;
        public Dictionary<String, Beatmap> Diffs;
        public string tags;
        private static string Checksample(string pre, string mid, string end)
        {
            if (File.Exists(String.Format("{0}{1}{2}.wav", pre, mid, end)))
            {
                return String.Format("{0}{1}{2}.wav", pre, mid, end);
            }
            if (File.Exists(String.Format("{0}{1}{2}.mp3", pre, mid, end)))
            {
                return String.Format("{0}{1}{2}.mp3", pre, mid, end);
            }
            return String.Format(@"{0}\Default\{1}.wav", Application.StartupPath, mid);
            // return "";
        }
        /// <summary>
        /// 取需要播放的音效完整目录
        /// </summary>
        /// <param name="sample">需要处理的sample类型</param>
        /// <param name="soundtype">需要处理的音效</param>
        /// <returns>当前需要播放音效的List string </returns>
        public IEnumerable<string> Getsamplename(CSample sample, int soundtype)
        {
            var tmp = new List<string>();
            if (sample.sample == 0) { return tmp; }
            if (sample.sampleset == 0)
            {
                string all = String.Format(@"{0}\Default\{1}", Application.StartupPath, Enum.GetName(typeof(TSample), sample.sample));
                if (soundtype % 2 == 1 || soundtype == 0)
                {
                    tmp.Add(all + "-hitnormal.wav");
                }
                soundtype = soundtype >> 2;
                if (soundtype % 2 == 1)
                {
                    tmp.Add(all + "-hitwhistle.wav");
                }
                soundtype = soundtype >> 2;
                if (soundtype % 2 == 1)
                {
                    tmp.Add(all + "-hitfinish.wav");
                }
                soundtype = soundtype >> 2;
                if (soundtype % 2 == 1)
                {
                    tmp.Add(all + "-hitclap.wav");
                }
                return tmp;
            }
            string last = "";
            last = sample.sampleset == 1 ? "" : sample.sampleset.ToString();
            //normal-sliderslide(loops)
            //normal-sliderwhistle(loops)
            //normal-slidertick
            //不考虑以上
            string first = location + "\\";
            if (soundtype % 2 == 1 || soundtype == 0)
            {
                tmp.Add(Checksample(first, Enum.GetName(typeof(TSample), sample.sample) + "-hitnormal", last));
            }
            soundtype = soundtype / 2;
            if (soundtype % 2 == 1)
            {
                tmp.Add(Checksample(first, Enum.GetName(typeof(TSample), sample.sample) + "-hitwhistle", last));
            }
            soundtype = soundtype / 2;
            if (soundtype % 2 == 1)
            {
                tmp.Add(Checksample(first, Enum.GetName(typeof(TSample), sample.sample) + "-hitfinish", last));
            }
            soundtype = soundtype / 2;
            if (soundtype % 2 == 1)
            {
                tmp.Add(Checksample(first, Enum.GetName(typeof(TSample), sample.sample) + "-hitclap", last));
            }
            return tmp;
        }
        public void Add(Beatmap tmpbm)
        {
            if (count == 0)
            {
                location = tmpbm.Location;
                name = String.Format("{0} - {1}", tmpbm.Artist, tmpbm.Title);
                setid = tmpbm.BeatmapsetID;
                tags = tmpbm.Tags;
                tags += " " + tmpbm.ArtistRomanized;
                tags += " " + tmpbm.TitleRomanized;
                tags += " " + tmpbm.Creator;
                tags += " " + tmpbm.Source;
            }
            count++;
            var bmHash = tmpbm.GetHash();
            if (!Diffs.ContainsKey(bmHash)) Diffs.Add(bmHash, tmpbm);
            else
            {
                Diffs[bmHash] = tmpbm;
                count--;
            }
            tags += " " + tmpbm.Version;
        }
        public BeatmapSet()
        {
            count = 0;
            Diffs = new Dictionary<string, Beatmap>();
        }
        public void GetDetail()
        {
            var f = new DirectoryInfo(location);
            var osbfiles = f.GetFiles("*.osb");
            if (osbfiles.Length != 0)
            {
                OsbPath = osbfiles[0].FullName;
            }
            foreach (var bm in Diffs.Values)
            {
                bm.Setsb(OsbPath);
            }
            Detailed = true;
        }
        public bool Check()
        {
            return Directory.Exists(location);
        }
        private string Hash { get; set; }
        public string GetHash()
        {
            if (Hash != null) { return Hash; }
            var strHashData = "";
            using (var md5Hash = MD5.Create())
            {
                var md5String = string.Join("", Diffs.Keys.ToArray());
                var arrHashValue = md5Hash.ComputeHash(System.Text.Encoding.ASCII.GetBytes(md5String));
                strHashData = BitConverter.ToString(arrHashValue);
                strHashData = strHashData.Replace("-", "");

            }
            Hash = strHashData.ToLower();
            return Hash;
        }

        private List<Beatmap> _difflist = new List<Beatmap>();

        public List<Beatmap> GetBeatmaps()
        {
            if (_difflist.Count == Diffs.Keys.Count) return _difflist;
            _difflist = Diffs.Values.ToList();
            _difflist.Sort();
            return _difflist;
        }
        public override string ToString()
        {
            return name;
        }
        public bool Contains(Beatmap tmpbm)
        {
            return location == tmpbm.Location;
            /* if (setid == -1) {
            if (name==tmpbm.ArtistRomanized + " - " + tmpbm.TitleRomanized){return true;}else{return false;}
            }
            if (tmpbm.beatmapsetId == setid) { return true; }
            return false;*/
        }

        public void SaveAudios(string toLocation)
        {
            foreach (var map in Diffs.Values.Distinct(new MapAudioComparer()))
            {
                map.SaveAudio(toLocation);
            }
        }

        public int ArtistCompare(BeatmapSet obj)
        {
            if (obj == null) return 1;
            if (obj.GetBeatmaps().Count == 0) return 1;
            if (_difflist.Count == 0) return -1;
            if (obj.GetBeatmaps()[0].ArtistRomanized == "") return 1;
            var comp = String.Compare(_difflist[0].ArtistRomanized, obj.GetBeatmaps()[0].ArtistRomanized,
                StringComparison.CurrentCultureIgnoreCase);
            if (comp == 0)
            {
                var tcomp = String.Compare(_difflist[0].TitleRomanized, obj.GetBeatmaps()[0].TitleRomanized,
                StringComparison.CurrentCultureIgnoreCase);
                return tcomp;
            }
            return comp;
        }
        public int TitleCompare(BeatmapSet obj)
        {
            if (obj == null) return 1;
            if (obj.GetBeatmaps().Count == 0) return 1;
            if (_difflist.Count == 0) return -1;
            if (obj.GetBeatmaps()[0].TitleRomanized == "") return 1;
            var comp = String.Compare(_difflist[0].TitleRomanized, obj.GetBeatmaps()[0].TitleRomanized,
                StringComparison.CurrentCultureIgnoreCase);
            if (comp == 0)
            {
                var acomp = String.Compare(_difflist[0].ArtistRomanized, obj.GetBeatmaps()[0].ArtistRomanized,
                StringComparison.CurrentCultureIgnoreCase);
                return acomp;
            }
            return comp;
        }
    }
    public class MapAudioComparer : IEqualityComparer<Beatmap>
    {
        public bool Equals(Beatmap x, Beatmap y)
        {
            if (x == null)
                return y == null;
            return x.Audio == y.Audio;
        }

        public int GetHashCode(Beatmap obj)
        {
            return obj.Audio.GetHashCode();
        }
    }
}
