namespace OSU_player
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
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.TrackSeek = new gTrackBar.gTrackBar();
            this.TrackVolume = new gTrackBar.gTrackBar();
            this.LabelArtist = new System.Windows.Forms.Label();
            this.LabelTitle = new System.Windows.Forms.Label();
            this.LabelTime = new System.Windows.Forms.Label();
            this.GUITimer = new System.Windows.Forms.Timer(this.components);
            this.imageButton5 = new OSU_player.ImageButton();
            this.imageButton4 = new OSU_player.ImageButton();
            this.imageButton3 = new OSU_player.ImageButton();
            this.StopButton = new OSU_player.ImageButton();
            this.PlayButton = new OSU_player.ImageButton();
            this.SuspendLayout();
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // TrackSeek
            // 
            this.TrackSeek.BackColor = System.Drawing.Color.Transparent;
            this.TrackSeek.BrushStyle = gTrackBar.gTrackBar.eBrushStyle.Image;
            this.TrackSeek.Enabled = false;
            this.TrackSeek.FloatValue = false;
            this.TrackSeek.FloatValueFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(82)))), ((int)(((byte)(92)))));
            this.TrackSeek.Label = null;
            this.TrackSeek.Location = new System.Drawing.Point(12, 151);
            this.TrackSeek.MaxValue = 1000;
            this.TrackSeek.Name = "TrackSeek";
            this.TrackSeek.ShowFocus = false;
            this.TrackSeek.Size = new System.Drawing.Size(588, 50);
            colorLinearGradient1.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(167)))), ((int)(((byte)(170)))));
            colorLinearGradient1.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(167)))), ((int)(((byte)(170)))));
            this.TrackSeek.SliderColorHigh = colorLinearGradient1;
            colorLinearGradient2.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(82)))), ((int)(((byte)(92)))));
            colorLinearGradient2.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(82)))), ((int)(((byte)(92)))));
            this.TrackSeek.SliderColorLow = colorLinearGradient2;
            this.TrackSeek.SliderImage = global::OSU_player.Properties.Resources.track;
            this.TrackSeek.SliderSize = new System.Drawing.Size(38, 48);
            this.TrackSeek.SliderWidthHigh = 10F;
            this.TrackSeek.SliderWidthLow = 10F;
            this.TrackSeek.TabIndex = 5;
            this.TrackSeek.TickThickness = 1F;
            this.TrackSeek.UpDownShow = false;
            this.TrackSeek.Value = 0;
            this.TrackSeek.ValueAdjusted = 0F;
            this.TrackSeek.ValueDivisor = gTrackBar.gTrackBar.eValueDivisor.e1;
            this.TrackSeek.ValueStrFormat = "";
            this.TrackSeek.ValueChanged += new gTrackBar.gTrackBar.ValueChangedEventHandler(this.TrackSeek_ValueChanged);
            this.TrackSeek.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TrackSeek_MouseDown);
            this.TrackSeek.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrackSeek_MouseUp);
            // 
            // TrackVolume
            // 
            this.TrackVolume.BackColor = System.Drawing.Color.Transparent;
            this.TrackVolume.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.TrackVolume.BrushStyle = gTrackBar.gTrackBar.eBrushStyle.Image;
            this.TrackVolume.FloatValue = false;
            this.TrackVolume.FloatValueFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(82)))), ((int)(((byte)(92)))));
            this.TrackVolume.Label = null;
            this.TrackVolume.Location = new System.Drawing.Point(304, 1);
            this.TrackVolume.MaxValue = 100;
            this.TrackVolume.Name = "TrackVolume";
            this.TrackVolume.ShowFocus = false;
            this.TrackVolume.Size = new System.Drawing.Size(304, 37);
            colorLinearGradient3.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(167)))), ((int)(((byte)(170)))));
            colorLinearGradient3.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(167)))), ((int)(((byte)(170)))));
            this.TrackVolume.SliderColorHigh = colorLinearGradient3;
            colorLinearGradient4.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(82)))), ((int)(((byte)(92)))));
            colorLinearGradient4.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(82)))), ((int)(((byte)(92)))));
            this.TrackVolume.SliderColorLow = colorLinearGradient4;
            this.TrackVolume.SliderImage = global::OSU_player.Properties.Resources.VolumeHandle;
            this.TrackVolume.SliderSize = new System.Drawing.Size(27, 43);
            this.TrackVolume.SliderWidthHigh = 10F;
            this.TrackVolume.SliderWidthLow = 10F;
            this.TrackVolume.TabIndex = 6;
            this.TrackVolume.TickThickness = 1F;
            this.TrackVolume.UpDownShow = false;
            this.TrackVolume.Value = 100;
            this.TrackVolume.ValueAdjusted = 100F;
            this.TrackVolume.ValueDivisor = gTrackBar.gTrackBar.eValueDivisor.e1;
            this.TrackVolume.ValueStrFormat = "";
            this.TrackVolume.ValueChanged += new gTrackBar.gTrackBar.ValueChangedEventHandler(this.TrackVolume_ValueChanged);
            // 
            // LabelArtist
            // 
            this.LabelArtist.BackColor = System.Drawing.Color.Transparent;
            this.LabelArtist.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelArtist.ForeColor = System.Drawing.Color.White;
            this.LabelArtist.Location = new System.Drawing.Point(28, 55);
            this.LabelArtist.Name = "LabelArtist";
            this.LabelArtist.Size = new System.Drawing.Size(188, 23);
            this.LabelArtist.TabIndex = 7;
            // 
            // LabelTitle
            // 
            this.LabelTitle.BackColor = System.Drawing.Color.Transparent;
            this.LabelTitle.Font = new System.Drawing.Font("Segoe UI Symbol", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTitle.ForeColor = System.Drawing.Color.White;
            this.LabelTitle.Location = new System.Drawing.Point(28, 81);
            this.LabelTitle.Name = "LabelTitle";
            this.LabelTitle.Size = new System.Drawing.Size(188, 23);
            this.LabelTitle.TabIndex = 8;
            // 
            // LabelTime
            // 
            this.LabelTime.BackColor = System.Drawing.Color.Transparent;
            this.LabelTime.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTime.ForeColor = System.Drawing.Color.White;
            this.LabelTime.Location = new System.Drawing.Point(28, 109);
            this.LabelTime.Name = "LabelTime";
            this.LabelTime.Size = new System.Drawing.Size(180, 23);
            this.LabelTime.TabIndex = 9;
            this.LabelTime.Text = "00:00 | 00:00";
            // 
            // GUITimer
            // 
            this.GUITimer.Enabled = true;
            this.GUITimer.Interval = 500;
            this.GUITimer.Tick += new System.EventHandler(this.GUITimer_Tick);
            // 
            // imageButton5
            // 
            this.imageButton5.BackColor = System.Drawing.Color.Transparent;
            this.imageButton5.BackgroundImage = global::OSU_player.Properties.Resources.Pin;
            this.imageButton5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.imageButton5.BaseImage = global::OSU_player.Properties.Resources.Pin;
            this.imageButton5.ClickImage = global::OSU_player.Properties.Resources.Pined;
            this.imageButton5.EnterImage = global::OSU_player.Properties.Resources.PinE;
            this.imageButton5.Location = new System.Drawing.Point(2, 1);
            this.imageButton5.Name = "imageButton5";
            this.imageButton5.Size = new System.Drawing.Size(46, 46);
            this.imageButton5.TabIndex = 4;
            this.imageButton5.Click += new System.EventHandler(this.imageButton5_Click);
            // 
            // imageButton4
            // 
            this.imageButton4.BackColor = System.Drawing.Color.Transparent;
            this.imageButton4.BackgroundImage = global::OSU_player.Properties.Resources.Up;
            this.imageButton4.BaseImage = global::OSU_player.Properties.Resources.Up;
            this.imageButton4.ClickImage = global::OSU_player.Properties.Resources.UpC;
            this.imageButton4.EnterImage = global::OSU_player.Properties.Resources.UpE;
            this.imageButton4.Location = new System.Drawing.Point(524, 55);
            this.imageButton4.Name = "imageButton4";
            this.imageButton4.Size = new System.Drawing.Size(84, 90);
            this.imageButton4.TabIndex = 3;
            this.imageButton4.Click += new System.EventHandler(this.imageButton4_Click);
            // 
            // imageButton3
            // 
            this.imageButton3.BackColor = System.Drawing.Color.Transparent;
            this.imageButton3.BackgroundImage = global::OSU_player.Properties.Resources.Next;
            this.imageButton3.BaseImage = global::OSU_player.Properties.Resources.Next;
            this.imageButton3.ClickImage = global::OSU_player.Properties.Resources.NextC;
            this.imageButton3.EnterImage = global::OSU_player.Properties.Resources.NextE;
            this.imageButton3.Location = new System.Drawing.Point(434, 55);
            this.imageButton3.Name = "imageButton3";
            this.imageButton3.Size = new System.Drawing.Size(84, 90);
            this.imageButton3.TabIndex = 2;
            this.imageButton3.Click += new System.EventHandler(this.imageButton3_Click);
            // 
            // StopButton
            // 
            this.StopButton.BackColor = System.Drawing.Color.Transparent;
            this.StopButton.BackgroundImage = global::OSU_player.Properties.Resources.Stop;
            this.StopButton.BaseImage = global::OSU_player.Properties.Resources.Stop;
            this.StopButton.ClickImage = global::OSU_player.Properties.Resources.StopC;
            this.StopButton.Enabled = false;
            this.StopButton.EnterImage = global::OSU_player.Properties.Resources.StopE;
            this.StopButton.Location = new System.Drawing.Point(344, 55);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(84, 90);
            this.StopButton.TabIndex = 1;
            this.StopButton.Click += new System.EventHandler(this.imageButton2_Click);
            // 
            // PlayButton
            // 
            this.PlayButton.BackColor = System.Drawing.Color.Transparent;
            this.PlayButton.BackgroundImage = global::OSU_player.Properties.Resources.play;
            this.PlayButton.BaseImage = global::OSU_player.Properties.Resources.play;
            this.PlayButton.ClickImage = global::OSU_player.Properties.Resources.PlayC;
            this.PlayButton.EnterImage = global::OSU_player.Properties.Resources.PlayE;
            this.PlayButton.Location = new System.Drawing.Point(234, 35);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(104, 110);
            this.PlayButton.TabIndex = 0;
            this.PlayButton.Click += new System.EventHandler(this.imageButton1_Click);
            // 
            // Mini
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(612, 209);
            this.Controls.Add(this.LabelTime);
            this.Controls.Add(this.LabelTitle);
            this.Controls.Add(this.LabelArtist);
            this.Controls.Add(this.TrackVolume);
            this.Controls.Add(this.imageButton5);
            this.Controls.Add(this.imageButton4);
            this.Controls.Add(this.imageButton3);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.TrackSeek);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Mini";
            this.Text = "Mini";
            this.Load += new System.EventHandler(this.Mini_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private ImageButton PlayButton;
        private System.Windows.Forms.Timer UpdateTimer;
        private ImageButton StopButton;
        private ImageButton imageButton3;
        private ImageButton imageButton4;
        private ImageButton imageButton5;
        private gTrackBar.gTrackBar TrackSeek;
        private gTrackBar.gTrackBar TrackVolume;
        private System.Windows.Forms.Label LabelArtist;
        private System.Windows.Forms.Label LabelTitle;
        private System.Windows.Forms.Label LabelTime;
        private System.Windows.Forms.Timer GUITimer;
    }
}