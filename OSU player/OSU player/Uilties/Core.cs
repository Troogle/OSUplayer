using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Drawing;
using OSU_player.Properties;
using System.Threading;
using Telerik.WinControls.UI;
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
        public int score;
        public modes mode;
        public string mod;
        public int hit300;
        public int hit320;
        public int hit200;
        public int hit100;
        public int hit50;
        public int totalhit;
        public int miss;
        public int maxCombo;
        public DateTime time;
        public double acc;
    }
    public class Core
    {
        #region 数据区
        /// <summary>
        /// OSU的路径
        /// </summary>
        public static string osupath;
        /// <summary>
        /// 所有的set
        /// </summary>
        public static List<BeatmapSet> allsets = new List<BeatmapSet>();
        public static List<int> PlayList = new List<int>();
        public static Dictionary<string, List<Score>> Scores = new Dictionary<string, List<Score>>();
        public static Dictionary<string, List<int>> Collections = new Dictionary<string, List<int>>();
        public static bool scoresearched = false;
        public static System.Drawing.Image defaultBG = Properties.Resources.defaultBG;
        public static string defaultAudio = Application.StartupPath + "\\Default\\" + "blank.wav";
        public static float Allvolume = 1.0f;
        public static float Musicvolume = 0.8f;
        public static float Fxvolume = 0.6f;
        public static int Nextmode = 3;
        /// <summary>
        /// 选定的QQ号
        /// </summary>
        public static int uin;
        public static bool syncQQ = true;
        public static QQ uni_QQ = new QQ();
        public static bool playvideo = true;
        public static bool playfx = true;
        public static bool playsb = true;
        public static bool needsave = false;
        #endregion
        private static Player player;
        public static int currentset = 0;
        public static int currentmap = 0;
        public static int tmpset = 0;
        public static int tmpmap = 0;
        public static BeatmapSet CurrentSet { get { return allsets[PlayList[currentset]]; } }
        public static Beatmap CurrentBeatmap { get { return CurrentSet.Diffs[currentmap]; } }
        public static BeatmapSet TmpSet { get { return allsets[PlayList[tmpset]]; } }
        public static Beatmap TmpBeatmap { get { return TmpSet.Diffs[tmpmap]; } }
        public static Size size;
        public static IntPtr handle;
        public static void init(IntPtr Shandle, Size Ssize)
        {
            Getpath();
            LoadPreference();
            new Thread(new ThreadStart(Selfupdate.check_update)).Start();
            initset();
            size = Ssize;
            handle = Shandle;
            player = new Player();
        }
        public static void exit()
        {
            uni_QQ.Send2QQ(uin, "");
            player.Dispose();
            if (needsave) { SaveList(); }
        }
        #region 方法区
        #region 初始化有关
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
                RadMessageBox.Show("读取游戏目录出错! 请手动指定", "错误", MessageBoxButtons.OK, RadMessageIcon.Error);
                while (!Setpath()) { };
            }
        }
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
                        RadMessageBox.Show("为什么你总是想卖萌OAO", "错误", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                RadMessageBox.Show("为什么你总是想卖萌OxO", "错误", MessageBoxButtons.OK, RadMessageIcon.Error);
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
        /// <summary>
        /// 保存全局Set
        /// </summary>
        public static void SaveList()
        {
            using (FileStream fs = new FileStream("list.db", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, allsets);
            }
        }
        public static void SetQQ(bool show = true)
        {
            if (show) { using (Form2 dialog = new Form2()) { dialog.ShowDialog(); } }
            if (uin == 0)
            {
                Properties.Settings.Default.QQuin = 0;
                syncQQ = false;
                Properties.Settings.Default.SyncQQ = false;
            }
            else
            {
                Properties.Settings.Default.QQuin = uin;
                syncQQ = true;
                Properties.Settings.Default.SyncQQ = true;
            }
        }
        public static void initplaylist()
        {
            for (int i = 0; i < allsets.Count; i++)
            {
                PlayList.Add(i);
            }
        }
        public static void LoadPreference()
        {
            uin = Properties.Settings.Default.QQuin;
            syncQQ = Properties.Settings.Default.SyncQQ;
            if (uin == 0)
            {
                if (
                    RadMessageBox.Show("木有设置过QQ号，需要现在设置么？", "提示", MessageBoxButtons.YesNo)
                    == DialogResult.Yes) { SetQQ(); }
                else { SetQQ(false); }
            }
            Allvolume = Properties.Settings.Default.Allvolume;
            Fxvolume = Properties.Settings.Default.Fxvolume;
            Musicvolume = Properties.Settings.Default.Musicvolume;
            playfx = Properties.Settings.Default.PlayFx;
            playsb = Properties.Settings.Default.PlaySB;
            playvideo = Properties.Settings.Default.PlayVideo;
            Nextmode = Properties.Settings.Default.NextMode;
        }
        public static void initset()
        {
            if (File.Exists("list.db") && LoadList())
            {
                initplaylist();
            }
            else
            {
                RadMessageBox.Show("将开始初始化");
                if (File.Exists(Path.Combine(Core.osupath, "osu!.db")))
                {
                    OsuDB.ReadDb(Path.Combine(Core.osupath, "osu!.db"));
                }
                initplaylist();
                RadMessageBox.Show(string.Format("初始化完毕，发现曲目{0}个", allsets.Count));
                needsave = true;
            }
            currentset = 0;
            currentmap = 0;
            tmpset = 0;
            tmpmap = 0;
        }
        #endregion
        public static void remove(int index)
        {
            //  Core.allsets.RemoveAt(PlayList[index]);
            PlayList.RemoveAt(index);
            //  needsave = true;
        }
        public static bool SetSet(int vaule, bool p = false)
        {
            tmpset = vaule;
            if (!TmpSet.check())
            {
                RadMessageBox.Show("没事删什么曲子啊><", ">_<");
                remove(tmpset);
                return true;
            }
            if (!TmpSet.detailed)
            {
                TmpSet.GetDetail();
            }
            if (p) { currentset = tmpset; }
            return false;
        }
        public static bool SetMap(int vaule, bool p = false)
        {
            tmpmap = vaule;
            if (!File.Exists(TmpBeatmap.Path))
            {
                RadMessageBox.Show("没事删什么曲子啊><", ">_<");
                remove(tmpset);
                return true;
            }
            if (p) { currentmap = tmpmap; }
            return false;
        }
        public static void SetVolume(int set, float volume)
        {
            switch (set)
            {
                case 1:
                    Allvolume = volume;
                    Properties.Settings.Default.Allvolume = Allvolume;
                    break;
                case 2:
                    Musicvolume = volume;
                    Properties.Settings.Default.Musicvolume = Musicvolume;
                    break;
                case 3:
                    Fxvolume = volume;
                    Properties.Settings.Default.Fxvolume = Fxvolume;
                    break;
                default:
                    break;
            }
            player.SetVolume(set, volume);
        }
        public static void RefreashSet()
        {
            File.Delete("list.db");
            Stop();
            allsets.Clear();
            initset();
        }
        public static void Stop()
        {
            player.Stop();
        }
        public static void Play()
        {
            if (!CurrentSet.detailed) { CurrentSet.GetDetail(); }
            if (!File.Exists(CurrentBeatmap.Audio)) { RadMessageBox.Show("丧心病狂不？音频文件你都删！"); return; }
            player.Play();
            uni_QQ.Send2QQ(uin, CurrentBeatmap.NameToString());
        }
        public static void Pause()
        {
            player.Pause();
            uni_QQ.Send2QQ(Core.uin, "");
        }
        public static void Resume()
        {
            player.Resume();
            uni_QQ.Send2QQ(Core.uin, CurrentBeatmap.NameToString());
        }
        public static void seek(double time)
        {
            if (player.durnation - 10 > time)
            {
                player.seek(time);
            }
        }
        public static int GetNext()
        {
            int next;
            int now = currentset;
            switch (Nextmode)
            {
                case 1: next = (now + 1) % PlayList.Count;
                    break;
                case 2: next = now;
                    break;
                case 3: next = new Random().Next() % PlayList.Count;
                    break;
                default: next = 0;
                    break;
            }
            currentset = next;
            currentmap = 0;
            return next;
        }
        public static void search(string k)
        {
			List<int> searchedMaps = new List<int>();
			string keyword = k.Trim().ToLower();

            if (keyword.Length == 0) {
				PlayList.Clear();
                initplaylist();
            } else {
                // PlayList.Clear();
                for (int i = 0; i < allsets.Count; i++) {
                    if (allsets[i].tags.ToLower().Contains(keyword)) {
						searchedMaps.Add(i);
                    }
                }
				if (searchedMaps.Count == 0) {
					RadMessageBox.Show("神马都木有找到！");
				} else {
					PlayList.Clear();
					for (int i = 0; i < searchedMaps.Count; i++) {
						// Console.WriteLine(string.Format("Adding: {0}/{1}", i, searchedMaps.Count));
						PlayList.Add(searchedMaps[i]);
					}
				}

            }
        }
        public static double durnation
        { get { return player.durnation; } }
        public static double position
        { get { return player.position; } }
        public static bool isplaying
        { get { return player.isplaying; } }
        public static bool willnext
        { get { return player.willnext; } }
        public static ListViewItem[] getdetail()
        {
            ListViewItem[] Detail = new ListViewItem[16];
            Detail[0] = new ListViewItem("歌曲名称");
            Detail[0].SubItems.Add(TmpBeatmap.Title);
            Detail[1] = new ListViewItem("歌手");
            Detail[1].SubItems.Add(TmpBeatmap.Artist);
            Detail[2] = new ListViewItem("作者");
            Detail[2].SubItems.Add(TmpBeatmap.Creator);
            Detail[3] = new ListViewItem("来源");
            Detail[3].SubItems.Add(TmpBeatmap.Source);
            Detail[4] = new ListViewItem("模式");
            Detail[4].SubItems.Add(Enum.GetName(typeof(modes), TmpBeatmap.Mode));
            Detail[5] = new ListViewItem("SetID");
            Detail[5].SubItems.Add(TmpBeatmap.beatmapsetId.ToString());
            Detail[6] = new ListViewItem("ID");
            Detail[6].SubItems.Add(TmpBeatmap.beatmapId.ToString());
            Detail[7] = new ListViewItem("音频文件名称");
            Detail[7].SubItems.Add(TmpBeatmap.Audio);
            Detail[8] = new ListViewItem("背景文件名称");
            Detail[8].SubItems.Add(TmpBeatmap.Background);
            Detail[9] = new ListViewItem("视频文件名称");
            Detail[9].SubItems.Add(TmpBeatmap.Video);
            Detail[10] = new ListViewItem("OSU文件版本");
            Detail[10].SubItems.Add(TmpBeatmap.FileVersion);
            Detail[11] = new ListViewItem("HP");
            Detail[11].SubItems.Add(TmpBeatmap.HPDrainRate.ToString());
            Detail[12] = new ListViewItem("CS");
            Detail[12].SubItems.Add(TmpBeatmap.CircleSize.ToString());
            Detail[13] = new ListViewItem("OD");
            Detail[13].SubItems.Add(TmpBeatmap.OverallDifficulty.ToString());
            Detail[14] = new ListViewItem("AR");
            Detail[14].SubItems.Add(TmpBeatmap.ApproachRate.ToString());
            Detail[15] = new ListViewItem("md5");
            Detail[15].SubItems.Add(TmpBeatmap.GetHash());
            return Detail;
        }
        public static ListViewDataItem[] getscore(Font font)
        {
            ListViewDataItem[] ret = new ListViewDataItem[255];
            int cur = 0;
            for (int i = 0; i < TmpSet.count; i++)
            {
                if (Core.Scores.ContainsKey(TmpSet.Diffs[i].GetHash()))
                {
                    foreach (Score tmp in Core.Scores[TmpSet.Diffs[i].GetHash()])
                    {
                        ListViewDataItem tmpl = new ListViewDataItem();
                        tmpl.Image = Core.getrank(tmp);
                        tmpl.Text = String.Format(
                            "<html>Player:{0},Date:{1},Score: {2}<br>Diff:{3},Mods:{4},Mode:{5}<br>300:{6},100:{7},50:{8},Miss:{9},Maxcombo:{10}</html>",
                            tmp.player, tmp.time.ToString(), tmp.score, TmpSet.Diffs[i].Version,
                            tmp.mod, tmp.mode.ToString(),
                            tmp.hit300, tmp.hit100, tmp.hit50, tmp.miss, tmp.maxCombo);
                        tmpl.Font = font;
                        ret[cur] = tmpl;
                        cur++;
                    }
                }
            }
            return ret;
        }
        public static void AddRangeSet(List<int> sets)
        {
            foreach (var set in sets)
            {
                AddSet(set);
            }
        }
        public static void AddSet(int setno)
        {
            if (!PlayList.Contains(setno)) { PlayList.Add(setno); }
        }
        public static Image getbg()
        {
            if (CurrentBeatmap.Background != "" && !File.Exists(CurrentBeatmap.Background))
            {
                RadMessageBox.Show("没事删什么BG！", "错误", MessageBoxButtons.OK, RadMessageIcon.Error);
                CurrentBeatmap.Background = "";
            }
            if (CurrentBeatmap.Background == "")
            {
                return player.Resize(Core.defaultBG);
            }
            else { return player.Resize(Image.FromFile(CurrentBeatmap.Background)); }
        }
        #endregion
        #region 通用转换区
        public static string modconverter(long mod)
        {
            string cmod = "";
            if (mod == 0) { cmod = "None"; }
            else
            {
                for (int i = 0; i < (int)mods.Random; i++)
                {
                    if ((mod & 1) == 1)
                    {
                        cmod += " " + Enum.GetName(typeof(mods), i);
                    }
                    mod = mod >> 1;
                }
            }
            return cmod;
        }
        public static Image getrank(Score S)
        {
            switch (S.mode)
            {
                case modes.Osu:
                    if (S.acc == 1)
                    {
                        if (S.mod.Contains("HD") || S.mod.Contains("FL")) { return Resources.XH; }
                        else { return Resources.X; }
                    }
                    if ((S.hit300) / (double)S.totalhit > 0.9 && S.hit50 / (double)S.totalhit < 0.01 && S.miss == 0)
                    {
                        if (S.mod.Contains("HD") || S.mod.Contains("FL")) { return Resources.SH; }
                        else { return Resources.S; }
                    }
                    if (((S.hit300) / (double)S.totalhit > 0.9) || ((S.hit300) / (double)S.totalhit > 0.8 && S.miss == 0))
                    {
                        return Resources.A;
                    }
                    if (((S.hit300) / (double)S.totalhit > 0.8) || ((S.hit300) / (double)S.totalhit > 0.7 && S.miss == 0))
                    {
                        return Resources.B;
                    }
                    if ((S.hit300) / (double)S.totalhit > 0.6)
                    {
                        return Resources.C;
                    }
                    return Resources.D;
                case modes.Taiko:
                    if (S.acc == 1)
                    {
                        if (S.mod.Contains("HD") || S.mod.Contains("FL")) { return Resources.XH; }
                        else { return Resources.X; }
                    }
                    if ((S.hit300) / (double)S.totalhit > 0.9 && S.hit50 / (double)S.totalhit < 0.01 && S.miss == 0)
                    {
                        if (S.mod.Contains("HD") || S.mod.Contains("FL")) { return Resources.SH; }
                        else { return Resources.S; }
                    }
                    if (((S.hit300) / (double)S.totalhit > 0.9) || ((S.hit300) / (double)S.totalhit > 0.8 && S.miss == 0))
                    {
                        return Resources.A;
                    }
                    if (((S.hit300) / (double)S.totalhit > 0.8) || ((S.hit300) / (double)S.totalhit > 0.7 && S.miss == 0))
                    {
                        return Resources.B;
                    }
                    if ((S.hit300) / (double)S.totalhit > 0.6)
                    {
                        return Resources.C;
                    }
                    return Resources.D;
                case modes.CTB:
                    if (S.acc == 1)
                    {
                        if (S.mod.Contains("HD") || S.mod.Contains("FL")) { return Resources.XH; }
                        else { return Resources.X; }
                    }
                    if (S.acc >= 0.9801)
                    {
                        if (S.mod.Contains("HD") || S.mod.Contains("FL")) { return Resources.SH; }
                        else { return Resources.S; }
                    }
                    if (S.acc >= 0.9401)
                    {
                        return Resources.A;
                    }
                    if (S.acc >= 0.9001)
                    {
                        return Resources.B;
                    }
                    if (S.acc >= 0.8501)
                    {
                        return Resources.C;
                    }
                    return Resources.D;
                case modes.Mania:
                    if (S.acc == 1)
                    {
                        if (S.mod.Contains("HD") || S.mod.Contains("FL")) { return Resources.XH; }
                        else { return Resources.X; }
                    }
                    if (S.acc > 0.95)
                    {
                        if (S.mod.Contains("HD") || S.mod.Contains("FL")) { return Resources.SH; }
                        else { return Resources.S; }
                    }
                    if (S.acc > 0.90)
                    {
                        return Resources.A;
                    }
                    if (S.acc > 0.80)
                    {
                        return Resources.B;
                    }
                    if (S.acc > 0.70)
                    {
                        return Resources.C;
                    }
                    return Resources.D;
                default:
                    return Resources.D;
            }
        }
        public static double getacc(Score S)
        {
            switch (S.mode)
            {
                case modes.Osu:
                    return (S.hit300 * 6 + S.hit100 * 2 + S.hit50)
                        / (double)((S.hit300 + S.hit100 + S.hit50 + S.miss) * 6);
                case modes.Taiko:
                    return (S.hit300 * 2 + S.hit100)
                        / (double)((S.hit300 + S.hit100 + S.miss) * 2);
                case modes.CTB:
                    return (S.hit300 + S.hit100 + S.hit50)
                        / (double)(S.hit300 + S.hit100 + S.hit50 + S.hit200 + S.miss);
                case modes.Mania:
                    return (S.hit300 * 6 + S.hit320 * 6 + S.hit200 * 4 + S.hit100 * 2 + S.hit50)
                        / (double)((S.hit300 + S.hit320 + S.hit200 + S.hit100 + S.hit50 + S.miss) * 6);
                default:
                    return 0;
            }
        }
        #endregion
    }
}