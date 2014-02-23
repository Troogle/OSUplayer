namespace OSUplayer
{
    partial class Mini
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            gTrackBar.ColorLinearGradient colorLinearGradient1 = new gTrackBar.ColorLinearGradient();
            gTrackBar.ColorLinearGradient colorLinearGradient2 = new gTrackBar.ColorLinearGradient();
            gTrackBar.ColorLinearGradient colorLinearGradient3 = new gTrackBar.ColorLinearGradient();
            gTrackBar.ColorLinearGradient colorLinearGradient4 = new gTrackBar.ColorLinearGradient();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mini));
            this.Mini_UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.Mini_Seek_TrackBar = new gTrackBar.gTrackBar();
            this.Mini_Volume_TrackBar = new gTrackBar.gTrackBar();
            this.Mini_Artist_Label = new System.Windows.Forms.Label();
            this.Mini_Title_Label = new System.Windows.Forms.Label();
            this.Mini_Time_Label = new System.Windows.Forms.Label();
            this.Mini_GUITimer = new System.Windows.Forms.Timer(this.components);
            this.Mini_Pin = new OSUplayer.ImageButton();
            this.Mini_Main_Switcher = new OSUplayer.ImageButton();
            this.Mini_PlayNext = new OSUplayer.ImageButton();
            this.Mini_Stop = new OSUplayer.ImageButton();
            this.Mini_Play = new OSUplayer.ImageButton();
            this.SuspendLayout();
            // 
            // UpdateTimer
            // 
            this.Mini_UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // TrackSeek
            // 
            this.Mini_Seek_TrackBar.BackColor = System.Drawing.Color.Transparent;
            this.Mini_Seek_TrackBar.BrushStyle = gTrackBar.gTrackBar.eBrushStyle.Image;
            this.Mini_Seek_TrackBar.ChangeLarge = 10000;
            this.Mini_Seek_TrackBar.ChangeSmall = 1000;
            this.Mini_Seek_TrackBar.Enabled = false;
            this.Mini_Seek_TrackBar.FloatValue = false;
            this.Mini_Seek_TrackBar.FloatValueFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(82)))), ((int)(((byte)(92)))));
            this.Mini_Seek_TrackBar.Label = null;
            this.Mini_Seek_TrackBar.Location = new System.Drawing.Point(12, 151);
            this.Mini_Seek_TrackBar.MaxValue = 1000;
            this.Mini_Seek_TrackBar.Name = "TrackSeek";
            this.Mini_Seek_TrackBar.ShowFocus = false;
            this.Mini_Seek_TrackBar.Size = new System.Drawing.Size(588, 50);
            colorLinearGradient1.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(167)))), ((int)(((byte)(170)))));
            colorLinearGradient1.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(167)))), ((int)(((byte)(170)))));
            this.Mini_Seek_TrackBar.SliderColorHigh = colorLinearGradient1;
            colorLinearGradient2.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(82)))), ((int)(((byte)(92)))));
            colorLinearGradient2.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(82)))), ((int)(((byte)(92)))));
            this.Mini_Seek_TrackBar.SliderColorLow = colorLinearGradient2;
            this.Mini_Seek_TrackBar.SliderImage = global::OSUplayer.Properties.Resources.track;
            this.Mini_Seek_TrackBar.SliderSize = new System.Drawing.Size(38, 48);
            this.Mini_Seek_TrackBar.SliderWidthHigh = 10F;
            this.Mini_Seek_TrackBar.SliderWidthLow = 10F;
            this.Mini_Seek_TrackBar.TabIndex = 5;
            this.Mini_Seek_TrackBar.TickThickness = 1F;
            this.Mini_Seek_TrackBar.UpDownShow = false;
            this.Mini_Seek_TrackBar.Value = 0;
            this.Mini_Seek_TrackBar.ValueAdjusted = 0F;
            this.Mini_Seek_TrackBar.ValueDivisor = gTrackBar.gTrackBar.eValueDivisor.e1;
            this.Mini_Seek_TrackBar.ValueStrFormat = "";
            this.Mini_Seek_TrackBar.ValueChanged += new gTrackBar.gTrackBar.ValueChangedEventHandler(this.TrackSeek_ValueChanged);
            this.Mini_Seek_TrackBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TrackSeek_MouseDown);
            this.Mini_Seek_TrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrackSeek_MouseUp);
            // 
            // TrackVolume
            // 
            this.Mini_Volume_TrackBar.BackColor = System.Drawing.Color.Transparent;
            this.Mini_Volume_TrackBar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Mini_Volume_TrackBar.BrushStyle = gTrackBar.gTrackBar.eBrushStyle.Image;
            this.Mini_Volume_TrackBar.FloatValue = false;
            this.Mini_Volume_TrackBar.FloatValueFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(82)))), ((int)(((byte)(92)))));
            this.Mini_Volume_TrackBar.Label = null;
            this.Mini_Volume_TrackBar.Location = new System.Drawing.Point(304, 1);
            this.Mini_Volume_TrackBar.MaxValue = 100;
            this.Mini_Volume_TrackBar.Name = "TrackVolume";
            this.Mini_Volume_TrackBar.ShowFocus = false;
            this.Mini_Volume_TrackBar.Size = new System.Drawing.Size(304, 37);
            colorLinearGradient3.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(167)))), ((int)(((byte)(170)))));
            colorLinearGradient3.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(167)))), ((int)(((byte)(170)))));
            this.Mini_Volume_TrackBar.SliderColorHigh = colorLinearGradient3;
            colorLinearGradient4.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(82)))), ((int)(((byte)(92)))));
            colorLinearGradient4.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(82)))), ((int)(((byte)(92)))));
            this.Mini_Volume_TrackBar.SliderColorLow = colorLinearGradient4;
            this.Mini_Volume_TrackBar.SliderImage = global::OSUplayer.Properties.Resources.VolumeHandle;
            this.Mini_Volume_TrackBar.SliderSize = new System.Drawing.Size(27, 43);
            this.Mini_Volume_TrackBar.SliderWidthHigh = 10F;
            this.Mini_Volume_TrackBar.SliderWidthLow = 10F;
            this.Mini_Volume_TrackBar.TabIndex = 6;
            this.Mini_Volume_TrackBar.TickThickness = 1F;
            this.Mini_Volume_TrackBar.UpDownShow = false;
            this.Mini_Volume_TrackBar.Value = 100;
            this.Mini_Volume_TrackBar.ValueAdjusted = 100F;
            this.Mini_Volume_TrackBar.ValueDivisor = gTrackBar.gTrackBar.eValueDivisor.e1;
            this.Mini_Volume_TrackBar.ValueStrFormat = "";
            this.Mini_Volume_TrackBar.ValueChanged += new gTrackBar.gTrackBar.ValueChangedEventHandler(this.TrackVolume_ValueChanged);
            // 
            // LabelArtist
            // 
            this.Mini_Artist_Label.BackColor = System.Drawing.Color.Transparent;
            this.Mini_Artist_Label.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mini_Artist_Label.ForeColor = System.Drawing.Color.White;
            this.Mini_Artist_Label.Location = new System.Drawing.Point(28, 55);
            this.Mini_Artist_Label.Name = "LabelArtist";
            this.Mini_Artist_Label.Size = new System.Drawing.Size(188, 23);
            this.Mini_Artist_Label.TabIndex = 7;
            // 
            // LabelTitle
            // 
            this.Mini_Title_Label.BackColor = System.Drawing.Color.Transparent;
            this.Mini_Title_Label.Font = new System.Drawing.Font("Segoe UI Symbol", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mini_Title_Label.ForeColor = System.Drawing.Color.White;
            this.Mini_Title_Label.Location = new System.Drawing.Point(28, 81);
            this.Mini_Title_Label.Name = "LabelTitle";
            this.Mini_Title_Label.Size = new System.Drawing.Size(188, 23);
            this.Mini_Title_Label.TabIndex = 8;
            // 
            // LabelTime
            // 
            this.Mini_Time_Label.BackColor = System.Drawing.Color.Transparent;
            this.Mini_Time_Label.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mini_Time_Label.ForeColor = System.Drawing.Color.White;
            this.Mini_Time_Label.Location = new System.Drawing.Point(28, 109);
            this.Mini_Time_Label.Name = "LabelTime";
            this.Mini_Time_Label.Size = new System.Drawing.Size(180, 23);
            this.Mini_Time_Label.TabIndex = 9;
            this.Mini_Time_Label.Text = "00:00 | 00:00";
            // 
            // GUITimer
            // 
            this.Mini_GUITimer.Enabled = true;
            this.Mini_GUITimer.Interval = 500;
            this.Mini_GUITimer.Tick += new System.EventHandler(this.GUITimer_Tick);
            // 
            // imageButton5
            // 
            this.Mini_Pin.BackColor = System.Drawing.Color.Transparent;
            this.Mini_Pin.BackgroundImage = global::OSUplayer.Properties.Resources.Pin;
            this.Mini_Pin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Mini_Pin.BaseImage = global::OSUplayer.Properties.Resources.Pin;
            this.Mini_Pin.ClickImage = global::OSUplayer.Properties.Resources.Pined;
            this.Mini_Pin.EnterImage = global::OSUplayer.Properties.Resources.PinE;
            this.Mini_Pin.Location = new System.Drawing.Point(2, 1);
            this.Mini_Pin.Name = "imageButton5";
            this.Mini_Pin.Size = new System.Drawing.Size(46, 46);
            this.Mini_Pin.TabIndex = 4;
            this.Mini_Pin.Click += new System.EventHandler(this.imageButton5_Click);
            // 
            // imageButton4
            // 
            this.Mini_Main_Switcher.BackColor = System.Drawing.Color.Transparent;
            this.Mini_Main_Switcher.BackgroundImage = global::OSUplayer.Properties.Resources.Up;
            this.Mini_Main_Switcher.BaseImage = global::OSUplayer.Properties.Resources.Up;
            this.Mini_Main_Switcher.ClickImage = global::OSUplayer.Properties.Resources.UpC;
            this.Mini_Main_Switcher.EnterImage = global::OSUplayer.Properties.Resources.UpE;
            this.Mini_Main_Switcher.Location = new System.Drawing.Point(524, 55);
            this.Mini_Main_Switcher.Name = "imageButton4";
            this.Mini_Main_Switcher.Size = new System.Drawing.Size(84, 90);
            this.Mini_Main_Switcher.TabIndex = 3;
            this.Mini_Main_Switcher.Click += new System.EventHandler(this.imageButton4_Click);
            // 
            // imageButton3
            // 
            this.Mini_PlayNext.BackColor = System.Drawing.Color.Transparent;
            this.Mini_PlayNext.BackgroundImage = global::OSUplayer.Properties.Resources.Next;
            this.Mini_PlayNext.BaseImage = global::OSUplayer.Properties.Resources.Next;
            this.Mini_PlayNext.ClickImage = global::OSUplayer.Properties.Resources.NextC;
            this.Mini_PlayNext.EnterImage = global::OSUplayer.Properties.Resources.NextE;
            this.Mini_PlayNext.Location = new System.Drawing.Point(434, 55);
            this.Mini_PlayNext.Name = "imageButton3";
            this.Mini_PlayNext.Size = new System.Drawing.Size(84, 90);
            this.Mini_PlayNext.TabIndex = 2;
            this.Mini_PlayNext.Click += new System.EventHandler(this.imageButton3_Click);
            // 
            // StopButton
            // 
            this.Mini_Stop.BackColor = System.Drawing.Color.Transparent;
            this.Mini_Stop.BackgroundImage = global::OSUplayer.Properties.Resources.Stop;
            this.Mini_Stop.BaseImage = global::OSUplayer.Properties.Resources.Stop;
            this.Mini_Stop.ClickImage = global::OSUplayer.Properties.Resources.StopC;
            this.Mini_Stop.Enabled = false;
            this.Mini_Stop.EnterImage = global::OSUplayer.Properties.Resources.StopE;
            this.Mini_Stop.Location = new System.Drawing.Point(344, 55);
            this.Mini_Stop.Name = "StopButton";
            this.Mini_Stop.Size = new System.Drawing.Size(84, 90);
            this.Mini_Stop.TabIndex = 1;
            this.Mini_Stop.Click += new System.EventHandler(this.imageButton2_Click);
            // 
            // PlayButton
            // 
            this.Mini_Play.BackColor = System.Drawing.Color.Transparent;
            this.Mini_Play.BackgroundImage = global::OSUplayer.Properties.Resources.play;
            this.Mini_Play.BaseImage = global::OSUplayer.Properties.Resources.play;
            this.Mini_Play.ClickImage = global::OSUplayer.Properties.Resources.PlayC;
            this.Mini_Play.EnterImage = global::OSUplayer.Properties.Resources.PlayE;
            this.Mini_Play.Location = new System.Drawing.Point(234, 35);
            this.Mini_Play.Name = "PlayButton";
            this.Mini_Play.Size = new System.Drawing.Size(104, 110);
            this.Mini_Play.TabIndex = 0;
            this.Mini_Play.Click += new System.EventHandler(this.imageButton1_Click);
            // 
            // Mini
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(612, 209);
            this.Controls.Add(this.Mini_Time_Label);
            this.Controls.Add(this.Mini_Title_Label);
            this.Controls.Add(this.Mini_Artist_Label);
            this.Controls.Add(this.Mini_Volume_TrackBar);
            this.Controls.Add(this.Mini_Pin);
            this.Controls.Add(this.Mini_Main_Switcher);
            this.Controls.Add(this.Mini_PlayNext);
            this.Controls.Add(this.Mini_Stop);
            this.Controls.Add(this.Mini_Play);
            this.Controls.Add(this.Mini_Seek_TrackBar);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Mini";
            this.Text = "Mini";
            this.Load += new System.EventHandler(this.Mini_Load);
            this.Shown += new System.EventHandler(this.Mini_Shown);
            this.ResumeLayout(false);

        }
        #endregion

        private ImageButton Mini_Play;
        private System.Windows.Forms.Timer Mini_UpdateTimer;
        private ImageButton Mini_Stop;
        private ImageButton Mini_PlayNext;
        private ImageButton Mini_Main_Switcher;
        private ImageButton Mini_Pin;
        private gTrackBar.gTrackBar Mini_Seek_TrackBar;
        private gTrackBar.gTrackBar Mini_Volume_TrackBar;
        private System.Windows.Forms.Label Mini_Artist_Label;
        private System.Windows.Forms.Label Mini_Title_Label;
        private System.Windows.Forms.Label Mini_Time_Label;
        private System.Windows.Forms.Timer Mini_GUITimer;
    }
}