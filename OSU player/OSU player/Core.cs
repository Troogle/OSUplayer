using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using OSUplayer.Uilties;
using Telerik.WinControls;
using System.Drawing;
using OSUplayer.Properties;
using System.Threading;
using Telerik.WinControls.UI;
using System.Reflection;
using System.Diagnostics;
using OSUplayer.OsuFiles;
using OSUplayer.Graphic;
namespace OSUplayer
{
    public class Core
    {
        #region 数据区
        /// <summary>
        /// 程序中的所有set
        /// </summary>
        public static List<BeatmapSet> allsets = new List<BeatmapSet>();
        /// <summary>
        /// 播放列表，和显示的一一对应，int中是Set的程序内部编号
        /// </summary>
        public static List<int> PlayList = new List<int>();
        /// <summary>
        /// 本地成绩，Key是地图MD5,Value是Score
        /// </summary>
        public static Dictionary<string, List<ScoreRecord>> Scores = new Dictionary<string, List<ScoreRecord>>();
        /// <summary>
        /// 本地Collection，Key是Collection名字,Value是Set的程序内部编号的List
        /// </summary>
        public static Dictionary<string, List<int>> Collections = new Dictionary<string, List<int>>();
        /// <summary>
        /// 是否已经载入过本地成绩
        /// </summary>
        public static bool Scoresearched = false;
        /// <summary>
        /// Set有变化，需要保存
        /// </summary>
        public static bool needsave = false;
        /// <summary>
        /// 目前正在播放的Set程序内部编号
        /// </summary>
        public static int currentset = 0;
        /// <summary>
        /// 目前正在播放的Map在Set中的编号
        /// </summary>
        public static int currentmap = 0;
        /// <summary>
        /// 目前选中的Set在播放列表中的编号
        /// </summary>
        public static int tmpset = 0;
        /// <summary>
        /// 目前选中的Map在Set中的编号
        /// </summary>
        public static int tmpmap = 0;
        /// <summary>
        /// 目前正在播放的Set
        /// </summary>
        public static BeatmapSet CurrentSet { get { return allsets[currentset]; } }
        /// <summary>
        /// 目前正在播放的Map
        /// </summary>
        public static Beatmap CurrentBeatmap { get { return CurrentSet.Diffs[currentmap]; } }
        /// <summary>
        /// 目前选中的Set
        /// </summary>
        public static BeatmapSet TmpSet { get { return allsets[PlayList[tmpset]]; } }
        /// <summary>
        /// 目前选中的Map
        /// </summary>
        public static Beatmap TmpBeatmap { get { return TmpSet.Diffs[tmpmap]; } }
        #endregion
        private static Player player;

        public static string Version
        {
            get
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return fvi.FileVersion;
            }
        }
        public static bool MainIsVisible = false;
        public static void exit()
        {
            NotifySystem.ClearText();
            player.Dispose();
            if (needsave) { DBSupporter.SaveList(); }
        }
        #region 方法区
        #region 初始化有关
        /// <summary>
        /// 全局初始化
        /// </summary>
        /// <param name="Shandle">显示区域的handle</param>
        /// <param name="Ssize">显示区域的大小</param>
        public static void init(IntPtr Shandle, Size Ssize)
        {
            player = new Player(Shandle, Ssize);
            NotifySystem.Showtip(1000, "OSUplayer", "正在初始化...", System.Windows.Forms.ToolTipIcon.Info);
            Getpath();
            LoadPreference();
            new Thread(Selfupdate.check_update).Start();
            Initset();
        }
        /// <summary>
        /// 获取OSU路径
        /// </summary>
        private static void Getpath()
        {
            string str = "";
            try
            {
                var rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("osu!\\shell\\open\\command");
                str = rk.GetValue("").ToString();
                str = str.Substring(1, str.LastIndexOf(@"\"));
                Settings.Default.OSUpath = str;
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
            var dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            dialog.Description = "请选择与osu!.exe同级的目录~";
            try
            {
                if (DialogResult.OK == dialog.ShowDialog())
                {
                    if (File.Exists(dialog.SelectedPath + "\\osu!.exe"))
                    {
                        Settings.Default.OSUpath = dialog.SelectedPath;
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
        /// 设置QQ
        /// </summary>
        /// <param name="show">是否显示设置窗口</param>
        public static void SetQQ(bool show = true)
        {
            if (show) { using (SetQQ dialog = new SetQQ()) { dialog.ShowDialog(); } }
            if (Settings.Default.QQuin == "0")
            {
                Settings.Default.QQuin = "0";
                Settings.Default.SyncQQ = false;
                Settings.Default.SyncQQ = false;
            }
            else
            {
                Settings.Default.QQuin = Settings.Default.QQuin;
                Settings.Default.SyncQQ = true;
                Settings.Default.SyncQQ = true;
            }
        }
        /// <summary>
        /// 初始化播放列表，初始时与Set一一对应
        /// </summary>
        public static void initplaylist()
        {
            for (int i = 0; i < allsets.Count; i++)
            {
                PlayList.Add(i);
                //allsets[i].GetDetail();
            }
        }
        /// <summary>
        /// 载入设置
        /// </summary>
        public static void LoadPreference()
        {
            if (!Settings.Default.Upgraded)
            {
                Settings.Default.Upgrade();
                Settings.Default.Save();
            }
            if (Settings.Default.QQuin == "0")
            {
                /* if (
                     RadMessageBox.Show("木有设置过QQ号，需要现在设置么？", "提示", MessageBoxButtons.YesNo)
                     == DialogResult.Yes) { SetQQ(); }
                 else { SetQQ(false); }*/
                SetQQ();
            }
            Settings.Default.PlaySB = false;
            //playsb = Settings.Default.PlaySB;
            Settings.Default.Upgraded = true;
        }
        /// <summary>
        /// 初始化Set的总方法，从文件读取或从osu!.db读取
        /// </summary>
        public static void Initset()
        {
            if (File.Exists("list.db") && DBSupporter.LoadList())
            {
                initplaylist();
            }
            else
            {
                if (File.Exists(Path.Combine(Settings.Default.OSUpath, "osu!.db")))
                {
                    OsuDB.ReadDb(Path.Combine(Settings.Default.OSUpath, "osu!.db"));
                }
                initplaylist();
                NotifySystem.Showtip(1000, "OSUplayer", string.Format("初始化完毕，发现曲目{0}个", allsets.Count), ToolTipIcon.Info);
                needsave = true;
            }
            currentset = 0;
            currentmap = 0;
            tmpset = 0;
            tmpmap = 0;
        }
        #endregion
        public static void Remove(int index)
        {
            // Core.allsets.RemoveAt(PlayList[index]);
            PlayList.RemoveAt(index);
            needsave = true;
        }
        public static bool SetSet(int vaule, bool p = false)
        {
            tmpset = vaule;
            if (!TmpSet.check())
            {
                NotifySystem.Showtip(1000, "OSUplayer", "没事删什么曲子啊><", System.Windows.Forms.ToolTipIcon.Info);
                Remove(tmpset);
                return true;
            }
            if (!TmpSet.detailed)
            {
                TmpSet.GetDetail();
            }
            if (p) { currentset = PlayList[tmpset]; }
            return false;
        }
        public static bool SetMap(int vaule, bool p = false)
        {
            tmpmap = vaule;
            if (!TmpBeatmap.detailed)
            {
                TmpBeatmap.GetDetail();
            }
            if (!File.Exists(TmpBeatmap.Audio))
            {
                NotifySystem.Showtip(1000, "OSUplayer", "没事删什么曲子啊><", System.Windows.Forms.ToolTipIcon.Info);
                Remove(tmpset);
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
                    Settings.Default.Allvolume = volume;
                    break;
                case 2:
                    Settings.Default.Musicvolume = volume;
                    break;
                case 3:
                    Settings.Default.Fxvolume = volume;
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
            Initset();
        }
        public static void Stop()
        {
            player.Stop();
        }
        public static void Play()
        {
            if (!CurrentSet.detailed) { CurrentSet.GetDetail(); }
            if (!CurrentBeatmap.detailed) { CurrentBeatmap.GetDetail(); }
            if (!File.Exists(CurrentBeatmap.Audio))
            {
                NotifySystem.Showtip(1000, "OSUplayer", "音频文件你都删！><", System.Windows.Forms.ToolTipIcon.Info);
                return;
            }
            player.Play();
            NotifySystem.Showtip(1000, "OSUplayer", "正在播放\n" + CurrentBeatmap.NameToString(), System.Windows.Forms.ToolTipIcon.Info);
            NotifySystem.SetText(CurrentBeatmap.NameToString());
        }
        public static void PauseOrResume()
        {
            if (player.isplaying)
            {
                player.Pause();
                NotifySystem.ClearText();
            }
            else
            {
                if (player.position == -1.0) return;
                player.Resume();
                NotifySystem.SetText(CurrentBeatmap.NameToString());
            }
        }
        public static void seek(double time)
        {
            if (player.durnation - 10 > time)
            {
                player.seek(time);
            }
        }
        /// <summary>
        /// 获得下一首音乐
        /// </summary>
        /// <returns>下一首Set的Playlist编号</returns>
        public static int GetNext()
        {
            int next;
            int now = PlayList.IndexOf(currentset, 0);
            if (currentset == -1) { currentset = 0; }
            switch (Settings.Default.NextMode)
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
            currentset = PlayList[next];
            currentmap = 0;
            return next;
        }
        public static void search(string k)
        {
            List<int> searchedMaps = new List<int>();
            string keyword = k.Trim().ToLower();
            if (keyword.Length == 0)
            {
                PlayList.Clear();
                initplaylist();
            }
            else
            {
                //PlayList.Clear();
                for (int i = 0; i < allsets.Count; i++)
                {
                    if (allsets[i].tags.ToLower().Contains(keyword)) { searchedMaps.Add(i); }
                }
                if (searchedMaps.Count == 0) { PlayList.Clear(); }
                else
                {
                    PlayList.Clear();
                    for (int i = 0; i < searchedMaps.Count; i++) { PlayList.Add(searchedMaps[i]); }
                }
            }
        }
        public static double Durnation
        { get { return player.durnation; } }
        public static double Position
        { get { return player.position; } }
        public static bool Isplaying
        { get { return player.isplaying; } }
        public static bool Willnext
        { get { return player.willnext; } }
        public static ListViewItem[] getdetail()
        {
            var detail = new ListViewItem[16];
            detail[0] = new ListViewItem("歌曲名称");
            detail[0].SubItems.Add(TmpBeatmap.Title);
            detail[1] = new ListViewItem("歌手");
            detail[1].SubItems.Add(TmpBeatmap.Artist);
            detail[2] = new ListViewItem("作者");
            detail[2].SubItems.Add(TmpBeatmap.Creator);
            detail[3] = new ListViewItem("来源");
            detail[3].SubItems.Add(TmpBeatmap.Source);
            detail[4] = new ListViewItem("模式");
            detail[4].SubItems.Add(Enum.GetName(typeof(Modes), TmpBeatmap.Mode));
            detail[5] = new ListViewItem("SetID");
            detail[5].SubItems.Add(TmpBeatmap.beatmapsetId.ToString());
            detail[6] = new ListViewItem("ID");
            detail[6].SubItems.Add(TmpBeatmap.beatmapId.ToString());
            detail[7] = new ListViewItem("音频文件名称");
            detail[7].SubItems.Add(TmpBeatmap.Audio);
            if (!File.Exists(TmpBeatmap.Audio))
            {
                detail[7].ForeColor = Color.Red;
            }
            detail[8] = new ListViewItem("背景文件名称");
            detail[8].SubItems.Add(TmpBeatmap.Background);
            if (!File.Exists(TmpBeatmap.Background))
            {
                detail[8].ForeColor = Color.Red;
            }
            detail[9] = new ListViewItem("视频文件名称");
            detail[9].SubItems.Add(TmpBeatmap.Video);
            if (!File.Exists(TmpBeatmap.Video))
            {
                detail[9].ForeColor = Color.Red;
            }
            detail[10] = new ListViewItem("OSU文件版本");
            detail[10].SubItems.Add(TmpBeatmap.FileVersion);
            detail[11] = new ListViewItem("HP");
            detail[11].SubItems.Add(TmpBeatmap.HPDrainRate.ToString());
            detail[12] = new ListViewItem("CS");
            detail[12].SubItems.Add(TmpBeatmap.CircleSize.ToString());
            detail[13] = new ListViewItem("OD");
            detail[13].SubItems.Add(TmpBeatmap.OverallDifficulty.ToString());
            detail[14] = new ListViewItem("AR");
            detail[14].SubItems.Add(TmpBeatmap.ApproachRate.ToString());
            detail[15] = new ListViewItem("md5");
            detail[15].SubItems.Add(TmpBeatmap.GetHash());
            return detail;
        }
        public static void Render()
        {
            if (MainIsVisible) { player.Render(); }
        }
        public static void Resize(Size size)
        {
            player.resize(size);
        }
        public static IEnumerable<ListViewDataItem> getscore(Font font)
        {
            var ret = new ListViewDataItem[255];
            int cur = 0;
            for (int i = 0; i < TmpSet.count; i++)
            {
                if (Core.Scores.ContainsKey(TmpSet.Diffs[i].GetHash()))
                {
                    foreach (var tmp in Core.Scores[TmpSet.Diffs[i].GetHash()])
                    {
                        ret[cur] = new ListViewDataItem
                        {
                            Image = Core.Getrank(tmp),
                            Text = String.Format(
                                "<html>Player:{0},Date:{1},Score: {2}<br>Diff:{3},Mods:{4},Mode:{5}<br>300:{6},100:{7},50:{8},Miss:{9},Maxcombo:{10}</html>",
                                tmp.Player, tmp.Time.ToString(), tmp.Score, TmpSet.Diffs[i].Version,
                                tmp.Mod, tmp.Mode.ToString(),
                                tmp.Hit300, tmp.Hit100, tmp.Hit50, tmp.Miss, tmp.MaxCombo),
                            Font = font
                        }; ;
                        cur++;
                    }
                }
            }
            return ret;
        }
        public static void AddRangeSet(IEnumerable<int> sets)
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
        public static void SetBG()
        {
            player.initBG();
        }
        #endregion
        #region 通用转换区
        public static Image Getrank(ScoreRecord S)
        {
            switch (S.Mode)
            {
                case Modes.Osu:
                    if (S.Acc == 1)
                    {
                        if (S.Mod.Contains("HD") || S.Mod.Contains("FL")) { return Resources.XH; }
                        else { return Resources.X; }
                    }
                    if ((S.Hit300) / (double)S.Totalhit > 0.9 && S.Hit50 / (double)S.Totalhit < 0.01 && S.Miss == 0)
                    {
                        if (S.Mod.Contains("HD") || S.Mod.Contains("FL")) { return Resources.SH; }
                        else { return Resources.S; }
                    }
                    if (((S.Hit300) / (double)S.Totalhit > 0.9) || ((S.Hit300) / (double)S.Totalhit > 0.8 && S.Miss == 0))
                    {
                        return Resources.A;
                    }
                    if (((S.Hit300) / (double)S.Totalhit > 0.8) || ((S.Hit300) / (double)S.Totalhit > 0.7 && S.Miss == 0))
                    {
                        return Resources.B;
                    }
                    if ((S.Hit300) / (double)S.Totalhit > 0.6)
                    {
                        return Resources.C;
                    }
                    return Resources.D;
                case Modes.Taiko:
                    if (S.Acc == 1)
                    {
                        if (S.Mod.Contains("HD") || S.Mod.Contains("FL")) { return Resources.XH; }
                        else { return Resources.X; }
                    }
                    if ((S.Hit300) / (double)S.Totalhit > 0.9 && S.Hit50 / (double)S.Totalhit < 0.01 && S.Miss == 0)
                    {
                        if (S.Mod.Contains("HD") || S.Mod.Contains("FL")) { return Resources.SH; }
                        else { return Resources.S; }
                    }
                    if (((S.Hit300) / (double)S.Totalhit > 0.9) || ((S.Hit300) / (double)S.Totalhit > 0.8 && S.Miss == 0))
                    {
                        return Resources.A;
                    }
                    if (((S.Hit300) / (double)S.Totalhit > 0.8) || ((S.Hit300) / (double)S.Totalhit > 0.7 && S.Miss == 0))
                    {
                        return Resources.B;
                    }
                    if ((S.Hit300) / (double)S.Totalhit > 0.6)
                    {
                        return Resources.C;
                    }
                    return Resources.D;
                case Modes.CTB:
                    if (S.Acc == 1)
                    {
                        if (S.Mod.Contains("HD") || S.Mod.Contains("FL")) { return Resources.XH; }
                        else { return Resources.X; }
                    }
                    if (S.Acc >= 0.9801)
                    {
                        if (S.Mod.Contains("HD") || S.Mod.Contains("FL")) { return Resources.SH; }
                        else { return Resources.S; }
                    }
                    if (S.Acc >= 0.9401)
                    {
                        return Resources.A;
                    }
                    if (S.Acc >= 0.9001)
                    {
                        return Resources.B;
                    }
                    if (S.Acc >= 0.8501)
                    {
                        return Resources.C;
                    }
                    return Resources.D;
                case Modes.Mania:
                    if (S.Acc == 1)
                    {
                        if (S.Mod.Contains("HD") || S.Mod.Contains("FL")) { return Resources.XH; }
                        else { return Resources.X; }
                    }
                    if (S.Acc > 0.95)
                    {
                        if (S.Mod.Contains("HD") || S.Mod.Contains("FL")) { return Resources.SH; }
                        else { return Resources.S; }
                    }
                    if (S.Acc > 0.90)
                    {
                        return Resources.A;
                    }
                    if (S.Acc > 0.80)
                    {
                        return Resources.B;
                    }
                    if (S.Acc > 0.70)
                    {
                        return Resources.C;
                    }
                    return Resources.D;
                default:
                    return Resources.D;
            }
        }
        #endregion
    }
}