namespace OSUplayer.Uilties
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.About_Program_Link = new System.Windows.Forms.LinkLabel();
            this.About_Profile_Hint = new System.Windows.Forms.Label();
            this.About_PictureBox = new System.Windows.Forms.PictureBox();
            this.About_Content = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.About_PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // About_Program_Link
            // 
            this.About_Program_Link.AutoSize = true;
            this.About_Program_Link.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.About_Program_Link.Location = new System.Drawing.Point(219, 224);
            this.About_Program_Link.Name = "About_Program_Link";
            this.About_Program_Link.Size = new System.Drawing.Size(300, 20);
            this.About_Program_Link.TabIndex = 1;
            this.About_Program_Link.TabStop = true;
            this.About_Program_Link.Text = "https://github.com/Troogle/OSUplayer/";
            this.About_Program_Link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.About_Program_Link_LinkClicked);
            // 
            // About_Profile_Hint
            // 
            this.About_Profile_Hint.AutoSize = true;
            this.About_Profile_Hint.BackColor = System.Drawing.Color.Transparent;
            this.About_Profile_Hint.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.About_Profile_Hint.Location = new System.Drawing.Point(20, 209);
            this.About_Profile_Hint.Name = "About_Profile_Hint";
            this.About_Profile_Hint.Size = new System.Drawing.Size(185, 20);
            this.About_Profile_Hint.TabIndex = 2;
            this.About_Profile_Hint.Text = "(Click for osu! userpage)";
            // 
            // About_PictureBox
            // 
            this.About_PictureBox.Image = global::OSUplayer.Properties.Resources.Troogle;
            this.About_PictureBox.InitialImage = global::OSUplayer.Properties.Resources.Troogle;
            this.About_PictureBox.Location = new System.Drawing.Point(12, 12);
            this.About_PictureBox.Name = "About_PictureBox";
            this.About_PictureBox.Size = new System.Drawing.Size(200, 200);
            this.About_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.About_PictureBox.TabIndex = 0;
            this.About_PictureBox.TabStop = false;
            this.About_PictureBox.Click += new System.EventHandler(this.About_PictureBox_Click);
            // 
            // About_Content
            // 
            this.About_Content.Font = new System.Drawing.Font("微软雅黑", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.About_Content.Location = new System.Drawing.Point(216, 4);
            this.About_Content.Name = "About_Content";
            this.About_Content.Size = new System.Drawing.Size(300, 208);
            this.About_Content.TabIndex = 3;
            this.About_Content.Text = "OSU Player Ver {0}\r\nDeveloped By Troogle\r\n\r\nThanks for bugs fixing and develpoing" +
    ":\r\nchrisyan,JiXun\r\n\r\nSpecial Thanks:\r\nWeiren,Muscipular\r\n\r\nThx for using~";
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 253);
            this.Controls.Add(this.About_Content);
            this.Controls.Add(this.About_Profile_Hint);
            this.Controls.Add(this.About_Program_Link);
            this.Controls.Add(this.About_PictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "About";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "About";
            this.Load += new System.EventHandler(this.About_Load);
            ((System.ComponentModel.ISupportInitialize)(this.About_PictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.PictureBox About_PictureBox;
        private System.Windows.Forms.LinkLabel About_Program_Link;
        private System.Windows.Forms.Label About_Profile_Hint;
        private System.Windows.Forms.Label About_Content;
    }
}