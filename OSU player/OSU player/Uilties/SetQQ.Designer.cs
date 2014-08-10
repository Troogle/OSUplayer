namespace OSUplayer.Uilties
{
    partial class SetQQ
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
            this.SetQQ_MainView = new System.Windows.Forms.ListView();
            this.SetQQ_MainView_ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SetQQ_MainView_Nickname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SetQQ_GetQQ = new Telerik.WinControls.UI.RadButton();
            this.SetQQ_OK = new Telerik.WinControls.UI.RadButton();
            this.SetQQ_Manual = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.SetQQ_GetQQ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SetQQ_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // SetQQ_MainView
            // 
            this.SetQQ_MainView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SetQQ_MainView_ID,
            this.SetQQ_MainView_Nickname});
            this.SetQQ_MainView.FullRowSelect = true;
            this.SetQQ_MainView.GridLines = true;
            this.SetQQ_MainView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.SetQQ_MainView.HideSelection = false;
            this.SetQQ_MainView.Location = new System.Drawing.Point(12, 12);
            this.SetQQ_MainView.MultiSelect = false;
            this.SetQQ_MainView.Name = "SetQQ_MainView";
            this.SetQQ_MainView.Size = new System.Drawing.Size(230, 193);
            this.SetQQ_MainView.TabIndex = 1;
            this.SetQQ_MainView.UseCompatibleStateImageBehavior = false;
            this.SetQQ_MainView.View = System.Windows.Forms.View.Details;
            // 
            // SetQQ_MainView_ID
            // 
            this.SetQQ_MainView_ID.Text = "ID";
            this.SetQQ_MainView_ID.Width = 100;
            // 
            // SetQQ_MainView_Nickname
            // 
            this.SetQQ_MainView_Nickname.Text = "昵称";
            this.SetQQ_MainView_Nickname.Width = 100;
            // 
            // SetQQ_GetQQ
            // 
            this.SetQQ_GetQQ.Location = new System.Drawing.Point(248, 52);
            this.SetQQ_GetQQ.Name = "SetQQ_GetQQ";
            this.SetQQ_GetQQ.Size = new System.Drawing.Size(97, 30);
            this.SetQQ_GetQQ.TabIndex = 2;
            this.SetQQ_GetQQ.Text = "手动获取";
            this.SetQQ_GetQQ.Click += new System.EventHandler(this.SetQQ_GetQQ_Click);
            // 
            // SetQQ_OK
            // 
            this.SetQQ_OK.Location = new System.Drawing.Point(248, 122);
            this.SetQQ_OK.Name = "SetQQ_OK";
            this.SetQQ_OK.Size = new System.Drawing.Size(97, 27);
            this.SetQQ_OK.TabIndex = 3;
            this.SetQQ_OK.Text = "确定";
            this.SetQQ_OK.Click += new System.EventHandler(this.SetQQ_OK_Click);
            // 
            // SetQQ_Manual
            // 
            this.SetQQ_Manual.Location = new System.Drawing.Point(12, 86);
            this.SetQQ_Manual.Name = "SetQQ_Manual";
            this.SetQQ_Manual.Size = new System.Drawing.Size(230, 25);
            this.SetQQ_Manual.TabIndex = 4;
            this.SetQQ_Manual.Visible = false;
            // 
            // SetQQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 217);
            this.Controls.Add(this.SetQQ_Manual);
            this.Controls.Add(this.SetQQ_OK);
            this.Controls.Add(this.SetQQ_GetQQ);
            this.Controls.Add(this.SetQQ_MainView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SetQQ";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "QQ";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SetQQ_GetQQ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SetQQ_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion
        private System.Windows.Forms.ListView SetQQ_MainView;
		private Telerik.WinControls.UI.RadButton SetQQ_GetQQ;
		private Telerik.WinControls.UI.RadButton SetQQ_OK;
		private System.Windows.Forms.ColumnHeader SetQQ_MainView_Nickname;
        private System.Windows.Forms.ColumnHeader SetQQ_MainView_ID;
        private System.Windows.Forms.TextBox SetQQ_Manual;
	}
}
