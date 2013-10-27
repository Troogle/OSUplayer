using System;
using System.Collections.Generic;

using System.IO;
using System.Windows.Forms;

namespace OSU_player
{
	public class BeatmapSet
	{
		public string location;
		public string OsbPath;
		public int count; //number of diffs
		public string name;
		public List<string> diffstr = new List<string>();
		public bool detailed = false;
		public List<Beatmap> Diffs = new List<Beatmap>();
		private string check(string url)
		{
			if (File.Exists(Path.Combine(location, url + ".wav")))
			{
                return Path.Combine(location, url + ".wav");
			}
			else
            {
                if (File.Exists(Path.Combine(location, url + ".mp3")))
                {
                    return Path.Combine(location, url + ".mp3");
                }
                else
                    return Application.StartupPath + "\\Default\\" + "normal-hitnormal.wav";
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
            if (sample.sample == 0) { return (tmp); }
			if (sample.sampleset == 0)
			{
                string all = Application.StartupPath + "\\Default\\" + Enum.GetName(typeof(TSample), sample.sample);
				if (soundtype % 2 == 0)
				{
					tmp.Add(all + "-hitnormal.wav");
				}
				soundtype = (int) (soundtype / 2);
				if (soundtype == 0)
				{
					return tmp;
				}
				if (soundtype % 2 == 1)
				{
					tmp.Add(all + "-hitwhistle.wav");
				}
				soundtype = (int) (soundtype / 2);
				if (soundtype == 0)
				{
					return tmp;
				}
				if (soundtype % 2 == 1)
				{
					tmp.Add(all + "-hitfinish.wav");
				}
				soundtype = (int) (soundtype / 2);
				if (soundtype == 0)
				{
					return tmp;
				}
				if (soundtype % 2 == 1)
				{
					tmp.Add(all + "-hitclap.wav");
				}
				soundtype = (int) (soundtype / 2);
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
			string first =Enum.GetName(typeof(TSample), sample.sample);
			if (soundtype % 2 == 0)
			{
				tmp.Add(check(first + "-hitnormal" + last));
			}
			soundtype = (int) (soundtype / 2);
			if (soundtype == 0)
			{
				return tmp;
			}
			if (soundtype % 2 == 1)
			{
				tmp.Add(check(first + "-hitwhistle" + last));
			}
			soundtype = (int) (soundtype / 2);
			if (soundtype == 0)
			{
				return tmp;
			}
			if (soundtype % 2 == 1)
			{
				tmp.Add(check(first + "-hitfinish" + last));
			}
			soundtype = (int) (soundtype / 2);
			if (soundtype == 0)
			{
				return tmp;
			}
			if (soundtype % 2 == 1)
			{
				tmp.Add(check(first + "-hitclap" + last));
			}
			soundtype = (int) (soundtype / 2);
			return tmp;
		}
		public BeatmapSet(string path)
		{
			count = 0;
			location = path;
			DirectoryInfo F = new DirectoryInfo(location);
			FileInfo[] osbfiles = F.GetFiles("*.osb");
			if (osbfiles.Length != 0)
			{
				OsbPath = osbfiles[0].Name;
			}
			//osb first
			FileInfo[] osufiles = F.GetFiles("*.osu");
			name = osufiles[0].Name;
			name = name.Substring(0, name.LastIndexOf("("));
			foreach (var s in osufiles)
			{
				count++;
				string filename = s.Name;
				Beatmap bm = new Beatmap(location, filename, OsbPath);
				Diffs.Add(bm);
			}
		}
		public void GetDetail()
		{
			foreach (var bm in Diffs)
			{
				bm.GetDetail();
				diffstr.Add(bm.Version);
			}
			detailed = true;
		}
		public override string ToString()
		{
			return name;
		}
	}
	
}
