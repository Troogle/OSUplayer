using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

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
        BeatmapSet tmp;
        Beatmap tmpbm;
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
            uni_QQ.Send2QQ(Core.uin,"");
        }
        private void setbg()
        {
            System.Drawing.Image tmpimg = default(System.Drawing.Image);
            string bgpath = tmpbm.Background;
            tmpimg = Image.FromFile(bgpath);
            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            Panel1.BackgroundImage = tmpimg.GetThumbnailImage(Panel1.Width, Panel1.Height, myCallback, IntPtr.Zero);
            tmpimg.Dispose();
        }
        private void Play()
        {
            if (tmpbm.haveVideo)
            {
                uni_Video.init(System.IO.Path.Combine(tmpbm.location, tmpbm.Video));
                uni_Video.Play(this.Panel1);
            }
            else 
            { 
            uni_Video.Dispose();
            }
            uni_Audio.init(tmpbm.Audio);
            uni_Audio.Play();
            timer1.Enabled = true;
            TrackBar1.Enabled = true;
            uni_QQ.Send2QQ(Core.uin, tmp.name);
        }
        public bool ThumbnailCallback()
        {
            return false;
        }
        public void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((ListBox1.SelectedIndices.Count == 0) || (ListView1.SelectedIndices.Count == 0))
            {
                return;
            }
            tmpbm = tmp.Diffs[ListBox1.SelectedIndex];
            setbg();
            PlayButton.Text = "暂停";
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
        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListView1.SelectedIndices.Count == 0)
            {
                return;
            }
            ListBox1.Items.Clear();
            ListDetail.Items.Clear();
            tmp = Core.allsets[ListView1.SelectedIndices[0]];
            if (!tmp.detailed)
            {
                tmp.GetDetail();
            }
            foreach (var s in tmp.diffstr)
            {
                ListBox1.Items.Add(s);
            }
            tmpbm = tmp.Diffs[0];
            for (int i = (int)OSUfile.FileVersion; i < (int)OSUfile.OSUfilecount; i++)
            {
                ListViewItem tmpl = new ListViewItem(i.ToString());
                tmpl.SubItems.Add(tmpbm.Rawdata[i]);
                ListDetail.Items.Add(tmpl);
            }
            TrackBar1.Enabled = false;
            setbg();
        }
        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (PlayButton.Text == "播放")
            {
                    uni_Video.Pause();
                    uni_Audio.Pause();
                PlayButton.Text = "暂停";
            }
            else
            {
                uni_Video.Pause();
                uni_Audio.Pause();
                uni_QQ.Send2QQ(Core.uin, "");
                PlayButton.Text = "播放";
            }

        }
        private void TrackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            uni_Audio.seek(TrackBar1.Value * uni_Audio.durnation / TrackBar1.Maximum);
            timer1.Enabled = true;

        }
        private void ListView1_DoubleClick(object sender, EventArgs e)
        {
            Play();
            PlayButton.Text = "暂停";
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Form2.Default.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Core.Superscanforset();
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw (new FormatException("Failed to read song path", ex));
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
         if (uni_Audio.durnation!=0) {
             TrackBar1.Value = (int)Math.Round((uni_Audio.position / uni_Audio.durnation) * TrackBar1.Maximum);
         }
        }

        private void TrackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            timer1.Enabled = false;
        }
    }

}
