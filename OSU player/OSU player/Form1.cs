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

//using OSU_player.Selfupdate;
//using OSU_player.Core;
//using OSU_player.OsuDB;

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
		public string uin;
		public Videofiles uni_Video = new Videofiles();
		//Public uni_Audio As New Audiofiles
		public Stopwatch uni_timer = new Stopwatch();
		public bool first_P = true;
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
		}
		private void setbg()
		{
			System.Drawing.Image tmpimg = default(System.Drawing.Image);
			string bgpath = "";
			if (tmpbm.background == "")
			{
				bgpath = Core.defaultBG;
			}
			else
			{
				bgpath = System.IO.Path.Combine(tmpbm.location, tmpbm.background);
			}
			tmpimg = Image.FromFile(bgpath);
			Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
			Panel1.BackgroundImage = tmpimg.GetThumbnailImage(Panel1.Width, Panel1.Height, myCallback, IntPtr.Zero);
			tmpimg.Dispose();
		}
		private void Play()
		{
			uni_timer.Start();
			if (tmpbm.haveVideo)
			{
				uni_Video.init(System.IO.Path.Combine(tmpbm.location, tmpbm.Video));
				uni_Video.Play(this.Panel1);
				uni_Video.Pause();
				uni_Video.Pause();
			}
			TrackBar1.Enabled = true;
			first_P = false;
		}
		public void PlayButton_Click(object sender, EventArgs e)
		{
			if (PlayButton.Text == "播放")
			{
				if (first_P)
				{
					Play();
				}
				else
				{
					uni_Video.Pause();
				}
				PlayButton.Text = "暂停";
			}
			else
			{
				//uni_Video.Pause()
				//uni_timer.Stop()
				PlayButton.Text = "播放";
			}
		}
		public void Button1_Click(object sender, EventArgs e)
		{
			Form2.Default.Show();
		}
		public void ListView1_DoubleClick(object sender, EventArgs e)
		{
			Play();
			PlayButton.Text = "暂停";
		}
		public bool ThumbnailCallback()
		{
			return false;
		}
        private void TrackBar1_MouseUp(object sender, EventArgs e)
        {
            uni_Video.seek(TrackBar1.Value / 100 * uni_Video.durnation);

        }
		public void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((ListBox1.SelectedIndices.Count == 0) || (ListView1.SelectedIndices.Count == 0))
			{
				return;
			}
			tmpbm = tmp.Diffs[ListBox1.SelectedIndex];
			setbg();
			first_P = true;
			PlayButton.Text = "播放";
		}
        private void Form1_Load(object sender, EventArgs e)
        {
            Un4seen.Bass.BassNet.Registration("sqh1994@163.com", "2X280331512622");
            //check_update()
            Core.Getpath();
        }
        private void SearchButton_Click(object sender, EventArgs e)
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
            try
            {
                uni_Video.dispose();
                TrackBar1.Enabled = false;
            }
            catch (Exception)
            {

            }

            setbg();

        }

	}
	
}
