using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using OSUplayer.Graphic;
using OSUplayer.OsuFiles;
using OSUplayer.Properties;
using OSUplayer.Uilties;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace OSUplayer
{
    public static class Core
    {
        #region 数据区

        /// <summary>
        ///     程序中的所有set
        /// </summary>
        public static List<BeatmapSet> Allsets = new List<BeatmapSet>();

        public static string CurrentListName = "Full";

        /// <summary>
        ///     本地Collection，Key是Collection名字,Value是Set的程序内部编号的List
        /// </summary>
        public static Dictionary<string, List<int>> Collections = new Dictionary<string, List<int>>();

        /// <summary>
        ///     播放列表，和显示的一一对应，int中是Set的程序内部编号
        /// </summary>
        public static List<int> PlayList
        {
            get { return Collections[CurrentListName]; }
        }


        /// <summary>
        ///     本地成绩，Key是地图MD5,Value是Score
        /// </summary>
        public static Dictionary<string, List<ScoreRecord>> Scores = new Dictionary<string, List<ScoreRecord>>();



        /// <summary>
        ///     是否已经载入过本地成绩
        /// </summary>
        public static bool Scoresearched = false;

        /// <summary>
        ///     Set有变化，需要保存
        /// </summary>
        private static bool _needsave = false;

        /// <summary>
        ///     目前正在播放的Set程序内部编号
        /// </summary>
        public static int currentset = 0;

        /// <summary>
        ///     目前正在播放的Map在Set中的编号
        /// </summary>
        public static int currentmap = 0;

        /// <summary>
        ///     目前选中的Set在播放列表中的编号
        /// </summary>
        public static int tmpset = 0;

        /// <summary>
        ///     目前选中的Map在Set中的编号
        /// </summary>
        public static int tmpmap = 0;

        /// <summary>
        ///     目前正在播放的Set
        /// </summary>
        public static BeatmapSet CurrentSet
        {
            get { return Allsets[currentset]; }
        }

        /// <summary>
        ///     目前正在播放的Map
        /// </summary>
        public static Beatmap CurrentBeatmap
        {
            get { return CurrentSet.Diffs[currentmap]; }
        }

        /// <summary>
        ///     目前选中的Set
        /// </summary>
        public static BeatmapSet TmpSet
        {
            get { return Allsets[PlayList[tmpset]]; }
        }

        /// <summary>
        ///     目前选中的Map
        /// </summary>
        public static Beatmap TmpBeatmap
        {
            get { return TmpSet.Diffs[tmpmap]; }
        }


        #endregion
        private static Thread _renderThread;
        private static Player _player;
        public static bool MainIsVisible = false;

        public static string Version
        {
            get
            {
                var asm = Assembly.GetExecutingAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return fvi.FileVersion;
            }
        }

        public static void Exit()
        {
            NotifySystem.ClearText();
            _player.Dispose();
            if (_renderThread != null) { _renderThread.Abort(); }
            if (_needsave)
            {
                DBSupporter.SaveList();
            }
        }

        #region 方法区

        #region 初始化有关

        /// <summary>
        ///     全局初始化
        /// </summary>
        /// <param name="shandle">显示区域的handle</param>
        /// <param name="ssize">显示区域的大小</param>
        public static void Init(IntPtr shandle, Size ssize)
        {
            _player = new Player(shandle, ssize);
            NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), LanguageManager.Get("Core_Init_Text"));
            Getpath();
            LoadPreference();
            new Thread(Selfupdate.check_update).Start();
            Initset();
            _renderThread = new Thread(Render);
            _renderThread.Start();
        }

        /// <summary>
        /// 获取OSU路径
        /// </summary>
        private static void Getpath()
        {
            if (Settings.Default.OSUpath != "" && File.Exists(Settings.Default.OSUpath + "\\osu!.exe")) return;
            var str = "";
            try
            {
                var rk = Registry.ClassesRoot.OpenSubKey("osu!\\shell\\open\\command");
                if (rk == null) throw new Exception("OSU not installed(?");
                str = rk.GetValue("").ToString();
                str = str.Substring(1, str.LastIndexOf(@"\", StringComparison.Ordinal));
                Settings.Default.OSUpath = str;
            }
            catch (Exception)
            {
                RadMessageBox.Show(LanguageManager.Get("Core_Error_Osupath_Text"), LanguageManager.Get("Error_Text"), MessageBoxButtons.OK, RadMessageIcon.Error);
                while (!Setpath())
                {
                }
                ;
            }
        }

        /// <summary>
        ///     设置路径
        /// </summary>
        /// <returns>是否选择正常路径</returns>
        public static bool Setpath()
        {
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                RootFolder = Environment.SpecialFolder.MyComputer,
                Description = LanguageManager.Get("Core_Osupath_Tip_Text")
            };
            try
            {
                if (DialogResult.OK == dialog.ShowDialog())
                {
                    if (File.Exists(dialog.SelectedPath + "\\osu!.exe"))
                    {
                        Settings.Default.OSUpath = dialog.SelectedPath;
                        return true;
                    }
                    RadMessageBox.Show(LanguageManager.Get("Core_Osupath_Tip_Text"), LanguageManager.Get("Error_Text"), MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
            catch (Exception)
            {
                RadMessageBox.Show(LanguageManager.Get("Core_Osupath_Tip_Text"), LanguageManager.Get("Error_Text"), MessageBoxButtons.OK, RadMessageIcon.Error);
            }
            return false;
        }

        /// <summary>
        ///     设置QQ
        /// </summary>
        /// <param name="show">是否显示设置窗口</param>
        public static void SetQQ(bool show = true)
        {
            if (show)
            {
                using (var dialog = new SetQQ())
                {
                    dialog.ShowDialog();
                }
            }
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
        ///     初始化播放列表，初始时与Set一一对应
        /// </summary>
        private static void Initplaylist()
        {
            Collections.Clear();
            var fullList = new List<int>();
            for (var i = 0; i < Allsets.Count; i++)
            {
                fullList.Add(i);
                //allsets[i].GetDetail();
            }
            Collections.Add("Full", fullList);
            var collectpath = Path.Combine(Settings.Default.OSUpath, "collection.db");
            if (File.Exists(collectpath)) { OsuDB.ReadCollect(collectpath); }
        }

        /// <summary>
        ///     载入设置
        /// </summary>
        private static void LoadPreference()
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
        ///     初始化Set的总方法，从文件读取或从osu!.db读取
        /// </summary>
        private static void Initset()
        {
            if (DBSupporter.LoadList())
            {
                Initplaylist();
            }
            else
            {
                if (File.Exists(Path.Combine(Settings.Default.OSUpath, "osu!.db")))
                {
                    OsuDB.ReadDb(Path.Combine(Settings.Default.OSUpath, "osu!.db"));
                }
                Initplaylist();
                NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), string.Format(LanguageManager.Get("Core_Init_Finish_Text"), Allsets.Count));
                _needsave = true;
            }
            currentset = 0;
            currentmap = 0;
            tmpset = 0;
            tmpmap = 0;
        }

        #endregion

        public static double Durnation
        {
            get { return _player.Durnation; }
        }

        public static double Position
        {
            get { return _player.Position; }
        }

        public static bool Isplaying
        {
            get { return _player.Isplaying; }
        }

        public static bool Willnext
        {
            get { return _player.Willnext; }
        }

        public static void Remove(int index)
        {
            // Core.allsets.RemoveAt(PlayList[index]);
            PlayList.RemoveAt(index);
            _needsave = true;
        }

        public static bool SetSet(int vaule, bool p = false)
        {
            tmpset = vaule;
            if (!TmpSet.Check())
            {
                NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), LanguageManager.Get("Core_Missing_Song_Text"));
                Remove(tmpset);
                return true;
            }
            if (!TmpSet.Detailed)
            {
                TmpSet.GetDetail();
            }
            if (p)
            {
                currentset = PlayList[tmpset];
            }
            return false;
        }

        public static bool SetMap(int vaule, bool p = false)
        {
            tmpmap = vaule;
            if (!TmpBeatmap.Detailed)
            {
                TmpBeatmap.GetDetail();
            }
            if (!File.Exists(TmpBeatmap.Audio))
            {
                NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), LanguageManager.Get("Core_Missing_Song_Text"));
                Remove(tmpset);
                return true;
            }
            if (p)
            {
                currentmap = tmpmap;
            }
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
            }
            _player.SetVolume(set, volume);
        }

        public static void RefreashSet()
        {
            File.Delete("list.db");
            Stop();
            Allsets.Clear();
            PlayList.Clear();
            Initset();
        }

        public static void Stop()
        {
            if (_player != null) _player.Stop();
        }

        public static void Play()
        {
            if (!CurrentSet.Detailed)
            {
                CurrentSet.GetDetail();
            }
            if (!CurrentBeatmap.Detailed)
            {
                CurrentBeatmap.GetDetail();
            }
            if (!File.Exists(CurrentBeatmap.Audio))
            {
                NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), LanguageManager.Get("Core_Missing_MP3_Text"));
                return;
            }
            _player.Play();
            NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), LanguageManager.Get("Core_Current_Playing_Text") + CurrentBeatmap.NameToString());
            NotifySystem.SetText(CurrentBeatmap.NameToString());
        }

        public static void PauseOrResume()
        {
            if (_player.Isplaying)
            {
                _player.Pause();
                NotifySystem.ClearText();
            }
            else
            {
                if (_player.Position == -1.0) return;
                _player.Resume();
                NotifySystem.SetText(CurrentBeatmap.NameToString());
            }
        }

        public static void Seek(double time)
        {
            if (_player.Durnation - 10 > time)
            {
                _player.Seek(time);
            }
        }

        /// <summary>
        ///     获得下一首音乐
        /// </summary>
        /// <returns>下一首Set的Playlist编号</returns>
        public static int GetNext()
        {
            int next;
            var now = PlayList.IndexOf(currentset, 0);
            if (currentset == -1)
            {
                currentset = 0;
            }
            switch (Settings.Default.NextMode)
            {
                case 1:
                    next = (now + 1) % PlayList.Count;
                    break;
                case 2:
                    next = now;
                    break;
                case 3:
                    next = new Random().Next() % PlayList.Count;
                    break;
                default:
                    next = 0;
                    break;
            }
            currentset = PlayList[next];
            currentmap = 0;
            return next;
        }

        public static void Search(string k)
        {
            var searchedMaps = new List<int>();
            var keyword = k.Trim().ToLower();
            if (keyword.Length == 0)
            {
                PlayList.Clear();
                Initplaylist();
            }
            else
            {
                //PlayList.Clear();
                for (var i = 0; i < Allsets.Count; i++)
                {
                    if (Allsets[i].tags.ToLower().Contains(keyword))
                    {
                        searchedMaps.Add(i);
                    }
                }
                if (searchedMaps.Count == 0)
                {
                    PlayList.Clear();
                }
                else
                {
                    PlayList.Clear();
                    AddRangeSet(searchedMaps);
                }
            }
        }

        public static ListViewItem[] Getdetail()
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
            detail[5].SubItems.Add(TmpBeatmap.BeatmapsetID.ToString());
            detail[6] = new ListViewItem("ID");
            detail[6].SubItems.Add(TmpBeatmap.BeatmapID.ToString());
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
            if (!String.IsNullOrEmpty(TmpBeatmap.Video) && !File.Exists(TmpBeatmap.Video))
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

        private static void Render(object sender)
        {
            while (true)
            {
                if (MainIsVisible)
                {
                    _player.Render();
                }
            }
        }

        public static void Resize(Size size)
        {
            MainIsVisible = false;
            Thread.Sleep(100);
            _player.Resize(size);
            MainIsVisible = true;
        }

        public static IEnumerable<ListViewDataItem> Getscore(Font font)
        {
            var ret = new ListViewDataItem[255];
            int cur = 0;
            for (int i = 0; i < TmpSet.count; i++)
            {
                if (Scores.ContainsKey(TmpSet.Diffs[i].GetHash()))
                {
                    foreach (var tmp in Scores[TmpSet.Diffs[i].GetHash()])
                    {
                        ret[cur] = new ListViewDataItem
                        {
                            Image = Getrank(tmp),
                            Text = String.Format(
                                "<html>Player:{0},Date:{1},Score: {2}<br>Diff:{3},Mods:{4},Mode:{5}<br>300:{6},100:{7},50:{8},Miss:{9},Maxcombo:{10}</html>",
                                tmp.Player, tmp.Time, tmp.Score, TmpSet.Diffs[i].Version,
                                tmp.Mod, tmp.Mode,
                                tmp.Hit300, tmp.Hit100, tmp.Hit50, tmp.Miss, tmp.MaxCombo),
                            Font = font
                        };
                        ;
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
            if (!PlayList.Contains(setno))
            {
                PlayList.Add(setno);
            }
        }

        public static void SetBG()
        {
            _player.InitBG();
        }

        #endregion

        #region 通用转换区

        private static Image Getrank(ScoreRecord s)
        {
            switch (s.Mode)
            {
                case Modes.Osu:
                    if (s.Acc == 1)
                    {
                        if (s.Mod.Contains("HD") || s.Mod.Contains("FL"))
                        {
                            return Resources.XH;
                        }
                        return Resources.X;
                    }
                    if ((s.Hit300) / (double)s.Totalhit > 0.9 && s.Hit50 / (double)s.Totalhit < 0.01 && s.Miss == 0)
                    {
                        if (s.Mod.Contains("HD") || s.Mod.Contains("FL"))
                        {
                            return Resources.SH;
                        }
                        return Resources.S;
                    }
                    if (((s.Hit300) / (double)s.Totalhit > 0.9) || ((s.Hit300) / (double)s.Totalhit > 0.8 && s.Miss == 0))
                    {
                        return Resources.A;
                    }
                    if (((s.Hit300) / (double)s.Totalhit > 0.8) || ((s.Hit300) / (double)s.Totalhit > 0.7 && s.Miss == 0))
                    {
                        return Resources.B;
                    }
                    if ((s.Hit300) / (double)s.Totalhit > 0.6)
                    {
                        return Resources.C;
                    }
                    return Resources.D;
                case Modes.Taiko:
                    if (s.Acc == 1)
                    {
                        if (s.Mod.Contains("HD") || s.Mod.Contains("FL"))
                        {
                            return Resources.XH;
                        }
                        return Resources.X;
                    }
                    if ((s.Hit300) / (double)s.Totalhit > 0.9 && s.Hit50 / (double)s.Totalhit < 0.01 && s.Miss == 0)
                    {
                        if (s.Mod.Contains("HD") || s.Mod.Contains("FL"))
                        {
                            return Resources.SH;
                        }
                        return Resources.S;
                    }
                    if (((s.Hit300) / (double)s.Totalhit > 0.9) || ((s.Hit300) / (double)s.Totalhit > 0.8 && s.Miss == 0))
                    {
                        return Resources.A;
                    }
                    if (((s.Hit300) / (double)s.Totalhit > 0.8) || ((s.Hit300) / (double)s.Totalhit > 0.7 && s.Miss == 0))
                    {
                        return Resources.B;
                    }
                    if ((s.Hit300) / (double)s.Totalhit > 0.6)
                    {
                        return Resources.C;
                    }
                    return Resources.D;
                case Modes.CTB:
                    if (s.Acc == 1)
                    {
                        if (s.Mod.Contains("HD") || s.Mod.Contains("FL"))
                        {
                            return Resources.XH;
                        }
                        return Resources.X;
                    }
                    if (s.Acc >= 0.9801)
                    {
                        if (s.Mod.Contains("HD") || s.Mod.Contains("FL"))
                        {
                            return Resources.SH;
                        }
                        return Resources.S;
                    }
                    if (s.Acc >= 0.9401)
                    {
                        return Resources.A;
                    }
                    if (s.Acc >= 0.9001)
                    {
                        return Resources.B;
                    }
                    if (s.Acc >= 0.8501)
                    {
                        return Resources.C;
                    }
                    return Resources.D;
                case Modes.Mania:
                    if (s.Acc == 1)
                    {
                        if (s.Mod.Contains("HD") || s.Mod.Contains("FL"))
                        {
                            return Resources.XH;
                        }
                        return Resources.X;
                    }
                    if (s.Acc > 0.95)
                    {
                        if (s.Mod.Contains("HD") || s.Mod.Contains("FL"))
                        {
                            return Resources.SH;
                        }
                        return Resources.S;
                    }
                    if (s.Acc > 0.90)
                    {
                        return Resources.A;
                    }
                    if (s.Acc > 0.80)
                    {
                        return Resources.B;
                    }
                    if (s.Acc > 0.70)
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