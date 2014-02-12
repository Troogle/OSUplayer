using System;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using OSUplayer.Properties;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.ComponentModel;
using System.Threading;
using OSUplayer.OsuFiles;
using OSUplayer.Uilties;

namespace OSUplayer
{
    public partial class Main : RadForm
    {
        public Main()
        {
            InitializeComponent();
            Initlang();
            NotifySystem.RegisterClick(TaskbarIconClickHandler);
        }

        #region 各种方法
        private void Initlang()
        {

            foreach (var lang in LanguageManager.LanguageList)
            {
                var item = new ToolStripMenuItem(lang);
                item.Click += delegate
                {
                    LanguageManager.Current = item.Text;
                    UpdateFormLanguage();
                };
                //Menubar_Language.DropDownItems.Add(item);
            }
        }
        private void UpdateFormLanguage()
        {

        }

        private void TaskbarIconClickHandler(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Visible = false;
            }
            else
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void AskForExit(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Core.Pause();
            if (RadMessageBox.Show("确认退出？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Core.MainIsVisible = false;
                Core.exit();
                hotkeyHelper.UnregisterHotkeys();
                this.Dispose();
            }
            else
            {
                e.Cancel = true;
                Core.Resume();
            }
        }
        private void SetDetail()
        {
            Main_ListDetail.Items.Clear();
            Main_ListDetail.Items.AddRange(Core.getdetail());
            Core.SetBG();
        }
        private void Stop()
        {
            UpdateTimer.Enabled = false;
            Core.Stop();
            Main_Time_Trackbar.Enabled = false;
            Main_Time_Trackbar.Value = 0;
            Main_Stop.Enabled = false;
            Main_Play.Text = "播放";
        }
        private void Play()
        {
            if (Core.PlayList.Count != 0)
            {
                UpdateTimer.Enabled = true;
                Core.Play();
                Main_Time_Trackbar.Enabled = true;
                Main_Play.Text = "暂停";
                Main_Stop.Enabled = true;
                Main_Time_Trackbar.Maximum = (int)Core.Durnation * 1000;
            }
        }
        private void Pause()
        {
            UpdateTimer.Enabled = false;
            Core.Pause();
            Main_Play.Text = "播放";
        }
        private void Resume()
        {
            Core.Resume();
            UpdateTimer.Enabled = true;
            Main_Play.Text = "暂停";
        }
        private void PlayNext(bool play = true)
        {
            if (Core.PlayList.Count == 0) return;
            int nextSongId = Core.GetNext();
            if (Main_PlayList.SelectedItems.Count != 0)
            {
                Main_PlayList.SelectedItems[0].Selected = false;
            }
            Main_PlayList.Items[nextSongId].Selected = true;
            Main_PlayList.EnsureVisible(nextSongId);
            Main_PlayList.Focus();
            SetDetail();
            if (play) { Play(); }
        }
        private void Setscore()
        {
            Main_ScoreBox.Items.Clear();
            foreach (var item in Core.getscore(Main_ScoreBox.Font))
            {
                if (item != null)
                {
                    Main_ScoreBox.Items.Add(item);
                }
            }
        }
        #endregion
        private void SetForm()
        {
            Main_QQ_Hint_Label.Text = "当前同步QQ：" + Settings.Default.QQuin;
            Main_Option_Sync_QQ.IsChecked = Settings.Default.SyncQQ;
            Main_Volume_TrackBar.Value = 100 - (int)(Settings.Default.Allvolume * Main_Volume_TrackBar.Maximum);
            Main_Volume_Fx_TrackBar.Value = (int)(Settings.Default.Fxvolume * Main_Volume_Fx_TrackBar.Maximum);
            Main_Volume_Music_TrackBar.Value = (int)(Settings.Default.Musicvolume * Main_Volume_Music_TrackBar.Maximum);
            Main_Option_Play_Fx.IsChecked = Settings.Default.PlayFx;
            Main_Option_Play_SB.IsChecked = Settings.Default.PlaySB;
            Main_Option_Play_Video.IsChecked = Settings.Default.PlayVideo;
            radMenuComboItem1.ComboBoxElement.SelectedIndex = Settings.Default.NextMode - 1;
        }
        private void RefreshList(int select = 0)
        {
            Main_PlayList.Items.Clear();
            Main_DiffList.Items.Clear();
            //PlayList.Enabled = false;
            //DiffList.Enabled = false;
            new Thread(delegate()
            {
                foreach (int t in Core.PlayList)
                {
                    Main_PlayList.Invoke((MethodInvoker)(() =>
                        Main_PlayList.Items.Add(Core.allsets[t].ToString())));
                }
                if (Main_PlayList.Items.Count != 0)
                {
                    Main_PlayList.Invoke((MethodInvoker)(() => Main_PlayList.Items[@select].Selected = true));
                }
                //PlayList.Enabled = true;
                //DiffList.Enabled = true;
            }).Start();
        }
        void Main_VisibleChanged(object sender, EventArgs e)
        {
            Core.MainIsVisible = this.Visible;
        }

        #region 菜单栏
        #region 文件
        private void 运行OSU_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Settings.Default.OSUpath, "osu!.exe"));
        }
        private void 手动指定OSU目录_Click(object sender, EventArgs e)
        {
            if (Core.Setpath())
            {
                Main_PlayList.Items.Clear();
                Main_DiffList.Items.Clear();
                Core.RefreashSet();
                RefreshList();
            }
        }
        private void 重新导入osu_Click(object sender, EventArgs e)
        {
            Main_PlayList.Items.Clear();
            Main_DiffList.Items.Clear();
            Core.RefreashSet();
            RefreshList();
        }
        private void 重新导入scores_Click(object sender, EventArgs e)
        {
            Core.Scores.Clear();
            string scorepath = Path.Combine(Settings.Default.OSUpath, "scores.db");
            if (File.Exists(scorepath)) { OsuDB.ReadScore(scorepath); Core.Scoresearched = true; Main_File_Import_Scores.Text = "重新导入scores.db"; }
        }
        private void 打开曲目文件夹_Click(object sender, EventArgs e)
        {
            Process.Start(Core.TmpSet.location);
        }
        private void 打开铺面文件_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", Core.TmpBeatmap.Path);
        }
        private void 打开SB文件_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", Core.TmpSet.OsbPath);
        }
        private void 导出BG_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Core.CurrentBeatmap.Background))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.CheckFileExists = false;
                dialog.CreatePrompt = false;
                dialog.AddExtension = true;
                dialog.OverwritePrompt = true;
                dialog.FileName = new FileInfo(Core.CurrentBeatmap.Background).Name;
                dialog.DefaultExt = new FileInfo(Core.CurrentBeatmap.Background).Extension;
                dialog.Filter = "All files (*.*)|*.*";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (DialogResult.OK == dialog.ShowDialog())
                {
                    File.Copy(Core.CurrentBeatmap.Background, dialog.FileName, true);
                }
            }
        }
        private void 导出音频文件_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.CheckFileExists = false;
            dialog.CreatePrompt = false;
            dialog.AddExtension = true;
            dialog.OverwritePrompt = true;
            dialog.FileName = new FileInfo(Core.CurrentBeatmap.Audio).Name;
            dialog.DefaultExt = new FileInfo(Core.CurrentBeatmap.Audio).Extension;
            dialog.Filter = "All files (*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (DialogResult.OK == dialog.ShowDialog())
            {
                File.Copy(Core.CurrentBeatmap.Audio, dialog.FileName, true);
            }
        }

        private void 退出_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #region 工具
        private void 重复歌曲扫描_Click(object sender, EventArgs e)
        {
            using (DelDulp dialog = new DelDulp())
            {
                dialog.ShowDialog();
            }
        }
        #endregion
        #region 选项
        private void 音效_Click(object sender, EventArgs e)
        {
            Settings.Default.PlayFx = Main_Option_Play_Fx.IsChecked;
        }
        private void 视频开关_Click(object sender, EventArgs e)
        {
            Settings.Default.PlayVideo = Main_Option_Play_Video.IsChecked;
        }
        private void radMenuComboItem1_ComboBoxElement_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            Settings.Default.NextMode = radMenuComboItem1.ComboBoxElement.SelectedIndex + 1;
        }
        private void QQ状态同步_Click(object sender, EventArgs e)
        {
            if (Settings.Default.SyncQQ && Settings.Default.QQuin != "0") { QQ.Send2QQ(Settings.Default.QQuin, ""); }
            Settings.Default.SyncQQ = Main_Option_Sync_QQ.IsChecked;
        }
        private void SB开关_Click(object sender, EventArgs e)
        {
            Settings.Default.PlaySB = Main_Option_Play_SB.IsChecked;
        }
        private void Main_Option_Show_Popup_Click(object sender, EventArgs e)
        {
            Settings.Default.ShowPopup = Main_Option_Show_Popup.IsChecked;
        }
        #endregion
        private void 关于_Click(object sender, EventArgs e)
        {
            using (About dialog = new About())
            {
                dialog.ShowDialog();
            }
        }
        #endregion
        #region 第一排
        private void Button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            using (var dialog = new ChooseColl())
            {
                dialog.ShowDialog();
            }
            this.Visible = true;
            RefreshList();
        }
        private void TrackFx_Scroll(object sender, ScrollEventArgs e)
        {
            Core.SetVolume(3, (float)Main_Volume_Fx_TrackBar.Value / (float)Main_Volume_Fx_TrackBar.Maximum);
        }
        private void TrackMusic_Scroll(object sender, ScrollEventArgs e)
        {
            Core.SetVolume(2, (float)Main_Volume_Music_TrackBar.Value / (float)Main_Volume_Music_TrackBar.Maximum);
        }
        private void TrackVolume_Scroll(object sender, ScrollEventArgs e)
        {
            Core.SetVolume(1, 1.0f - (float)Main_Volume_TrackBar.Value / (float)Main_Volume_TrackBar.Maximum);
        }
        #endregion
        #region 第二排
        private void PlayList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Main_PlayList.SelectedItems.Count == 0) { return; }
            Main_DiffList.Items.Clear();
            if (Core.SetSet(Main_PlayList.SelectedIndices[0])) { RefreshList(); PlayNext(false); }
            else
            {
                foreach (var s in Core.TmpSet.Diffs)
                {
                    Main_DiffList.Items.Add(s.Version);
                }
                Main_File_Open_SBFile.Enabled = File.Exists(Core.TmpSet.OsbPath);
                Main_DiffList.SelectedIndex = 0;
            }
        }
        private void DiffList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Main_PlayList.SelectedIndices.Count == 0) { return; }
            if (Core.SetMap(Main_DiffList.SelectedIndex)) { RefreshList(); PlayNext(false); }
            else
            {
                if (Core.Scoresearched) { Setscore(); }
                if (!Main_Stop.Enabled)
                {
                    Core.currentset = Core.PlayList[Core.tmpset];
                    Core.currentmap = Core.tmpmap;
                    SetDetail();
                }
                else if (Core.Isplaying)
                {
                    Main_ListDetail.Items.Clear();
                    Main_ListDetail.Items.AddRange(Core.getdetail());
                }
                else
                {
                    Stop();
                    SetDetail();
                }
            }
        }
        private void PlayList_DoubleClick(object sender, EventArgs e)
        {
            if (Core.SetSet(Main_PlayList.SelectedIndices[0], true)) { RefreshList(); PlayNext(); }
            else
            {
                Core.SetMap(0, true);
                Stop();
                SetDetail();
                Play();
            }
        }
        private void DiffList_DoubleClick(object sender, EventArgs e)
        {
            if (Core.SetMap(Main_DiffList.SelectedIndex, true)) { RefreshList(); PlayNext(); }
            else
            {
                Stop();
                SetDetail();
                Play();
            }
        }
        private void StopButton_Click(object sender, EventArgs e)
        {
            Stop();
        }
        private void NextButton_Click(object sender, EventArgs e)
        {
            NextTimer.Enabled = false;
            NextTimer.Enabled = true;
        }
        private void TrackSeek_Scroll(object sender, ScrollEventArgs e)
        {
            Core.seek((double)Main_Time_Trackbar.Value / 1000);
        }

        void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                Main_Search_Box.Text = "";
                Core.search(Main_Search_Box.Text);
                RefreshList();
            }
            if (e.KeyChar == (char)13)
            {
                Core.search(Main_Search_Box.Text);
                RefreshList();
            }
            else
            {
                SearchTimer.Enabled = false;
                SearchTimer.Enabled = true;
            }
        }
        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (!Main_Stop.Enabled)
            {
                Play();
            }
            else
            {
                if (Main_Play.Text == "播放")
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
        #endregion
        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.UpdateTimer.Enabled = false;
            hotkeyHelper.UnregisterHotkeys();
            NotifySystem.RegisterClick(null);
            using (var dialog = new Mini())
            {
                dialog.ShowDialog();
            }
            NotifySystem.RegisterClick(TaskbarIconClickHandler);
            playKey = hotkeyHelper.RegisterHotkey(Keys.F5, KeyModifiers.Alt);
            nextKey = hotkeyHelper.RegisterHotkey(Keys.Right, KeyModifiers.Alt);
            hotkeyHelper.OnHotkey += OnHotkey;
            if (Main_PlayList.SelectedItems.Count != 0)
            {
                Main_PlayList.SelectedItems[0].Selected = false;
            }
            int currentset = Core.PlayList.IndexOf(Core.currentset, 0);
            if (currentset == -1) { currentset = 0; }
            Main_PlayList.Items[currentset].Selected = true;
            Main_PlayList.EnsureVisible(currentset);
            Main_PlayList.Focus();
            Core.SetBG();
            if (Core.Isplaying)
            {
                Main_Time_Trackbar.Maximum = (int)Core.Durnation * 1000;
                Main_Time_Trackbar.Enabled = true;
                UpdateTimer.Enabled = true;
                Main_Play.Text = "暂停";
                Main_Stop.Enabled = true;

            }
            else
            {
                Main_Time_Trackbar.Enabled = false;
                UpdateTimer.Enabled = false;
                Main_Play.Text = "播放";
                Main_Stop.Enabled = false;
            }
            this.Visible = true;


        }
        private void radPageView1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (Main_PageView.SelectedPage == Main_PageView_Page2)
            {
                if (!Core.Scoresearched)
                {
                    string scorepath = Path.Combine(Settings.Default.OSUpath, "scores.db");
                    if (File.Exists(scorepath)) { OsuDB.ReadScore(scorepath); Core.Scoresearched = true; Main_File_Import_Scores.Text = "重新导入scores.db"; }
                }
                Setscore();
            }
        }
        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            Core.SetQQ(true);
            Main_QQ_Hint_Label.Text = "当前同步QQ：" + Settings.Default.QQuin;
            Main_Option_Sync_QQ.IsChecked = Settings.Default.SyncQQ;
        }
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            Main_Time_Trackbar.Value = (int)Core.Position * 1000;
            Main_Time_Display.Text = String.Format("{0}:{1:D2} / {2}:{3:D2}", (int)Core.Position / 60,
                (int)Core.Position % 60, (int)Core.Durnation / 60,
                (int)Core.Durnation % 60);
            if (Core.Willnext) { Stop(); PlayNext(); }
        }
        private void NextTimer_Tick(object sender, EventArgs e)
        {
            NextTimer.Enabled = false;
            Stop();
            PlayNext();
        }
        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            SearchTimer.Enabled = false;
            Core.search(Main_Search_Box.Text);
            RefreshList();
        }
        HotkeyHelper hotkeyHelper;
        int playKey;
        int nextKey;
        int playKey1;
        int playKey2;
        int nextKey1;
        private void Main_Shown(object sender, EventArgs e)
        {
            Core.init(this.Main_Main_Display.Handle, this.Main_Main_Display.Size);
            SetForm();
            RefreshList();
            this.VisibleChanged += Main_VisibleChanged;
            this.SizeChanged += Main_SizeChanged;
            Core.MainIsVisible = true;
            hotkeyHelper = new HotkeyHelper(this.Handle);
            playKey = hotkeyHelper.RegisterHotkey(Keys.F5, KeyModifiers.Alt);
            playKey1 = hotkeyHelper.RegisterHotkey(Keys.Play, KeyModifiers.None);
            playKey2 = hotkeyHelper.RegisterHotkey(Keys.Pause, KeyModifiers.None);
            nextKey = hotkeyHelper.RegisterHotkey(Keys.Right, KeyModifiers.Alt);
            nextKey1 = hotkeyHelper.RegisterHotkey(Keys.MediaNextTrack, KeyModifiers.None);
            hotkeyHelper.OnHotkey += OnHotkey;
            this.Main_Main_Display.ResumeLayout();
        }
        private void OnHotkey(int hotkeyID)
        {
            if (hotkeyID == playKey || hotkeyID == playKey1 || hotkeyID == playKey2)
            {
                Main_Play.PerformClick();
            }
            else if (hotkeyID == nextKey || hotkeyID == nextKey1)
            {
                Main_PlayNext.PerformClick();
            }
        }
        private void Main_SizeChanged(object sender, EventArgs e)
        {
            if (Core.MainIsVisible)
            {
                Core.Resize(Main_Main_Display.Size);
            }
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
            }
        }

        private void radMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void RightClick_Opening(object sender, CancelEventArgs e)
        {
            if (Main_PlayList.SelectedItems.Count == 0) { e.Cancel = true; return; }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            int index = Main_PlayList.SelectedIndices[0];
            Core.Remove(index);
            if (Main_PlayList.Items.Count != 0)
            {
                RefreshList(index);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Core.Stop();
            using (var outStream = new FileStream("tmp.osr", FileMode.Create))
            {
                using (var Wr = new BinaryWriter(outStream))
                {

                    Wr.Write((byte)0);
                    Wr.Write((int)20140127);
                    Wr.Write((byte)0x0b);
                    Wr.Write(Core.CurrentBeatmap.Hash);
                    Wr.Write((byte)0x0b);
                    Wr.Write("osu!");
                    var res = MD5.Create().ComputeHash(
                        Encoding.UTF8.GetBytes(string.Format(
                        PrivateConfig.ScoreHash, Core.CurrentBeatmap.Hash)));
                    var sb = new StringBuilder();
                    foreach (byte b in res)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    Wr.Write((byte)0x0b);
                    Wr.Write(sb.ToString());
                    Wr.Write((UInt16)1);
                    Wr.Write((UInt64)0);
                    Wr.Write((UInt16)0);
                    Wr.Write((UInt32)0);
                    Wr.Write((UInt16)1);
                    Wr.Write((byte)1);
                    Wr.Write((UInt32)2048);
                    Wr.Write((byte)0x0b);
                    Wr.Write("");
                    Wr.Write(new DateTime(2014, 1, 1).Ticks);
                    Wr.Write(0);
                    Wr.Write((UInt32)0);
                }
            }
            Process.Start("tmp.osr");
        }
    }
}