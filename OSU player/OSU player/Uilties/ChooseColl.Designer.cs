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
            this.ChooseColl_Hint_Label = new System.Windows.Forms.Label();
            this.ChooseColl_PlayListCurrentCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // ChooseColl_CollectionTitle_List
            // 
            this.ChooseColl_CollectionTitle_List.FormattingEnabled = true;
            this.ChooseColl_CollectionTitle_List.ItemHeight = 15;
            this.ChooseColl_CollectionTitle_List.Location = new System.Drawing.Point(12, 36);
            this.ChooseColl_CollectionTitle_List.Name = "ChooseColl_CollectionTitle_List";
            this.ChooseColl_CollectionTitle_List.Size = new System.Drawing.Size(130, 259);
            this.ChooseColl_CollectionTitle_List.TabIndex = 0;
            this.ChooseColl_CollectionTitle_List.SelectedIndexChanged += new System.EventHandler(this.ChooseColl_CollectionTitle_List_SelectedIndexChanged);
            this.ChooseColl_CollectionTitle_List.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ChooseColl_CollectionTitle_List_MouseDoubleClick);
            // 
            // ChooseColl_CollectionContent_List
            // 
            this.ChooseColl_CollectionContent_List.FormattingEnabled = true;
            this.ChooseColl_CollectionContent_List.ItemHeight = 15;
            this.ChooseColl_CollectionContent_List.Location = new System.Drawing.Point(148, 36);
            this.ChooseColl_CollectionContent_List.Name = "ChooseColl_CollectionContent_List";
            this.ChooseColl_CollectionContent_List.Size = new System.Drawing.Size(326, 259);
            this.ChooseColl_CollectionContent_List.TabIndex = 1;
            // 
            // ChooseColl_Hint_Label
            // 
            this.ChooseColl_Hint_Label.AutoSize = true;
            this.ChooseColl_Hint_Label.Location = new System.Drawing.Point(12, 9);
            this.ChooseColl_Hint_Label.Name = "ChooseColl_Hint_Label";
            this.ChooseColl_Hint_Label.Size = new System.Drawing.Size(153, 19);
            this.ChooseColl_Hint_Label.TabIndex = 0;
            this.ChooseColl_Hint_Label.Text = "双击Collection名称切换";
            // 
            // ChooseColl_PlayListCurrentCount
            // 
            this.ChooseColl_PlayListCurrentCount.AutoSize = true;
            this.ChooseColl_PlayListCurrentCount.Location = new System.Drawing.Point(187, 9);
            this.ChooseColl_PlayListCurrentCount.Name = "ChooseColl_PlayListCurrentCount";
            this.ChooseColl_PlayListCurrentCount.Size = new System.Drawing.Size(149, 19);
            this.ChooseColl_PlayListCurrentCount.TabIndex = 4;
            this.ChooseColl_PlayListCurrentCount.Text = "当前播放列表曲目数：";
            // 
            // ChooseColl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 301);
            this.Controls.Add(this.ChooseColl_PlayListCurrentCount);
            this.Controls.Add(this.ChooseColl_Hint_Label);
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
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.ListBox ChooseColl_CollectionTitle_List;
        private System.Windows.Forms.ListBox ChooseColl_CollectionContent_List;
        private System.Windows.Forms.Label ChooseColl_Hint_Label;
        private System.Windows.Forms.Label ChooseColl_PlayListCurrentCount;
    }
}