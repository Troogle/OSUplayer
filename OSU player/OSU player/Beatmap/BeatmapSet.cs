using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
namespace OSU_player
{
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
                name = tmpbm.ArtistRomanized + " - " + tmpbm.TitleRomanized;
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
                bm.GetDetail(OsbPath);
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
