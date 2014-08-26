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
            this.DelDulp_DeleteSelected = new System.Windows.Forms.Button();
            this.DelDulp_Cancel = new System.Windows.Forms.Button();
            this.DelDulp_StartSearch = new System.Windows.Forms.Button();
            this.BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.BackgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.BackgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.DelDulp_AutoSelect = new System.Windows.Forms.Button();
            this.DelDulp_ClearSelected = new System.Windows.Forms.Button();
            this.DelDulp_StatusStrip = new System.Windows.Forms.StatusStrip();
            this.DelDulp_ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.DelDulp_Status_Label = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.DelDulp_StatusStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DelDulp_MainView
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
            this.DelDulp_MainView.Location = new System.Drawing.Point(0, 0);
            this.DelDulp_MainView.Margin = new System.Windows.Forms.Padding(0);
            this.DelDulp_MainView.Name = "DelDulp_MainView";
            this.tableLayoutPanel1.SetRowSpan(this.DelDulp_MainView, 5);
            this.DelDulp_MainView.ShowGroups = false;
            this.DelDulp_MainView.Size = new System.Drawing.Size(407, 318);
            this.DelDulp_MainView.TabIndex = 0;
            this.DelDulp_MainView.UseCompatibleStateImageBehavior = false;
            this.DelDulp_MainView.View = System.Windows.Forms.View.Details;
            this.DelDulp_MainView.SelectedIndexChanged += new System.EventHandler(this.DelDulp_MainView_SelectedIndexChanged);
            this.DelDulp_MainView.SizeChanged += new System.EventHandler(this.DelDulp_MainView_SizeChanged);
            // 
            // DelDulp_MainView_Header1
            // 
            this.DelDulp_MainView_Header1.Text = "";
            this.DelDulp_MainView_Header1.Width = 400;
            // 
            // DelDulp_MainView_Header2
            // 
            this.DelDulp_MainView_Header2.Text = "md5";
            this.DelDulp_MainView_Header2.Width = 0;
            // 
            // DelDulp_DeleteSelected
            // 
            this.DelDulp_DeleteSelected.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DelDulp_DeleteSelected.AutoSize = true;
            this.DelDulp_DeleteSelected.Enabled = false;
            this.DelDulp_DeleteSelected.Location = new System.Drawing.Point(456, 208);
            this.DelDulp_DeleteSelected.Name = "DelDulp_DeleteSelected";
            this.DelDulp_DeleteSelected.Size = new System.Drawing.Size(77, 25);
            this.DelDulp_DeleteSelected.TabIndex = 1;
            this.DelDulp_DeleteSelected.Text = "删除选中";
            this.DelDulp_DeleteSelected.UseVisualStyleBackColor = false;
            this.DelDulp_DeleteSelected.Click += new System.EventHandler(this.DelDulp_DeleteSelected_Click);
            // 
            // DelDulp_Cancel
            // 
            this.DelDulp_Cancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DelDulp_Cancel.AutoSize = true;
            this.DelDulp_Cancel.Location = new System.Drawing.Point(457, 272);
            this.DelDulp_Cancel.Name = "DelDulp_Cancel";
            this.DelDulp_Cancel.Size = new System.Drawing.Size(75, 25);
            this.DelDulp_Cancel.TabIndex = 2;
            this.DelDulp_Cancel.Text = "取消";
            this.DelDulp_Cancel.Click += new System.EventHandler(this.DelDulp_Cancel_Click);
            // 
            // DelDulp_StartSearch
            // 
            this.DelDulp_StartSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DelDulp_StartSearch.AutoSize = true;
            this.DelDulp_StartSearch.Location = new System.Drawing.Point(456, 19);
            this.DelDulp_StartSearch.Name = "DelDulp_StartSearch";
            this.DelDulp_StartSearch.Size = new System.Drawing.Size(77, 25);
            this.DelDulp_StartSearch.TabIndex = 3;
            this.DelDulp_StartSearch.Text = "开始查找";
            this.DelDulp_StartSearch.Click += new System.EventHandler(this.DelDulp_StartSearch_Click);
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
            // DelDulp_AutoSelect
            // 
            this.DelDulp_AutoSelect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DelDulp_AutoSelect.AutoSize = true;
            this.DelDulp_AutoSelect.Enabled = false;
            this.DelDulp_AutoSelect.Location = new System.Drawing.Point(456, 82);
            this.DelDulp_AutoSelect.Name = "DelDulp_AutoSelect";
            this.DelDulp_AutoSelect.Size = new System.Drawing.Size(77, 25);
            this.DelDulp_AutoSelect.TabIndex = 5;
            this.DelDulp_AutoSelect.Text = "自动选择";
            this.DelDulp_AutoSelect.Click += new System.EventHandler(this.DelDulp_AutoSelect_Click);
            // 
            // DelDulp_ClearSelected
            // 
            this.DelDulp_ClearSelected.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DelDulp_ClearSelected.AutoSize = true;
            this.DelDulp_ClearSelected.Enabled = false;
            this.DelDulp_ClearSelected.Location = new System.Drawing.Point(456, 145);
            this.DelDulp_ClearSelected.Name = "DelDulp_ClearSelected";
            this.DelDulp_ClearSelected.Size = new System.Drawing.Size(77, 25);
            this.DelDulp_ClearSelected.TabIndex = 6;
            this.DelDulp_ClearSelected.Text = "清除选中";
            this.DelDulp_ClearSelected.Click += new System.EventHandler(this.DelDulp_ClearSelected_Click);
            // 
            // DelDulp_StatusStrip
            // 
            this.DelDulp_StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DelDulp_ProgressBar,
            this.DelDulp_Status_Label});
            this.DelDulp_StatusStrip.Location = new System.Drawing.Point(0, 318);
            this.DelDulp_StatusStrip.Name = "DelDulp_StatusStrip";
            this.DelDulp_StatusStrip.Size = new System.Drawing.Size(582, 25);
            this.DelDulp_StatusStrip.TabIndex = 7;
            this.DelDulp_StatusStrip.Text = "statusStrip1";
            // 
            // DelDulp_ProgressBar
            // 
            this.DelDulp_ProgressBar.Name = "DelDulp_ProgressBar";
            this.DelDulp_ProgressBar.Size = new System.Drawing.Size(300, 19);
            // 
            // DelDulp_Status_Label
            // 
            this.DelDulp_Status_Label.Name = "DelDulp_Status_Label";
            this.DelDulp_Status_Label.Size = new System.Drawing.Size(0, 20);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.DelDulp_AutoSelect, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.DelDulp_Cancel, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.DelDulp_ClearSelected, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.DelDulp_DeleteSelected, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.DelDulp_MainView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.DelDulp_StartSearch, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(582, 318);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // DelDulp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 343);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.DelDulp_StatusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 390);
            this.Name = "DelDulp";
            this.Text = "DelDulp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DelDulp_FormClosing);
            this.DelDulp_StatusStrip.ResumeLayout(false);
            this.DelDulp_StatusStrip.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.ListView DelDulp_MainView;
        private System.Windows.Forms.Button DelDulp_DeleteSelected;
        private System.Windows.Forms.Button DelDulp_Cancel;
        private System.Windows.Forms.Button DelDulp_StartSearch;
        private System.ComponentModel.BackgroundWorker BackgroundWorker1;
        private System.ComponentModel.BackgroundWorker BackgroundWorker2;
        private System.ComponentModel.BackgroundWorker BackgroundWorker3;
        private System.Windows.Forms.ColumnHeader DelDulp_MainView_Header1;
        private System.Windows.Forms.Button DelDulp_AutoSelect;
        private System.Windows.Forms.ColumnHeader DelDulp_MainView_Header2;
        private System.Windows.Forms.Button DelDulp_ClearSelected;
        private System.Windows.Forms.StatusStrip DelDulp_StatusStrip;
        private System.Windows.Forms.ToolStripProgressBar DelDulp_ProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel DelDulp_Status_Label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}