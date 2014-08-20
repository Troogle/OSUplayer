using OSUplayer.Properties;
using OSUplayer.Uilties;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace OSUplayer
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
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                Win32.MoveWindow(this);
            }
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Opacity = 1d;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (Bounds.Contains(Cursor.Position)) { return; }
            Opacity = 0.5d;
        }
        #endregion
        private void imageButton4_Click(object sender, EventArgs e)
        {
            hotkeyHelper.UnregisterHotkeys();
            Dispose();
        }

        private void Mini_Load(object sender, EventArgs e)
        {
            Front = false;
            Win32.SetFormRoundRectRgn(this, 12);
            Mini_Volume_TrackBar.Value = (int)(100 * Settings.Default.Allvolume);
            if (!Bounds.Contains(Cursor.Position)) { Opacity = 0.5d; }
            if (Core.Isplaying)
            {
                Mini_Seek_TrackBar.MaxValue = (int)Core.Durnation * 1000;
                Mini_Seek_TrackBar.Enabled = true;
                Artist = Core.CurrentBeatmap.Artist;
                Title = Core.CurrentBeatmap.Title;
                Mini_UpdateTimer.Enabled = true;
                SetPlay(false);
                Mini_Stop.Enabled = true;
            }
        }

        private void imageButton2_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void imageButton1_Click(object sender, EventArgs e)
        {
            if (!Mini_Stop.Enabled) { Play(); }
            else
            {
                if (!Core.Isplaying)
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
                TopMost = false;
                Mini_Pin.BackgroundImage = Resources.Pin;
                Mini_Pin.BaseImage = Resources.Pin;
                Mini_Pin.ClickImage = Resources.Pined;
                Mini_Pin.EnterImage = Resources.PinE;
            }
            else
            {
                Front = true;
                TopMost = true;
                Mini_Pin.BackgroundImage = Resources.Pined;
                Mini_Pin.BaseImage = Resources.Pined;
                Mini_Pin.ClickImage = Resources.Pin;
                Mini_Pin.EnterImage = Resources.Pined;
            }
        }
        private void SetPlay(bool play)
        {
            if (play)
            {
                Mini_Play.BackgroundImage = Resources.play;
                Mini_Play.BaseImage = Resources.play;
                Mini_Play.ClickImage = Resources.PlayC;
                Mini_Play.EnterImage = Resources.PlayE;
            }
            else
            {
                Mini_Play.BackgroundImage = Resources.Pause;
                Mini_Play.BaseImage = Resources.Pause;
                Mini_Play.ClickImage = Resources.PauseC;
                Mini_Play.EnterImage = Resources.PauseE;
            }
        }
        private void Stop()
        {
            Mini_UpdateTimer.Enabled = false;
            Core.Stop();
            Mini_Seek_TrackBar.Enabled = false;
            Mini_Seek_TrackBar.Value = 0;
            Mini_Stop.Enabled = false;
            SetPlay(true);
            Artist = "";
            Title = "";
        }
        private void Play()
        {
            if (Core.PlayList.Count == 0) return;
            Mini_UpdateTimer.Enabled = true;
            Core.Play();
            Mini_Seek_TrackBar.Enabled = true;
            SetPlay(false);
            Mini_Stop.Enabled = true;
            Mini_Seek_TrackBar.MaxValue = (int)Core.Durnation * 1000;
            Mini_Artist_Label.Refresh();
            Mini_Title_Label.Refresh();
            Artist = Core.CurrentBeatmap.Artist;
            Title = Core.CurrentBeatmap.Title;
        }
        private void Pause()
        {
            Mini_UpdateTimer.Enabled = false;
            Core.PauseOrResume();
            SetPlay(true);
        }
        private void Resume()
        {
            Core.PauseOrResume();
            SetPlay(false);
        }
        private void PlayNext()
        {
            Mini_UpdateTimer.Enabled = false;
            Artist = "";
            Title = "";
            if (Core.PlayList.Count == 0) return;
            int next = Core.GetNext();
            Play();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {

            Mini_Seek_TrackBar.Value = (int)Core.Position * 1000;
            Mini_Time_Label.Text = String.Format("{0}:{1:D2} | {2}:{3:D2}", (int)Core.Position / 60,
                (int)Core.Position % 60, (int)Core.Durnation / 60,
                (int)Core.Durnation % 60);
            if (Core.Willnext) { Stop(); PlayNext(); }
        }

        private void TrackVolume_ValueChanged(object sender, EventArgs e)
        {
            Core.SetVolume(1, Mini_Volume_TrackBar.Value / (float)Mini_Volume_TrackBar.MaxValue);
        }

        private void TrackSeek_ValueChanged(object sender, EventArgs e)
        {
            if (seeking)
            {
                Core.Seek((double)Mini_Seek_TrackBar.Value / 1000);
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
        private readonly Brush brush = Brushes.White;
        private SizeF s;
        private Graphics g;
        private void GUITimer_Tick(object sender, EventArgs e)
        {
            var Rects = new Rectangle(Left, Top, Left + Width, Top + Height);
            if ((Top < 0 && Win32.PtInRect(ref Rects, Cursor.Position)))
            {
                Top = 0;
            }
            else
            {
                if (Top > -5 && Top < 5 && !(Win32.PtInRect(ref Rects, Cursor.Position)))
                {
                    Top = 2 - Height;
                }
            }
            g = Mini_Artist_Label.CreateGraphics();
            if (g.MeasureString(Artist, Mini_Artist_Label.Font).Width > Mini_Artist_Label.Width)
            {
                Mini_Artist_Label.Refresh();
                s = g.MeasureString(Artist, Mini_Artist_Label.Font);//测量文字长度  
                if (tempA != Artist)//文字改变时,重新显示  
                {
                    pA = new PointF(Mini_Artist_Label.Size.Width, 0);
                    tempA = Artist;
                }
                else
                    pA = new PointF(pA.X - 10, 0);//每次偏移10  
                if (pA.X <= -s.Width)
                    pA = new PointF(Mini_Artist_Label.Size.Width, 0);
                g.DrawString(Artist, Mini_Artist_Label.Font, brush, pA);
            }
            else { Mini_Artist_Label.Text = Artist; }
            g = Mini_Title_Label.CreateGraphics();
            if (g.MeasureString(Title, Mini_Title_Label.Font).Width > Mini_Title_Label.Width)
            {
                Mini_Title_Label.Refresh();
                s = g.MeasureString(Title, Mini_Title_Label.Font);//测量文字长度  
                if (tempB != Title)//文字改变时,重新显示  
                {
                    pB = new PointF(Mini_Title_Label.Size.Width, 0);
                    tempB = Title;
                }
                else
                    pB = new PointF(pB.X - 10, 0);//每次偏移10  
                if (pB.X <= -s.Width)
                    pB = new PointF(Mini_Title_Label.Size.Width, 0);
                g.DrawString(Title, Mini_Title_Label.Font, brush, pB);
            }
            else { Mini_Title_Label.Text = Title; }
        }
        HotkeyHelper hotkeyHelper;
        int playKey;
        int nextKey;
        int playKey1;
        int playKey2;
        int nextKey1;
        private void Mini_Shown(object sender, EventArgs e)
        {
            hotkeyHelper = new HotkeyHelper(Handle);
            playKey = hotkeyHelper.RegisterHotkey(Keys.F5, KeyModifiers.Alt);
            playKey1 = hotkeyHelper.RegisterHotkey(Keys.Play, KeyModifiers.None);
            playKey2 = hotkeyHelper.RegisterHotkey(Keys.Pause, KeyModifiers.None);
            nextKey = hotkeyHelper.RegisterHotkey(Keys.Right, KeyModifiers.Alt);
            nextKey1 = hotkeyHelper.RegisterHotkey(Keys.MediaNextTrack, KeyModifiers.None);
            hotkeyHelper.OnHotkey += OnHotkey;
        }
        private void OnHotkey(int hotkeyID)
        {
            if (hotkeyID == playKey || hotkeyID == playKey1 || hotkeyID == playKey2)
            {
                imageButton1_Click(null, EventArgs.Empty);
            }
            else if (hotkeyID == nextKey || hotkeyID == nextKey1)
            {
                imageButton3_Click(null, EventArgs.Empty);
            }
        }
    }
}
