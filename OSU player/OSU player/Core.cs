using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
using System.Linq;

namespace OSUplayer
{
    public static class Core
    {
        #region 数据区

        /// <summary>
        ///     程序中的所有set
        /// </summary>
        public static Dictionary<string, BeatmapSet> Allsets = new Dictionary<string, BeatmapSet>();

        public static string CurrentListName = "Full";

        /// <summary>
        ///     本地Collection，Key是Collection名字,Value是Set的程序内部编号的List
        /// </summary>
        public static Dictionary<string, List<string>> Collections = new Dictionary<string, List<string>>();

        /// <summary>
        ///     播放列表，和显示的一一对应，int中是Set的程序内部编号
        /// </summary>
        public static List<string> PlayList
        {
            get
            {
                return Collections.ContainsKey(CurrentListName) ? Collections[CurrentListName] : new List<string>();
            }
        }


        /// <summary>
        ///     本地成绩，Key是地图MD5,Value是Score
        /// </summary>
        public static Dictionary<string, List<ScoreRecord>> Scores = new Dictionary<string, List<ScoreRecord>>();



        /// <summary>
        ///     是否已经载入过本地成绩
        /// </summary>
        public static bool Scoresearched;

        /// <summary>
        ///     Set有变化，需要保存
        /// </summary>
        private static bool _needsave;

        /// <summary>
        ///     目前正在播放的Set播放列表编号(或者是-1)
        /// </summary>
        public static int CurrentSetIndex
        {
            get
            {
                return CurrentSet == null ? -1 : PlayList.IndexOf(CurrentSet.GetHash());
            }
        }

        /// <summary>
        ///     目前正在播放的Set
        /// </summary>
        public static BeatmapSet CurrentSet { get; private set; }

        /// <summary>
        ///     目前正在播放的Map
        /// </summary>
        public static Beatmap CurrentBeatmap { get; private set; }

        /// <summary>
        ///     目前选中的Set
        /// </summary>
        public static BeatmapSet TmpSet { get; private set; }

        /// <summary>
        ///     目前选中的Map
        /// </summary>
        public static Beatmap TmpBeatmap { get; private set; }


        #endregion
        private static Thread _renderThread;
        private static Player _player;
        public static bool MainIsVisible;

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
            if (Settings.Default.OSUpath == "" || !File.Exists(Settings.Default.OSUpath + "\\osu!.exe"))
            {
                try
                {
                    var rk = Registry.ClassesRoot.OpenSubKey("osu!\\shell\\open\\command");
                    if (rk == null) throw new Exception("OSU not installed(?");
                    var str = rk.GetValue("").ToString();
                    str = str.Substring(1, str.LastIndexOf(@"\", StringComparison.Ordinal));
                    Settings.Default.OSUpath = str;
                }
                catch (Exception)
                {
                    MessageBox.Show(LanguageManager.Get("Core_Error_Osupath_Text"), LanguageManager.Get("Error_Text"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    while (!Setpath())
                    {
                    }
                }
            }
            if (!File.Exists("bass_fx.dll"))
            {
                File.Copy(Settings.Default.OSUpath + "\\bass_fx.dll", "bass_fx.dll");
                Un4seen.Bass.AddOn.Fx.BassFx.LoadMe();
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
                    MessageBox.Show(LanguageManager.Get("Core_Osupath_Tip_Text"), LanguageManager.Get("Error_Text"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(LanguageManager.Get("Core_Osupath_Tip_Text"), LanguageManager.Get("Error_Text"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            }
            else
            {
                Settings.Default.QQuin = Settings.Default.QQuin;
                Settings.Default.SyncQQ = true;
            }
        }

        /// <summary>
        ///     初始化播放列表，初始时与Set一一对应
        /// </summary>
        private static void Initplaylist()
        {
            Collections.Clear();
            Collections.Add("Full", Allsets.Select(d => d.Value.GetHash()).ToList());
            PlayList.Sort(((a, b) => (Allsets[a].ArtistCompare(Allsets[b]))));
            var collectpath = Path.Combine(Settings.Default.OSUpath, "collection.db");
            if (File.Exists(collectpath)) { OsuDB.ReadCollect(collectpath); }
        }

        /// <summary>
        ///     载入设置
        /// </summary>
        public static void LoadPreference()
        {
            if (!Settings.Default.Upgraded)
            {
                Settings.Default.Upgrade();
                Settings.Default.Save();
                //if (File.Exists("list.db")) File.Delete("list.db");
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
            if (DBSupporter.LoadList() && (Allsets != null))
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
            CurrentSet = Allsets[PlayList[0]];
            CurrentBeatmap = CurrentSet.GetBeatmaps()[0];
            TmpSet = CurrentSet;
            TmpBeatmap = CurrentBeatmap;
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

        public static void Remove(string key)
        {
            // Core.allsets.RemoveAt(PlayList[index]);
            PlayList.Remove(key);
            _needsave = true;
        }

        public static void Remove(int index)
        {
            PlayList.RemoveAt(index);
            _needsave = true;
        }

        public static void Tmp2Current(bool set)
        {
            if (set) CurrentSet = TmpSet;
            else CurrentBeatmap = TmpBeatmap;
        }
        public static bool SetSet(int value, bool p = false)
        {
            TmpSet = Allsets[PlayList[value]];
            if (!TmpSet.Check())
            {
                NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), LanguageManager.Get("Core_Missing_Song_Text"));
                Remove(TmpSet.GetHash());
#if THROW
                throw new Exception("Set net found, text is " + TmpSet.location);
#endif
                return true;
            }
            if (!TmpSet.Detailed)
            {
                TmpSet.GetDetail();
            }
            if (p)
            {
                Tmp2Current(true);
            }
            return false;
        }

        public static bool SetMap(int value, bool p = false)
        {
            TmpBeatmap = TmpSet.GetBeatmaps()[value];
            if (!TmpBeatmap.Detailed)
            {
                TmpBeatmap.GetDetail();
            }
            if (!File.Exists(TmpBeatmap.Audio))
            {
                NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), LanguageManager.Get("Core_Missing_Song_Text"));
                Remove(TmpSet.GetHash());
#if THROW
                throw new Exception("Song net found, text is " + TmpBeatmap.Audio);
#endif
                return true;
            }
            if (p)
            {
                Tmp2Current(false);
            }
            return false;
        }

        public static void SetVolume(int set, int volume)
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
            _player.UpdateVolume();
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
            if (_historyPointer != PlayHistory.Count - 1)
            {
                _historyPointer++;
                var nextsong = PlayList.IndexOf(PlayHistory[_historyPointer]);
                if (nextsong != -1) return nextsong;
                PlayHistory.RemoveRange(_historyPointer, PlayHistory.Count - _historyPointer);
                _historyPointer--;
            }
            int next;
            var now = CurrentSetIndex;
            if (CurrentSetIndex == -1)
            {
                now = 0;
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
            CurrentSet = Allsets[PlayList[next]];
            CurrentBeatmap = CurrentSet.GetBeatmaps()[0];
            SetHistory();
            return next;
        }

        public static void SetHistory(bool front = false, bool back = false, bool init = false)
        {
            if (init)
            {
                if (PlayHistory.Count == 0)
                {
                    PlayHistory.Add(CurrentSet.GetHash());
                    _historyPointer = 0;
                }

            }
            else if (front)
            {
                if (_historyPointer == -1) _historyPointer = 0;
                PlayHistory.Insert(0, CurrentSet.GetHash());
            }
            else
            {
                _historyPointer++;
                if (back)
                {
                    PlayHistory.RemoveRange(_historyPointer, PlayHistory.Count - _historyPointer);
                }
                PlayHistory.Add(CurrentSet.GetHash());
            }
        }
        private static readonly List<string> PlayHistory = new List<string>();
        private static int _historyPointer = -1;
        public static int GetPrev()
        {
            if (_historyPointer > 0)
            {
                _historyPointer--;
                var prevsong = PlayList.IndexOf(PlayHistory[_historyPointer]);
                if (prevsong != -1) return prevsong;
                PlayHistory.RemoveRange(0, _historyPointer + 1);
                _historyPointer = 0;
            }
            int prev;
            var now = CurrentSetIndex;
            if (CurrentSetIndex == -1)
            {
                now = 0;
            }
            switch (Settings.Default.NextMode)
            {
                case 1:
                    prev = (now - 1) % PlayList.Count;
                    break;
                case 2:
                    prev = now;
                    break;
                case 3:
                    prev = new Random().Next() % PlayList.Count;
                    break;
                default:
                    prev = 0;
                    break;
            }
            CurrentSet = Allsets[PlayList[prev]];
            CurrentBeatmap = CurrentSet.GetBeatmaps()[0];
            SetHistory(true);
            return prev;
        }

        public static void Search(string k)
        {
            var searchedMaps = new List<string>();
            var keyword = k.Trim().ToLower();
            if (keyword.Length == 0)
            {
                PlayList.Clear();
                Initplaylist();
            }
            else
            {
                searchedMaps.AddRange(from beatmapSet in Allsets where beatmapSet.Value.tags.ToLower().Contains(keyword) select beatmapSet.Key);
                if (searchedMaps.Count == 0)
                {
                    PlayList.Clear();
                }
                else
                {
                    PlayList.Clear();
                    PlayList.AddRange(searchedMaps.Distinct());
                }
            }
        }

        public static ListViewItem Getdetail(int index)
        {
            ListViewItem ret;
            switch (index)
            {
                case 0:
                    ret = new ListViewItem(LanguageManager.Get("Main_ListDetail_Title"));
                    ret.SubItems.Add(TmpBeatmap.Title);
                    break;
                case 1:
                    ret = new ListViewItem(LanguageManager.Get("Main_ListDetail_Artist"));
                    ret.SubItems.Add(TmpBeatmap.Artist);
                    break;
                case 2:
                    ret = new ListViewItem(LanguageManager.Get("Main_ListDetail_Mapper"));
                    ret.SubItems.Add(TmpBeatmap.Creator);
                    break;
                case 3:
                    ret = new ListViewItem(LanguageManager.Get("Main_ListDetail_Source"));
                    ret.SubItems.Add(TmpBeatmap.Source);
                    break;
                case 4:
                    ret = new ListViewItem(LanguageManager.Get("Main_ListDetail_Mode"));
                    ret.SubItems.Add(Enum.GetName(typeof(Modes), TmpBeatmap.Mode));
                    break;
                case 5:
                    ret = new ListViewItem("SetID");
                    ret.SubItems.Add(TmpBeatmap.BeatmapsetID.ToString());
                    break;
                case 6:
                    ret = new ListViewItem("ID");
                    ret.SubItems.Add(TmpBeatmap.BeatmapID.ToString());
                    break;
                case 7:
                    ret = new ListViewItem(LanguageManager.Get("Main_ListDetail_WAVPath"));
                    ret.SubItems.Add(TmpBeatmap.Audio);
                    if (!File.Exists(TmpBeatmap.Audio))
                    {
                        ret.ForeColor = Color.Red;
                    }
                    break;
                case 8:
                    ret = new ListViewItem(LanguageManager.Get("Main_ListDetail_BGPath"));
                    ret.SubItems.Add(TmpBeatmap.Background);
                    if (!File.Exists(TmpBeatmap.Background))
                    {
                        ret.ForeColor = Color.Red;
                    }
                    break;
                case 9:
                    ret = new ListViewItem(LanguageManager.Get("Main_ListDetail_VideoPath"));
                    ret.SubItems.Add(TmpBeatmap.Video);
                    if (!String.IsNullOrEmpty(TmpBeatmap.Video) && !File.Exists(TmpBeatmap.Video))
                    {
                        ret.ForeColor = Color.Red;
                    }
                    break;
                case 10:
                    ret = new ListViewItem(LanguageManager.Get("Main_ListDetail_FileVersion"));
                    ret.SubItems.Add(TmpBeatmap.FileVersion);
                    break;
                case 11:
                    ret = new ListViewItem("HP");
                    ret.SubItems.Add(TmpBeatmap.HPDrainRate.ToString());
                    break;
                case 12:
                    ret = new ListViewItem("CS");
                    ret.SubItems.Add(TmpBeatmap.CircleSize.ToString());
                    break;
                case 13:
                    ret = new ListViewItem("OD");
                    ret.SubItems.Add(TmpBeatmap.OverallDifficulty.ToString());
                    break;
                case 14:
                    ret = new ListViewItem("AR");
                    ret.SubItems.Add(TmpBeatmap.ApproachRate.ToString());
                    break;
                case 15:
                    ret = new ListViewItem("MD5");
                    ret.SubItems.Add(TmpBeatmap.GetHash());
                    break;
                default:
                    ret = new ListViewItem();
                    break;
            }
            return ret;
        }

        private static void Render(object sender)
        {
            while (true)
            {
                if (MainIsVisible)
                {
                    _player.Render();
                }
                else Thread.Sleep(100);
            }
        }

        public static void Resize(Size size)
        {
            MainIsVisible = false;
            Thread.Sleep(100);
            _player.Resize(size);
            MainIsVisible = true;
        }

        public static IEnumerable<ScoreRecord> Getscore()
        {
            var ret = new List<ScoreRecord>();
            foreach (var beatmap in TmpSet.Diffs.Where(beatmap => Scores.ContainsKey(beatmap.Key)))
            {
                foreach (var item in Scores[beatmap.Key])
                {
                    item.DiffName = beatmap.Value.Version;
                }
                ret.AddRange(Scores[beatmap.Key]);
            }
            return ret;
        }
        public static void SetBG()
        {
            _player.InitBG();
        }

        public static void SetPlayerSpeed(bool nc, int speed)
        {
            if (_player != null) _player.SetSpeed(nc, speed);
        }
        #endregion
    }
}