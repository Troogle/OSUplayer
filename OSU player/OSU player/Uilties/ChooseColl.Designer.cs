namespace OSUplayer.Uilties
{
    partial class ChooseColl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseColl));
            this.ChooseColl_CollectionTitle_List = new System.Windows.Forms.ListBox();
            this.ChooseColl_CollectionContent_List = new System.Windows.Forms.ListBox();
            this.ChooseColl_GetCollections = new Telerik.WinControls.UI.RadButton();
            this.ChooseColl_OK = new Telerik.WinControls.UI.RadButton();
            this.ChooseColl_Hint_Label = new System.Windows.Forms.Label();
            this.ChooseColl_ClearPlayList = new Telerik.WinControls.UI.RadButton();
            this.ChooseColl_PlayListCurrentCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ChooseColl_GetCollections)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChooseColl_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChooseColl_ClearPlayList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // ChooseColl_CollectionTitle_List
            // 
            this.ChooseColl_CollectionTitle_List.FormattingEnabled = true;
            this.ChooseColl_CollectionTitle_List.ItemHeight = 15;
            this.ChooseColl_CollectionTitle_List.Location = new System.Drawing.Point(12, 36);
            this.ChooseColl_CollectionTitle_List.Name = "ChooseColl_CollectionTitle_List";
            this.ChooseColl_CollectionTitle_List.Size = new System.Drawing.Size(90, 259);
            this.ChooseColl_CollectionTitle_List.TabIndex = 0;
            this.ChooseColl_CollectionTitle_List.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.ChooseColl_CollectionTitle_List.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ChooseColl_CollectionTitle_List_MouseDoubleClick);
            // 
            // ChooseColl_CollectionContent_List
            // 
            this.ChooseColl_CollectionContent_List.FormattingEnabled = true;
            this.ChooseColl_CollectionContent_List.ItemHeight = 15;
            this.ChooseColl_CollectionContent_List.Location = new System.Drawing.Point(108, 36);
            this.ChooseColl_CollectionContent_List.Name = "ChooseColl_CollectionContent_List";
            this.ChooseColl_CollectionContent_List.Size = new System.Drawing.Size(366, 259);
            this.ChooseColl_CollectionContent_List.TabIndex = 1;
            this.ChooseColl_CollectionContent_List.SelectedIndexChanged += new System.EventHandler(this.ChooseColl_CollectionContent_List_SelectedIndexChanged);
            this.ChooseColl_CollectionContent_List.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ChooseColl_CollectionContent_List_MouseDoubleClick);
            // 
            // ChooseColl_GetCollections
            // 
            this.ChooseColl_GetCollections.Location = new System.Drawing.Point(483, 78);
            this.ChooseColl_GetCollections.Name = "ChooseColl_GetCollections";
            this.ChooseColl_GetCollections.Size = new System.Drawing.Size(81, 29);
            this.ChooseColl_GetCollections.TabIndex = 2;
            this.ChooseColl_GetCollections.Text = "扫描";
            this.ChooseColl_GetCollections.Click += new System.EventHandler(this.ChooseColl_GetCollections_Click);
            // 
            // ChooseColl_OK
            // 
            this.ChooseColl_OK.Location = new System.Drawing.Point(483, 210);
            this.ChooseColl_OK.Name = "ChooseColl_OK";
            this.ChooseColl_OK.Size = new System.Drawing.Size(81, 31);
            this.ChooseColl_OK.TabIndex = 3;
            this.ChooseColl_OK.Text = "确定";
            this.ChooseColl_OK.Click += new System.EventHandler(this.ChooseColl_OK_Click);
            // 
            // ChooseColl_Hint_Label
            // 
            this.ChooseColl_Hint_Label.AutoSize = true;
            this.ChooseColl_Hint_Label.Location = new System.Drawing.Point(12, 9);
            this.ChooseColl_Hint_Label.Name = "ChooseColl_Hint_Label";
            this.ChooseColl_Hint_Label.Size = new System.Drawing.Size(251, 19);
            this.ChooseColl_Hint_Label.TabIndex = 0;
            this.ChooseColl_Hint_Label.Text = "双击Collection名称或单独曲目名称导入";
            // 
            // ChooseColl_ClearPlayList
            // 
            this.ChooseColl_ClearPlayList.Location = new System.Drawing.Point(483, 144);
            this.ChooseColl_ClearPlayList.Name = "ChooseColl_ClearPlayList";
            this.ChooseColl_ClearPlayList.Size = new System.Drawing.Size(81, 29);
            this.ChooseColl_ClearPlayList.TabIndex = 3;
            this.ChooseColl_ClearPlayList.Text = "清空列表";
            this.ChooseColl_ClearPlayList.Click += new System.EventHandler(this.ChooseColl_ClearPlayList_Click);
            // 
            // ChooseColl_PlayListCurrentCount
            // 
            this.ChooseColl_PlayListCurrentCount.AutoSize = true;
            this.ChooseColl_PlayListCurrentCount.Location = new System.Drawing.Point(284, 9);
            this.ChooseColl_PlayListCurrentCount.Name = "ChooseColl_PlayListCurrentCount";
            this.ChooseColl_PlayListCurrentCount.Size = new System.Drawing.Size(149, 19);
            this.ChooseColl_PlayListCurrentCount.TabIndex = 4;
            this.ChooseColl_PlayListCurrentCount.Text = "当前播放列表曲目数：";
            // 
            // ChooseColl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 301);
            this.Controls.Add(this.ChooseColl_PlayListCurrentCount);
            this.Controls.Add(this.ChooseColl_ClearPlayList);
            this.Controls.Add(this.ChooseColl_Hint_Label);
            this.Controls.Add(this.ChooseColl_OK);
            this.Controls.Add(this.ChooseColl_GetCollections);
            this.Controls.Add(this.ChooseColl_CollectionContent_List);
            this.Controls.Add(this.ChooseColl_CollectionTitle_List);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChooseColl";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "ChooseColl";
            this.Load += new System.EventHandler(this.ChooseColl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ChooseColl_GetCollections)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChooseColl_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChooseColl_ClearPlayList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.ListBox ChooseColl_CollectionTitle_List;
        private System.Windows.Forms.ListBox ChooseColl_CollectionContent_List;
        private Telerik.WinControls.UI.RadButton ChooseColl_GetCollections;
        private Telerik.WinControls.UI.RadButton ChooseColl_OK;
        private System.Windows.Forms.Label ChooseColl_Hint_Label;
        private Telerik.WinControls.UI.RadButton ChooseColl_ClearPlayList;
        private System.Windows.Forms.Label ChooseColl_PlayListCurrentCount;
    }
}