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
using System.Reflection;
using System.Diagnostics;
using OSU_player.OSUFiles;
using OSU_player.Graphic;
namespace OSU_player
{
    public class Core
    {
        #region 数据区
        /// <summary>
        /// OSU的路径
        /// </summary>
        public static string osupath;
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
        public static Dictionary<string, List<Score>> Scores = new Dictionary<string, List<Score>>();
        /// <summary>
        /// 本地Collection，Key是Collection名字,Value是Set的程序内部编号的List
        /// </summary>
        public static Dictionary<string, List<int>> Collections = new Dictionary<string, List<int>>();
        /// <summary>
        /// 是否已经载入过本地成绩
        /// </summary>
        public static bool scoresearched = false;
        public static Image defaultBG = Properties.Resources.defaultBG;
        public static string defaultAudio = Application.StartupPath + "\\Default\\" + "blank.wav";
        public static float Allvolume = 1.0f;
        public static float Musicvolume = 0.8f;
        public static float Fxvolume = 0.6f;
        public static int Nextmode = 3;
        /// <summary>
        /// 选定的QQ号
        /// </summary>
        public static string uin;
        /// <summary>
        /// 是否同步QQ
        /// </summary>
        public static bool syncQQ = true;
        public static bool playvideo = true;
        /// <summary>
        /// 是否播放音效
        /// </summary>
        public static bool playfx = true;
        public static bool playsb = true;
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
        public static Uilties.QQ uni_QQ = new Uilties.QQ();
        private static Player player;
        public static NotifyIcon notifyIcon1 = new NotifyIcon();
        public static string Version
        {
            get
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return fvi.FileVersion;
            }
        }
        public static bool MainIsVisible = true;
        public static void exit()
        {
            uni_QQ.Send2QQ(uin, "");
            player.Dispose();
            notifyIcon1.Dispose();
            if (needsave) { SaveList(); }
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
            notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            notifyIcon1.BalloonTipTitle = "OSUplayer";
            notifyIcon1.BalloonTipText = "正在初始化...";
            notifyIcon1.Icon = ((System.Drawing.Icon)(Resources.icon));
            notifyIcon1.Text = "OSUplayer";
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(1000);
            Getpath();
            LoadPreference();
            new Thread(new ThreadStart(Uilties.Selfupdate.check_update)).Start();
            initset();
        }
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
        /// 从文件读取Set(allset)
        /// </summary>
        /// <returns>是否正常读取</returns>
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
        /// 保存全局Set(allset)到文件
        /// </summary>
        public static void SaveList()
        {
            using (FileStream fs = new FileStream("list.db", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, allsets);
            }
        }
        /// <summary>
        /// 设置QQ
        /// </summary>
        /// <param name="show">是否显示设置窗口</param>
        public static void SetQQ(bool show = true)
        {
            if (show) { using (Form2 dialog = new Form2()) { dialog.ShowDialog(); } }
            if (uin == "0")
            {
                Properties.Settings.Default.QQuin = "0";
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
            if (!Properties.Settings.Default.Upgraded)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.Save();
            }
            uin = Properties.Settings.Default.QQuin;
            syncQQ = Properties.Settings.Default.SyncQQ;
            if (uin == "0")
            {
                /* if (
                     RadMessageBox.Show("木有设置过QQ号，需要现在设置么？", "提示", MessageBoxButtons.YesNo)
                     == DialogResult.Yes) { SetQQ(); }
                 else { SetQQ(false); }*/
                SetQQ();
            }
            Allvolume = Properties.Settings.Default.Allvolume;
            Fxvolume = Properties.Settings.Default.Fxvolume;
            Musicvolume = Properties.Settings.Default.Musicvolume;
            playfx = Properties.Settings.Default.PlayFx;
            playsb = false;
            //playsb = Properties.Settings.Default.PlaySB;
            playvideo = Properties.Settings.Default.PlayVideo;
            Nextmode = Properties.Settings.Default.NextMode;
            Properties.Settings.Default.Upgraded = true;
        }
        /// <summary>
        /// 初始化Set的总方法，从文件读取或从osu!.db读取
        /// </summary>
        public static void initset()
        {
            if (File.Exists("list.db") && LoadList())
            {
                initplaylist();
            }
            else
            {
                if (File.Exists(Path.Combine(Core.osupath, "osu!.db")))
                {
                    OsuDB.ReadDb(Path.Combine(Core.osupath, "osu!.db"));
                }
                initplaylist();
                notifyIcon1.ShowBalloonTip(1000, "OSUplayer", string.Format("初始化完毕，发现曲目{0}个", allsets.Count), ToolTipIcon.Info);
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
                Core.notifyIcon1.ShowBalloonTip(1000, "OSUplayer", "没事删什么曲子啊><", System.Windows.Forms.ToolTipIcon.Info);
                remove(tmpset);
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
            if (!File.Exists(TmpBeatmap.Path))
            {
                Core.notifyIcon1.ShowBalloonTip(1000, "OSUplayer", "没事删什么曲子啊><", System.Windows.Forms.ToolTipIcon.Info);
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
            if (!File.Exists(CurrentBeatmap.Audio))
            {
                Core.notifyIcon1.ShowBalloonTip(1000, "OSUplayer", "音频文件你都删！><", System.Windows.Forms.ToolTipIcon.Info);
                return;
            }
            player.Play();
            Core.notifyIcon1.ShowBalloonTip(1000, "OSUplayer", "正在播放\n" + CurrentBeatmap.NameToString(), System.Windows.Forms.ToolTipIcon.Info);
            uni_QQ.Send2QQ(uin, CurrentBeatmap.NameToString());
        }
        public static void Pause()
        {
            player.Pause();
            uni_QQ.Send2QQ(uin, "");
        }
        public static void Resume()
        {
            player.Resume();
            uni_QQ.Send2QQ(uin, CurrentBeatmap.NameToString());
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
        public static void Render()
        {
            if (MainIsVisible) { player.Render(); }
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
        public static void setBG()
        {
            player.initBG();
        }
        #endregion
        #region 通用转换区
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
        #endregion
    }
}