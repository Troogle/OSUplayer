namespace OSUplayer.Uilties
{
    partial class DelDulp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DelDulp));
            this.DelDulp_MainView = new System.Windows.Forms.ListView();
            this.DelDulp_MainView_Header1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DelDulp_MainView_Header2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DelDulp_DeleteSelected = new Telerik.WinControls.UI.RadButton();
            this.DelDulp_Cancel = new Telerik.WinControls.UI.RadButton();
            this.DelDulp_StartSearch = new Telerik.WinControls.UI.RadButton();
            this.BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.BackgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.BackgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.DelDulp_AutoSelect = new Telerik.WinControls.UI.RadButton();
            this.DelDulp_ClearSelected = new Telerik.WinControls.UI.RadButton();
            this.DelDulp_StatusStrip = new Telerik.WinControls.UI.RadStatusStrip();
            this.DelDulp_ProgressBar = new Telerik.WinControls.UI.RadProgressBarElement();
            this.DelDulp_Status_Label = new Telerik.WinControls.UI.RadLabelElement();
            ((System.ComponentModel.ISupportInitialize)(this.DelDulp_DeleteSelected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelDulp_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelDulp_StartSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelDulp_AutoSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelDulp_ClearSelected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelDulp_StatusStrip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.DelDulp_MainView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.DelDulp_MainView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DelDulp_MainView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DelDulp_MainView.CheckBoxes = true;
            this.DelDulp_MainView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DelDulp_MainView_Header1,
            this.DelDulp_MainView_Header2});
            this.DelDulp_MainView.FullRowSelect = true;
            this.DelDulp_MainView.GridLines = true;
            this.DelDulp_MainView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.DelDulp_MainView.HideSelection = false;
            this.DelDulp_MainView.Location = new System.Drawing.Point(12, 12);
            this.DelDulp_MainView.Name = "listView1";
            this.DelDulp_MainView.ShowGroups = false;
            this.DelDulp_MainView.Size = new System.Drawing.Size(433, 313);
            this.DelDulp_MainView.TabIndex = 0;
            this.DelDulp_MainView.UseCompatibleStateImageBehavior = false;
            this.DelDulp_MainView.View = System.Windows.Forms.View.Details;
            this.DelDulp_MainView.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.DelDulp_MainView.SizeChanged += new System.EventHandler(this.listView1_SizeChanged);
            // 
            // columnHeader1
            // 
            this.DelDulp_MainView_Header1.Text = "";
            this.DelDulp_MainView_Header1.Width = 400;
            // 
            // columnHeader2
            // 
            this.DelDulp_MainView_Header2.Text = "md5";
            this.DelDulp_MainView_Header2.Width = 0;
            // 
            // button1
            // 
            this.DelDulp_DeleteSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DelDulp_DeleteSelected.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.DelDulp_DeleteSelected.Enabled = false;
            this.DelDulp_DeleteSelected.Location = new System.Drawing.Point(451, 206);
            this.DelDulp_DeleteSelected.Name = "button1";
            this.DelDulp_DeleteSelected.Size = new System.Drawing.Size(75, 26);
            this.DelDulp_DeleteSelected.TabIndex = 1;
            this.DelDulp_DeleteSelected.Text = "删除选中";
            this.DelDulp_DeleteSelected.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.DelDulp_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DelDulp_Cancel.Location = new System.Drawing.Point(451, 267);
            this.DelDulp_Cancel.Name = "button2";
            this.DelDulp_Cancel.Size = new System.Drawing.Size(75, 27);
            this.DelDulp_Cancel.TabIndex = 2;
            this.DelDulp_Cancel.Text = "取消";
            this.DelDulp_Cancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.DelDulp_StartSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DelDulp_StartSearch.Location = new System.Drawing.Point(451, 39);
            this.DelDulp_StartSearch.Name = "button3";
            this.DelDulp_StartSearch.Size = new System.Drawing.Size(75, 31);
            this.DelDulp_StartSearch.TabIndex = 3;
            this.DelDulp_StartSearch.Text = "开始查找";
            this.DelDulp_StartSearch.Click += new System.EventHandler(this.button3_Click);
            // 
            // BackgroundWorker1
            // 
            this.BackgroundWorker1.WorkerReportsProgress = true;
            this.BackgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1DoWork);
            this.BackgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker1ProgressChanged);
            this.BackgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker1RunWorkerCompleted);
            // 
            // BackgroundWorker2
            // 
            this.BackgroundWorker2.WorkerReportsProgress = true;
            this.BackgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker2DoWork);
            this.BackgroundWorker2.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker2ProgressChanged);
            this.BackgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker2RunWorkerCompleted);
            // 
            // BackgroundWorker3
            // 
            this.BackgroundWorker3.WorkerReportsProgress = true;
            this.BackgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker3DoWork);
            this.BackgroundWorker3.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker3ProgressChanged);
            this.BackgroundWorker3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker3RunWorkerCompleted);
            // 
            // button4
            // 
            this.DelDulp_AutoSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DelDulp_AutoSelect.Enabled = false;
            this.DelDulp_AutoSelect.Location = new System.Drawing.Point(451, 99);
            this.DelDulp_AutoSelect.Name = "button4";
            this.DelDulp_AutoSelect.Size = new System.Drawing.Size(75, 26);
            this.DelDulp_AutoSelect.TabIndex = 5;
            this.DelDulp_AutoSelect.Text = "自动选择";
            this.DelDulp_AutoSelect.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.DelDulp_ClearSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DelDulp_ClearSelected.Enabled = false;
            this.DelDulp_ClearSelected.Location = new System.Drawing.Point(451, 151);
            this.DelDulp_ClearSelected.Name = "button5";
            this.DelDulp_ClearSelected.Size = new System.Drawing.Size(75, 26);
            this.DelDulp_ClearSelected.TabIndex = 6;
            this.DelDulp_ClearSelected.Text = "清除选中";
            this.DelDulp_ClearSelected.Click += new System.EventHandler(this.button5_Click);
            // 
            // radStatusStrip1
            // 
            this.DelDulp_StatusStrip.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.DelDulp_ProgressBar,
            this.DelDulp_Status_Label});
            this.DelDulp_StatusStrip.Location = new System.Drawing.Point(0, 329);
            this.DelDulp_StatusStrip.Name = "radStatusStrip1";
            this.DelDulp_StatusStrip.Size = new System.Drawing.Size(538, 28);
            this.DelDulp_StatusStrip.TabIndex = 7;
            this.DelDulp_StatusStrip.Text = "radStatusStrip1";
            // 
            // progressBar1
            // 
            this.DelDulp_ProgressBar.AutoSize = false;
            this.DelDulp_ProgressBar.Bounds = new System.Drawing.Rectangle(0, 0, 300, 22);
            this.DelDulp_ProgressBar.Name = "progressBar1";
            this.DelDulp_ProgressBar.SeparatorColor1 = System.Drawing.Color.White;
            this.DelDulp_ProgressBar.SeparatorColor2 = System.Drawing.Color.White;
            this.DelDulp_ProgressBar.SeparatorColor3 = System.Drawing.Color.White;
            this.DelDulp_ProgressBar.SeparatorColor4 = System.Drawing.Color.White;
            this.DelDulp_ProgressBar.SeparatorGradientAngle = 0;
            this.DelDulp_ProgressBar.SeparatorGradientPercentage1 = 0.4F;
            this.DelDulp_ProgressBar.SeparatorGradientPercentage2 = 0.6F;
            this.DelDulp_ProgressBar.SeparatorNumberOfColors = 2;
            this.DelDulp_StatusStrip.SetSpring(this.DelDulp_ProgressBar, false);
            this.DelDulp_ProgressBar.StepWidth = 14;
            this.DelDulp_ProgressBar.SweepAngle = 90;
            this.DelDulp_ProgressBar.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // Label1
            // 
            this.DelDulp_Status_Label.Alignment = System.Drawing.ContentAlignment.TopLeft;
            this.DelDulp_Status_Label.Name = "Label1";
            this.DelDulp_StatusStrip.SetSpring(this.DelDulp_Status_Label, false);
            this.DelDulp_Status_Label.Text = "";
            this.DelDulp_Status_Label.TextWrap = true;
            this.DelDulp_Status_Label.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // DelDulp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 357);
            this.Controls.Add(this.DelDulp_StatusStrip);
            this.Controls.Add(this.DelDulp_ClearSelected);
            this.Controls.Add(this.DelDulp_AutoSelect);
            this.Controls.Add(this.DelDulp_StartSearch);
            this.Controls.Add(this.DelDulp_Cancel);
            this.Controls.Add(this.DelDulp_DeleteSelected);
            this.Controls.Add(this.DelDulp_MainView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(546, 387);
            this.Name = "DelDulp";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "DelDulp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DelDulp_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.DelDulp_DeleteSelected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelDulp_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelDulp_StartSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelDulp_AutoSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelDulp_ClearSelected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelDulp_StatusStrip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.ListView DelDulp_MainView;
        private Telerik.WinControls.UI.RadButton DelDulp_DeleteSelected;
        private Telerik.WinControls.UI.RadButton DelDulp_Cancel;
        private Telerik.WinControls.UI.RadButton DelDulp_StartSearch;
        private System.ComponentModel.BackgroundWorker BackgroundWorker1;
        private System.ComponentModel.BackgroundWorker BackgroundWorker2;
        private System.ComponentModel.BackgroundWorker BackgroundWorker3;
        private System.Windows.Forms.ColumnHeader DelDulp_MainView_Header1;
        private Telerik.WinControls.UI.RadButton DelDulp_AutoSelect;
        private System.Windows.Forms.ColumnHeader DelDulp_MainView_Header2;
        private Telerik.WinControls.UI.RadButton DelDulp_ClearSelected;
        private Telerik.WinControls.UI.RadStatusStrip DelDulp_StatusStrip;
        private Telerik.WinControls.UI.RadProgressBarElement DelDulp_ProgressBar;
        private Telerik.WinControls.UI.RadLabelElement DelDulp_Status_Label;
    }
}