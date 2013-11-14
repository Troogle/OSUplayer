using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
namespace OSU_player.OSUFiles
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
    }
    public enum modes
    {
        Osu = 0,
        Taiko = 1,
        CTB = 2,
        Mania = 3
    }
    public enum mods
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
        Random
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
        public List<string> md5 = new List<string>();
        [NonSerialized()]
        public bool detailed = false;
        public List<Beatmap> Diffs;
        public string tags;
        private string check(string pre, string mid, string end)
        {
            if (File.Exists(pre + mid + end + ".wav"))
            {
                return pre + mid + end + ".wav";
            }
            else
            {
                if (File.Exists(pre + mid + end + ".mp3"))
                {
                    return pre + mid + end + ".mp3";
                }
                else
                    return Application.StartupPath + "\\Default\\" + mid + ".wav";
                // return "";
            }
        }
        /// <summary>
        /// 取需要播放的音效完整目录
        /// </summary>
        /// <param name="sample">需要处理的sample类型</param>
        /// <param name="soundtype">需要处理的音效</param>
        /// <returns>当前需要播放音效的List<string></returns>
        public List<string> getsamplename(CSample sample, int soundtype)
        {
            List<string> tmp = new List<string>();
            if (sample.sample == 0) { return tmp; }
            if (sample.sampleset == 0)
            {
                string all = Application.StartupPath + "\\Default\\" + Enum.GetName(typeof(TSample), sample.sample);
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
            if (sample.sampleset == 1)
            {
                last = "";
            }
            else
            {
                last = sample.sampleset.ToString();
            }
            //normal-sliderslide(loops)
            //normal-sliderwhistle(loops)
            //normal-slidertick
            //不考虑以上
            string first = location + "\\";
            if (soundtype % 2 == 1 || soundtype == 0)
            {
                tmp.Add(check(first, Enum.GetName(typeof(TSample), sample.sample) + "-hitnormal", last));
            }
            soundtype = (int)(soundtype / 2);
            if (soundtype % 2 == 1)
            {
                tmp.Add(check(first, Enum.GetName(typeof(TSample), sample.sample) + "-hitwhistle", last));
            }
            soundtype = (int)(soundtype / 2);
            if (soundtype % 2 == 1)
            {
                tmp.Add(check(first, Enum.GetName(typeof(TSample), sample.sample) + "-hitfinish", last));
            }
            soundtype = (int)(soundtype / 2);
            if (soundtype % 2 == 1)
            {
                tmp.Add(check(first, Enum.GetName(typeof(TSample), sample.sample) + "-hitclap", last));
            }
            return tmp;
        }
        public void add(Beatmap tmpbm)
        {
            if (count == 0)
            {
                location = tmpbm.Location;
                name = tmpbm.Artist + " - " + tmpbm.Title;
                setid = tmpbm.beatmapsetId;
                tags = tmpbm.tags;
                tags += " " + tmpbm.ArtistRomanized;
                tags += " " + tmpbm.TitleRomanized;
                tags += " " + tmpbm.Creator;
                tags += " " + tmpbm.Source;
            }
            count++;
            Diffs.Add(tmpbm);
            md5.Add(tmpbm.GetHash());
            tags += " " + tmpbm.Version;
        }
        public BeatmapSet()
        {
            count = 0;
            Diffs = new List<Beatmap>();
        }
        public void GetDetail()
        {
            DirectoryInfo F = new DirectoryInfo(location);
            FileInfo[] osbfiles = F.GetFiles("*.osb");
            if (osbfiles.Length != 0)
            {
                OsbPath = osbfiles[0].FullName;
            }
            foreach (Beatmap bm in Diffs)
            {
                if (!bm.detailed){bm.GetDetail(OsbPath);}
            }
            Diffs.Sort();
            detailed = true;
        }
        public bool check()
        {
            if (!Directory.Exists(location)) { return false; }
            return true;
        }
        public override string ToString()
        {
            return name;
        }
        public bool Contains(Beatmap tmpbm)
        {
            if (location == tmpbm.Location) { return true; } else { return false; }
            /* if (setid == -1) {
            if (name==tmpbm.ArtistRomanized + " - " + tmpbm.TitleRomanized){return true;}else{return false;}
            }
            if (tmpbm.beatmapsetId == setid) { return true; }
            return false;*/
        }
    }
}
