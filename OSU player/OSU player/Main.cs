using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
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

        }

        #region 各种方法
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
            ListDetail.Items.Clear();
            ListDetail.Items.AddRange(Core.getdetail());
            Core.setBG();
        }
        private void Stop()
        {
            UpdateTimer.Enabled = false;
            Core.Stop();
            TrackSeek.Enabled = false;
            TrackSeek.Value = 0;
            StopButton.Enabled = false;
            PlayButton.Text = "播放";
        }
        private void Play()
        {
            if (Core.PlayList.Count != 0)
            {
                UpdateTimer.Enabled = true;
                Core.Play();
                TrackSeek.Enabled = true;
                PlayButton.Text = "暂停";
                StopButton.Enabled = true;
                TrackSeek.Maximum = (int)Core.Durnation * 1000;
            }
        }
        private void Pause()
        {
            UpdateTimer.Enabled = false;
            Core.Pause();
            PlayButton.Text = "播放";
        }
        private void Resume()
        {
            Core.Resume();
            UpdateTimer.Enabled = true;
            PlayButton.Text = "暂停";
        }
        private void PlayNext(bool play = true)
        {
            if (Core.PlayList.Count != 0)
            {
                int nextSongId = Core.GetNext();
                if (PlayList.SelectedItems.Count != 0)
                {
                    PlayList.SelectedItems[0].Selected = false;
                }
                PlayList.Items[nextSongId].Selected = true;
                PlayList.EnsureVisible(nextSongId);
                PlayList.Focus();
                SetDetail();
                if (play) { Play(); }
            }
        }
        private void setscore()
        {
            ScoreBox.Items.Clear();
            foreach (var item in Core.getscore(ScoreBox.Font))
            {
                if (item != null)
                {
                    ScoreBox.Items.Add(item);
                }
            }
        }
        #endregion
        private void SetForm()
        {
            LabelQQ.Text = "当前同步QQ：" + Core.uin;
            QQ状态同步.IsChecked = Core.syncQQ;
            TrackVolume.Value = 100 - (int)(Core.Allvolume * TrackVolume.Maximum);
            TrackFx.Value = (int)(Core.Fxvolume * TrackFx.Maximum);
            TrackMusic.Value = (int)(Core.Musicvolume * TrackMusic.Maximum);
            音效.IsChecked = Core.playfx;
            SB开关.IsChecked = Core.playsb;
            视频开关.IsChecked = Core.playvideo;
            radMenuComboItem1.ComboBoxElement.SelectedIndex = Core.Nextmode - 1;
        }
        private void RefreshList(int select = 0)
        {
            PlayList.Items.Clear();
            DiffList.Items.Clear();
            //PlayList.Enabled = false;
            //DiffList.Enabled = false;
            new Thread((ThreadStart)delegate()
                {
                    for (int i = 0; i < Core.PlayList.Count; i++)
                    {
                        PlayList.Invoke((MethodInvoker)delegate()
                        {
                            PlayList.Items.Add(Core.allsets[Core.PlayList[i]].ToString());
                        });
                    }
                    if (PlayList.Items.Count != 0)
                    {
                        PlayList.Invoke((MethodInvoker)delegate()
                        { PlayList.Items[select].Selected = true; });
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
            Process.Start(Path.Combine(Core.osupath, "osu!.exe"));
        }
        private void 手动指定OSU目录_Click(object sender, EventArgs e)
        {
            if (Core.Setpath())
            {
                PlayList.Items.Clear();
                DiffList.Items.Clear();
                Core.RefreashSet();
                RefreshList();
            }
        }
        private void 重新导入osu_Click(object sender, EventArgs e)
        {
            PlayList.Items.Clear();
            DiffList.Items.Clear();
            Core.RefreashSet();
            RefreshList();
        }
        private void 重新导入scores_Click(object sender, EventArgs e)
        {
            Core.Scores.Clear();
            string scorepath = Path.Combine(Core.osupath, "scores.db");
            if (File.Exists(scorepath)) { OsuDB.ReadScore(scorepath); Core.scoresearched = true; 重新导入scores.Text = "重新导入scores.db"; }
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
            if (Core.CurrentBeatmap.Background != null && Core.CurrentBeatmap.Background != "")
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
            SaveFileDialog dialog = new SaveFileDialog();
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
            Core.playfx = 音效.IsChecked;
            Properties.Settings.Default.PlayFx = Core.playfx;
        }
        private void 视频开关_Click(object sender, EventArgs e)
        {
            Core.playvideo = 视频开关.IsChecked;
            Properties.Settings.Default.PlayVideo = Core.playvideo;
        }
        private void radMenuComboItem1_ComboBoxElement_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            Core.Nextmode = radMenuComboItem1.ComboBoxElement.SelectedIndex + 1;
            Properties.Settings.Default.NextMode = Core.Nextmode;
        }
        private void QQ状态同步_Click(object sender, EventArgs e)
        {
            if (Core.syncQQ && Core.uin != "0") { Core.uni_QQ.Send2QQ(Core.uin, ""); }
            Core.syncQQ = QQ状态同步.IsChecked;
            Properties.Settings.Default.SyncQQ = Core.syncQQ;
        }
        private void SB开关_Click(object sender, EventArgs e)
        {
            Core.playsb = SB开关.IsChecked;
            Properties.Settings.Default.PlaySB = Core.playsb;
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
            using (ChooseColl dialog = new ChooseColl())
            {
                dialog.ShowDialog();
            }
            this.Visible = true;
            RefreshList();
        }
        private void TrackFx_Scroll(object sender, ScrollEventArgs e)
        {
            Core.SetVolume(3, (float)TrackFx.Value / (float)TrackFx.Maximum);
        }
        private void TrackMusic_Scroll(object sender, ScrollEventArgs e)
        {
            Core.SetVolume(2, (float)TrackMusic.Value / (float)TrackMusic.Maximum);
        }
        private void TrackVolume_Scroll(object sender, ScrollEventArgs e)
        {
            Core.SetVolume(1, 1.0f - (float)TrackVolume.Value / (float)TrackVolume.Maximum);
        }
        #endregion
        #region 第二排
        private void PlayList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PlayList.SelectedItems.Count == 0) { return; }
            DiffList.Items.Clear();
            if (Core.SetSet(PlayList.SelectedIndices[0])) { RefreshList(); PlayNext(false); }
            else
            {
                foreach (Beatmap s in Core.TmpSet.Diffs)
                {
                    DiffList.Items.Add(s.Version);
                }
                if (!File.Exists(Core.TmpSet.OsbPath))
                { 打开SB文件.Enabled = false; }
                else
                { 打开SB文件.Enabled = true; }
                DiffList.SelectedIndex = 0;
            }
        }
        private void DiffList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PlayList.SelectedIndices.Count == 0) { return; }
            if (Core.SetMap(DiffList.SelectedIndex)) { RefreshList(); PlayNext(false); }
            else
            {
                if (Core.scoresearched) { setscore(); }
                if (!StopButton.Enabled)
                {
                    Core.currentset = Core.PlayList[Core.tmpset];
                    Core.currentmap = Core.tmpmap;
                    SetDetail();
                }
                else if (Core.Isplaying)
                {
                    ListDetail.Items.Clear();
                    ListDetail.Items.AddRange(Core.getdetail());
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
            if (Core.SetSet(PlayList.SelectedIndices[0], true)) { RefreshList(); PlayNext(); }
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
            if (Core.SetMap(DiffList.SelectedIndex, true)) { RefreshList(); PlayNext(); }
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
            Core.seek((double)TrackSeek.Value / 1000);
        }

        void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape) { TextBox1.Text = ""; SearchButton.PerformClick(); }
            if (e.KeyChar == (char)13)
                SearchButton.PerformClick();
            else
            {
                SearchTimer.Enabled = false;
                SearchTimer.Enabled = true;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            // Stop();
            Core.search(TextBox1.Text);
            RefreshList();
        }
        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (!StopButton.Enabled)
            {
                Play();
            }
            else
            {
                if (PlayButton.Text == "播放")
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
            using (Mini dialog = new Mini())
            {
                dialog.ShowDialog();
            }
            playKey = hotkeyHelper.RegisterHotkey(Keys.F5, KeyModifiers.Alt);
            nextKey = hotkeyHelper.RegisterHotkey(Keys.Right, KeyModifiers.Alt);
            hotkeyHelper.OnHotkey += new HotkeyEventHandler(OnHotkey);
            if (PlayList.SelectedItems.Count != 0)
            {
                PlayList.SelectedItems[0].Selected = false;
            }
            int currentset = Core.PlayList.IndexOf(Core.currentset, 0);
            if (currentset == -1) { currentset = 0; }
            PlayList.Items[currentset].Selected = true;
            PlayList.EnsureVisible(currentset);
            PlayList.Focus();
            Core.setBG();
            if (Core.Isplaying)
            {
                TrackSeek.Maximum = (int)Core.Durnation * 1000;
                TrackSeek.Enabled = true;
                UpdateTimer.Enabled = true;
                PlayButton.Text = "暂停";
                StopButton.Enabled = true;

            }
            else
            {
                TrackSeek.Enabled = false;
                UpdateTimer.Enabled = false;
                PlayButton.Text = "播放";
                StopButton.Enabled = false;
            }
            this.Visible = true;


        }
        private void radPageView1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (radPageView1.SelectedPage == radPageViewPage2)
            {
                if (!Core.scoresearched)
                {
                    string scorepath = Path.Combine(Core.osupath, "scores.db");
                    if (File.Exists(scorepath)) { OsuDB.ReadScore(scorepath); Core.scoresearched = true; 重新导入scores.Text = "重新导入scores.db"; }
                }
                setscore();
            }
        }
        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            Core.SetQQ(true);
            LabelQQ.Text = "当前同步QQ：" + Core.uin;
            QQ状态同步.IsChecked = Core.syncQQ;
        }
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            TrackSeek.Value = (int)Core.Position * 1000;
            label1.Text = String.Format("{0}:{1:D2} / {2}:{3:D2}", (int)Core.Position / 60,
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
            Core.search(TextBox1.Text);
            RefreshList();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            Core.Stop();
            File.SetLastWriteTime(Path.Combine(Core.osupath, "Songs"), DateTime.Now);
            File.SetLastWriteTime(Core.CurrentBeatmap.Path, DateTime.Now);
        }
        HotkeyHelper hotkeyHelper;
        int playKey;
        int nextKey;
        int playKey1;
        int playKey2;
        int nextKey1;
        private void Main_Shown(object sender, EventArgs e)
        {
            Core.init(this.panel2.Handle, this.panel2.Size);
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
            hotkeyHelper.OnHotkey += new HotkeyEventHandler(OnHotkey);
            this.panel2.ResumeLayout();
        }
        private void OnHotkey(int hotkeyID)
        {
            if (hotkeyID == playKey || hotkeyID == playKey1 || hotkeyID == playKey2)
            {
                PlayButton.PerformClick();
            }
            else if (hotkeyID == nextKey || hotkeyID == nextKey1)
            {
                NextButton.PerformClick();
            }
        }
        private void Main_SizeChanged(object sender, EventArgs e)
        {
            if (Core.MainIsVisible)
            {
                Core.Resize(panel2.Size);
            }
        }

        private void radMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void RightClick_Opening(object sender, CancelEventArgs e)
        {
            if (PlayList.SelectedItems.Count == 0) { e.Cancel = true; return; }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            int index = PlayList.SelectedIndices[0];
            Core.remove(index);
            if (PlayList.Items.Count != 0)
            {
                RefreshList(index);
            }
        }
    }
}