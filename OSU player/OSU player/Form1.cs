using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.IO;
namespace OSU_player
{
    public partial class Form1
    {
        public Form1()
        {
            InitializeComponent();

            //Added to support default instance behavour in C#
            if (defaultInstance == null)
                defaultInstance = this;
        }

        #region Default Instance

        private static Form1 defaultInstance;

        /// <summary>
        /// Added by the VB.Net to C# Converter to support default instance behavour in C#
        /// </summary>
        public static Form1 Default
        {
            get
            {
                if (defaultInstance == null)
                {
                    defaultInstance = new Form1();
                    defaultInstance.FormClosed += new FormClosedEventHandler(defaultInstance_FormClosed);
                }

                return defaultInstance;
            }
        }

        static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
        {
            defaultInstance = null;
        }

        #endregion
        public Videofiles uni_Video = new Videofiles();
        public Audiofiles uni_Audio = new Audiofiles();
        public QQ uni_QQ = new QQ();
       // Thread DelayVideo = new Thread((delegate() { Thread.Sleep(10); }));
        BeatmapSet CurrentSet;
        Beatmap CurrentBeatmap;
        BeatmapSet TmpSet;
        Beatmap TmpBeatmap;
        public void AskForExit(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            DialogResult close;
            close = MessageBox.Show("确认退出？", "提示", MessageBoxButtons.YesNo);
            if (close == DialogResult.Yes)
            {
                this.Dispose();
            }
            else
            {
                e.Cancel = true;
            }
            uni_QQ.Send2QQ(Core.uin, "");
        }
        public bool ThumbnailCallback()
        {
            return false;
        }
        private void printdetail()
        {
            ListDetail.Items.Clear();
            for (int i = (int)OSUfile.FileVersion; i < (int)OSUfile.OSUfilecount; i++)
            {
                ListViewItem tmpl = new ListViewItem(i.ToString());
                tmpl.SubItems.Add(TmpBeatmap.Rawdata[i]);
                ListDetail.Items.Add(tmpl);
            }
        }
        private void setbg()
        {
            printdetail();
            //System.Drawing.Image tmpimg = default(System.Drawing.Image);
          //  string bgpath = CurrentBeatmap.Background;
           // tmpimg = Image.FromFile(bgpath);
           // Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
           // Panel1.BackgroundImage = tmpimg.GetThumbnailImage(Panel1.Width, Panel1.Height, myCallback, IntPtr.Zero);
           // tmpimg.Dispose();
            uni_Video.initbg(CurrentBeatmap.Background);
            uni_Video.Play(this.Panel1);
        }
        private void Stop()
        {
            AVsyncer.Enabled = false;
            uni_Audio.Stop();
            uni_Video.Stop();
            TrackBar1.Enabled = false;
            TrackBar1.Value = 0;
          //  DelayVideo.Abort();
        }
        private void Play()
        {
            Stop();
            setbg();
            if (CurrentBeatmap.haveVideo)
            {
                uni_Video.init(Path.Combine(CurrentBeatmap.location, CurrentBeatmap.Video));
               if (CurrentBeatmap.VideoOffset > 0)
            //    {
            //        DelayVideo = new Thread((delegate()
                    {
                        Thread.Sleep(CurrentBeatmap.VideoOffset);
        //                uni_Video.Play(this.Panel1);
         //           }));
         //           DelayVideo.Start();
                }
                else { uni_Video.Play(this.Panel1); }
            }
            uni_Audio.init(CurrentBeatmap.Audio);
            uni_Audio.Play();
            TrackBar1.Enabled = true;
            AVsyncer.Enabled = true;
            uni_QQ.Send2QQ(Core.uin, CurrentBeatmap.name);
            PlayButton.Text = "暂停";
        }
        private void Pause()
        {
            uni_Video.Pause();
            uni_Audio.Pause();
            uni_QQ.Send2QQ(Core.uin, "");
            PlayButton.Text = "播放";

        }
        private void Resume()
        {
            uni_Video.Pause();
            uni_Audio.Pause();
            AVsyncer.Enabled = true;
            PlayButton.Text = "暂停";
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox1.Items.Clear();
            if (ListView1.FocusedItem == null) { return; }
            TmpSet = Core.allsets[ListView1.FocusedItem.Index];
            if (!TmpSet.detailed)
            {
                TmpSet.GetDetail();
            }
            foreach (var s in TmpSet.diffstr)
            {
                ListBox1.Items.Add(s);
            }
            ListBox1.SelectedIndex = 0;
            //因为改变selectedindex会触发listbox的进程，所以以下省略了
            /*  if (uni_Audio.isstopped)
              {
                  CurrentSet = TmpSet;
                  CurrentBeatmap = TmpBeatmap;
                  setbg();
              }
              else if (uni_Audio.isplaying)
              {
                  printdetail();
              }
              else
              {
                  Stop();
                  setbg();
              }*/
        }
        public void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListView1.SelectedIndices.Count == 0)
            {
                return;
            }
            TmpBeatmap = TmpSet.Diffs[ListBox1.SelectedIndex];
            if (uni_Audio.isstopped)
            {
                CurrentSet = TmpSet;
                CurrentBeatmap = TmpBeatmap;
                setbg();
            }
            else if (uni_Audio.isplaying)
            {
                printdetail();
            }
            else
            {
                Stop();
                setbg();
            }
        }
        private void ListView1_DoubleClick(object sender, EventArgs e)
        {
            TmpSet = Core.allsets[ListView1.FocusedItem.Index];
            TmpBeatmap = TmpSet.Diffs[0];
            CurrentSet = TmpSet;
            CurrentBeatmap = TmpBeatmap;
            Play();
        }
        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            TmpSet = Core.allsets[ListView1.FocusedItem.Index];
            TmpBeatmap = TmpSet.Diffs[ListBox1.SelectedIndex];
            CurrentSet = TmpSet;
            CurrentBeatmap = TmpBeatmap;
            Play();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Un4seen.Bass.BassNet.Registration("sqh1994@163.com", "2X280331512622");
            Selfupdate.check_update();
            Core.Getpath();
        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text == "")
            {
                foreach (BeatmapSet tmpbms in Core.allsets)
                {
                    ListViewItem tmpl = new ListViewItem(tmpbms.name);
                    ListView1.Items.Add(tmpl);
                }
            }
            else
            {
                ListView1.Clear();
                foreach (BeatmapSet tmpbms in Core.allsets)
                {
                    if (tmpbms.name.Contains(TextBox1.Text))
                    {
                        ListViewItem tmpl = new ListViewItem(tmpbms.name);
                        ListView1.Items.Add(tmpl);
                    }
                }
            }
        }
        private void PlayButton_Click(object sender, EventArgs e)
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
        private void TrackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            uni_Audio.seek(TrackBar1.Value * uni_Audio.durnation / TrackBar1.Maximum);
            uni_Video.seek(TrackBar1.Value * uni_Audio.durnation / TrackBar1.Maximum + CurrentBeatmap.VideoOffset);
            AVsyncer.Enabled = true;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Form2.Default.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
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
        private void AVsync(object sender, EventArgs e)
        {
            if (uni_Audio.durnation != 0)
            {
                TrackBar1.Value = (int)Math.Round((uni_Audio.position / uni_Audio.durnation) * TrackBar1.Maximum);
            }
        }
        private void TrackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            AVsyncer.Enabled = false;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            scanforset(e.Argument.ToString());
        }
        public void scanforset(string path)
        {
            string[] osufiles = Directory.GetFiles(path, "*.osu");
            if (osufiles.Length != 0)
            {
                BeatmapSet tmp = new BeatmapSet(path);
                //tmp.GetDetail();
                Core.allsets.Add(tmp);
                this.backgroundWorker1.ReportProgress(0, "");
                this.backgroundWorker1.ReportProgress(1, tmp.name);
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
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                Invoke((EventHandler)delegate
                {
                    if (e.UserState.ToString().Length > 0)
                    {
                        ListViewItem tmpl = new ListViewItem(e.UserState.ToString());
                        Form1.Default.ListView1.Items.Add(tmpl);
                    }
                });
            }
        }

        private void 随机播放ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (随机播放ToolStripMenuItem1.Checked) { return; }
            顺序播放ToolStripMenuItem.Checked = false;

        }
        private void 顺序播放ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (顺序播放ToolStripMenuItem.Checked) { return; }
            顺序播放ToolStripMenuItem.Checked = false;

        }

        private void 单曲循环ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (单曲循环ToolStripMenuItem.Checked) { return; }
            顺序播放ToolStripMenuItem.Checked = false;

        }
    }

}
