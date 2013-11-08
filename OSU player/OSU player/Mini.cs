﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace OSU_player
{
    public partial class Mini : Form
    {
        public Mini()
        {
            InitializeComponent();

        }
        private bool Front;
        private bool seeking = false;
        #region 有关窗体的设置
        [DllImport("gdi32.dll")]
        public static extern int CreateRoundRectRgn(int x1, int y1, int x2, int y2, int x3, int y3);

        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hwnd, int hRgn, Boolean bRedraw);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject", CharSet = CharSet.Ansi)]
        public static extern int DeleteObject(int hObject);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int HTCAPTION = 2;
        [DllImport("User32.dll")]
        public static extern bool PtInRect(ref Rectangle r, Point p);
        /// <summary>
        /// 设置窗体的圆角矩形
        /// </summary>
        /// <param name="form">需要设置的窗体</param>
        /// <param name="rgnRadius">圆角矩形的半径</param>
        public static void SetFormRoundRectRgn(Form form, int rgnRadius)
        {
            int hRgn = 0;
            hRgn = CreateRoundRectRgn(0, 0, form.Width + 1, form.Height + 1, rgnRadius, rgnRadius);
            SetWindowRgn(form.Handle, hRgn, true);
            DeleteObject(hRgn);
        }
        public static void MoveWindow(Form form)
        {
            ReleaseCapture();
            SendMessage(form.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                MoveWindow(this);
            }
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.Opacity = 1d;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (this.Bounds.Contains(Cursor.Position)) { return; }
            this.Opacity = 0.5d;
        }
        #endregion
        private void imageButton4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Mini_Load(object sender, EventArgs e)
        {
            Front = false;
            SetFormRoundRectRgn(this, 12);
            if (Core.isplaying)
            {
                TrackSeek.MaxValue = (int)Core.durnation * 1000;
                Artist = Core.CurrentBeatmap.ArtistRomanized;
                Title = Core.CurrentBeatmap.TitleRomanized;
                UpdateTimer.Enabled = true;
                SetPlay(false);
                StopButton.Enabled = true;
            }
        }

        private void TrackVolume_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void imageButton2_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void imageButton1_Click(object sender, EventArgs e)
        {
            if (!StopButton.Enabled) { Play(); }
            else
            {
                if (!Core.isplaying)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        private void imageButton3_Click(object sender, EventArgs e)
        {
            Stop();
            PlayNext();
        }

        private void imageButton5_Click(object sender, EventArgs e)
        {
            if (Front)
            {
                Front = false;
                this.TopMost = false;
                imageButton5.BackgroundImage = global::OSU_player.Properties.Resources.Pin;
                imageButton5.BaseImage = global::OSU_player.Properties.Resources.Pin;
                imageButton5.ClickImage = global::OSU_player.Properties.Resources.Pined;
                imageButton5.EnterImage = global::OSU_player.Properties.Resources.PinE;
            }
            else
            {
                Front = true;
                this.TopMost = true;
                imageButton5.BackgroundImage = global::OSU_player.Properties.Resources.Pined;
                imageButton5.BaseImage = global::OSU_player.Properties.Resources.Pined;
                imageButton5.ClickImage = global::OSU_player.Properties.Resources.Pin;
                imageButton5.EnterImage = global::OSU_player.Properties.Resources.Pined;
            }
        }
        private void SetPlay(bool play)
        {
            if (play)
            {
                PlayButton.BackgroundImage = OSU_player.Properties.Resources.play;
                PlayButton.BaseImage = OSU_player.Properties.Resources.play;
                PlayButton.ClickImage = OSU_player.Properties.Resources.PlayC;
                PlayButton.EnterImage = OSU_player.Properties.Resources.PlayE;
            }
            else
            {
                PlayButton.BackgroundImage = OSU_player.Properties.Resources.Pause;
                PlayButton.BaseImage = OSU_player.Properties.Resources.Pause;
                PlayButton.ClickImage = OSU_player.Properties.Resources.PauseC;
                PlayButton.EnterImage = OSU_player.Properties.Resources.PauseE;
            }
        }
        private void Stop()
        {
            UpdateTimer.Enabled = false;
            Core.Stop();
            TrackSeek.Enabled = false;
            TrackSeek.Value = 0;
            StopButton.Enabled = false;
            SetPlay(true);
        }
        private void Play()
        {
            UpdateTimer.Enabled = true;
            Core.Play();
            TrackSeek.Enabled = true;
            SetPlay(false);
            StopButton.Enabled = true;
            TrackSeek.MaxValue = (int)Core.durnation * 1000;
            Artist = Core.CurrentBeatmap.ArtistRomanized;
            Title = Core.CurrentBeatmap.TitleRomanized;
        }
        private void Pause()
        {
            UpdateTimer.Enabled = false;
            Core.Pause();
            SetPlay(true);
        }
        private void Resume()
        {
            Core.Resume();
            SetPlay(false);
        }
        private void PlayNext()
        {
            int next = Core.GetNext();
            Core.allsets[next].GetDetail();
            Play();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {

            TrackSeek.Value = (int)Core.position * 1000;

            LabelTime.Text = String.Format("{0}:{1:D2} | {2}:{3:D2}", (int)Core.position / 60,
                (int)Core.position % 60, (int)Core.durnation / 60,
                (int)Core.durnation % 60);
            if (Core.willnext) { PlayNext(); }


        }

        private void TrackVolume_ValueChanged(object sender, EventArgs e)
        {
            Core.SetVolume(1, (float)TrackVolume.Value / (float)TrackVolume.MaxValue);
        }

        private void TrackSeek_ValueChanged(object sender, EventArgs e)
        {
            if (seeking)
            {
                Core.seek((double)TrackSeek.Value / 1000);
            }
        }

        private void TrackSeek_MouseDown(object sender, MouseEventArgs e)
        {
            seeking = true;
        }

        private void TrackSeek_MouseUp(object sender, MouseEventArgs e)
        {
            seeking = false;
        }
        private PointF pA;
        private PointF pB;
        private string tempA;
        private string tempB;
        private string Title;
        private string Artist;
        private Brush brush = Brushes.White;
        private SizeF s;
        private Graphics g;
        private void GUITimer_Tick(object sender, EventArgs e)
        {
            Rectangle Rects = new Rectangle(this.Left, this.Top, this.Left + this.Width, this.Top + this.Height);
            if ((this.Top < 0 && PtInRect(ref Rects, Cursor.Position)))
            {
                this.Top = 0;
            }
            else
            {
                if (this.Top > -5 && this.Top < 5 && !(PtInRect(ref Rects, Cursor.Position)))
                {
                    this.Top = 2 - this.Height;
                }
            }
            g = this.LabelArtist.CreateGraphics();
            if (g.MeasureString(Artist, LabelArtist.Font).Width > LabelArtist.Width)
            {
                this.LabelArtist.Refresh();
                s = g.MeasureString(Artist, LabelArtist.Font);//测量文字长度  
                if (tempA != Artist)//文字改变时,重新显示  
                {
                    pA = new PointF(this.LabelArtist.Size.Width, 0);
                    tempA = Artist;
                }
                else
                    pA = new PointF(pA.X - 10, 0);//每次偏移10  
                if (pA.X <= -s.Width)
                    pA = new PointF(this.LabelArtist.Size.Width, 0);
                g.DrawString(Artist, LabelArtist.Font, brush, pA);
            }
            else { LabelArtist.Text = Artist; }
            g = this.LabelTitle.CreateGraphics();
            if (g.MeasureString(Title, LabelTitle.Font).Width > LabelTitle.Width)
            {
                this.LabelTitle.Refresh();
                s = g.MeasureString(Title, LabelTitle.Font);//测量文字长度  
                if (tempB != Title)//文字改变时,重新显示  
                {
                    pB = new PointF(this.LabelTitle.Size.Width, 0);
                    tempB = Title;
                }
                else
                    pB = new PointF(pB.X - 10, 0);//每次偏移10  
                if (pB.X <= -s.Width)
                    pB = new PointF(this.LabelTitle.Size.Width, 0);
                g.DrawString(Title, LabelTitle.Font, brush, pB);
            }
            else { LabelTitle.Text = Title; }
        }
    }
}
