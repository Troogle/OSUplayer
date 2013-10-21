// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

using System.IO;
//using OSU_player.Core;

namespace OSU_player
{
	public class BeatmapSet
	{
		public string location;
		public string Osbfilename;
		public int count; //number of diffs
		public string name;
		public List<string> diffstr = new List<string>();
		public bool detailed = false;
		public List<Beatmap> Diffs = new List<Beatmap>();
		private string check(string url)
		{
			if (File.Exists(Path.Combine(location, url + ".wav")))
			{
				return (url + ".wav");
			}
			else
			{
				return (url + ".mp3");
			}
		}
		public List<string> getsamplename(CSample sample, int soundtype, int objecttype)
		{
			List<string> tmp = new List<string>();
            if (objecttype == (int)ObjectFlag.Spinner | objecttype == (int)ObjectFlag.SpinnerNewCombo)
			{
				return tmp;
			}
			string last = "";
			if (sample.sampleset == 0)
			{
                string all = (string)(Path.Combine(Application.StartupPath, "\\defalut\\") + Enum.GetName(typeof(TSample), sample.sample));
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
					tmp.Add(all + "-finish.wav");
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
			string first = "";
            first = Enum.GetName(typeof(TSample), sample.sample);
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
				tmp.Add(check(first + "-finish" + last));
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
				Osbfilename = osbfiles[0].Name;
			}
			//osb first
			FileInfo[] osufiles = F.GetFiles("*.osu");
			name = osufiles[0].Name;
			name = name.Substring(0, name.LastIndexOf("("));
			foreach (var s in osufiles)
			{
				count++;
				string filename = s.Name;
				Beatmap bm = new Beatmap(location, filename, Osbfilename);
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
