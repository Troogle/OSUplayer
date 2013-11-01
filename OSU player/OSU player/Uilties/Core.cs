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
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSU_player
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
    /// <summary>
    /// 获取的QQ信息
    /// </summary>
    public struct QQInfo
    {
        public int uin;
        public string nick;
        public QQInfo(int uin, string nick)
        {
            this.uin = uin;
            this.nick = nick;
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
    public struct Score
    {
        public string player;
        public string rank;
        public int score;
        public modes mode;
        public string mod;
        public int hit300;
        public int hit100;
        public int hit50;
        public int miss;
        public int maxCombo;
        public DateTime time;
        public double acc;
    }
    public class Core
    {
        public Core()
        {
        }
        /// <summary>
        /// OSU的路径
        /// </summary>
        public static string osupath;
        /// <summary>
        /// 所有的set
        /// </summary>
        public static List<BeatmapSet> allsets = new List<BeatmapSet>();
        public static Dictionary<string, List<Score>> Scores = new Dictionary<string, List<Score>>();
        public static Dictionary<string, List<int>> Collections = new Dictionary<string, List<int>>();
        public static string defaultBG = Path.Combine(Application.StartupPath, "default\\") + "defaultBG.png";
        public static string defaultAudio = Path.Combine(Application.StartupPath, "default\\") + "blank.wav";
        public static bool scoresearched = false;
        /// <summary>
        /// 选定的QQ号
        /// </summary>
        public static int uin;
        /// <summary>
        /// 获取OSU路径
        /// </summary>
        public static void Getpath()
        {
            string str = "";
            try
            {
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("osu!\\shell\\open\\command");
                str = rk.GetValue("").ToString();
                str = str.Substring(1, str.IndexOf("\"", 1) - 9);
                osupath = str;
            }
            catch (Exception)
            {
                MessageBox.Show("读取游戏目录出错! 请手动指定", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                while (!Setpath()) { };
            }
        }
        public static bool syncQQ = true;
        /// <summary>
        /// 设置路径
        /// </summary>
        /// <returns>是否选择正常路径</returns>
        public static bool Setpath()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            dialog.Description = "请选择与osu!.exe同级的目录~";
            try
            {
                if (DialogResult.OK == dialog.ShowDialog())
                {
                    if (File.Exists(dialog.SelectedPath + "\\osu!.exe"))
                    {
                        osupath = dialog.SelectedPath;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("为什么你总是想卖萌OAO", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("为什么你总是想卖萌OxO", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        /// <summary>
        /// 载入播放列表
        /// </summary>
        /// <returns>是否正常载入</returns>
        public static bool LoadList()
        {
            try
            {
                using (FileStream fs = new FileStream("list.db", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    allsets = (List<BeatmapSet>)formatter.Deserialize(fs);
                }
                return true;
            }
            catch
            {
                File.Delete("list.db");
                return false;
            }
        }
        public static void SaveList()
        {
            using (FileStream fs = new FileStream("list.db", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, allsets);
            }
        }
        public static string modconverter(int mod)
        {
            string cmod = "";
            if (mod == 0) { cmod = "None"; }
            else
            {
                for (int i = 0; i < (int)mods.Random; i++)
                {
                    if ((mod & 1) == 1) { cmod += " "+Enum.GetName(typeof(mods), i); }
                    mod = mod >> 2;
                }
            }
            return cmod;
        }
        /*   public static void findsetsbyfolder()
           {
               MessageBox.Show("将开始初始化");
               try
               {
                   if (Directory.Exists(Path.Combine(Core.osupath, "Songs")))
                   {
                       this.backgroundWorker1.RunWorkerAsync(Path.Combine(Core.osupath, "Songs"));
                   }
               }
               catch (SystemException ex)
               {
                   Console.WriteLine(ex.StackTrace);
                   throw (new FormatException("Failed to read song path", ex));
               }
           }
                 private void scanforset(string path)
           {
               string[] osufiles = Directory.GetFiles(path, "*.osu");
               if (osufiles.Length != 0)
               {
                   BeatmapSet tmp = new BeatmapSet(path);
                   //tmp.GetDetail();
                   Core.allsets.Add(tmp);
                   this.backgroundWorker1.ReportProgress(0, tmp.ToString());
               }
               else
               {
                   string[] tmpfolder = Directory.GetDirectories(path);
                   foreach (string subfolder in tmpfolder)
                   {
                       scanforset(subfolder);
                   }
               }
           }
                 private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
           scanforset(e.Argument.ToString());
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString().Length > 0)
            {
                ListViewItem tmpl = new ListViewItem(e.UserState.ToString());
                PlayList.Items.Add(tmpl);
            }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (ListViewItem item in PlayList.Items)
            {
                item.SubItems.Add(item.Index.ToString());
                FullList.Add(item);
            }
            MessageBox.Show(string.Format("初始化完毕，发现曲目{0}个", Core.allsets.Count));
            PlayList.Items[0].Selected = true;
            Core.SaveList();
        }*/
    }
}