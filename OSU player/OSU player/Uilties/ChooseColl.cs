using OSUplayer.OsuFiles;
using System;
using System.Collections.Generic;
using System.IO;
using Telerik.WinControls.UI;
namespace OSUplayer.Uilties
{
    public partial class ChooseColl : RadForm
    {
        public ChooseColl()
        {
            InitializeComponent();
        }
        private List<int> tmpindex = new List<int>();
        private void button1_Click(object sender, EventArgs e)
        {
            string collectpath = Path.Combine(Core.Osupath, "collection.db");
            if (File.Exists(collectpath)) { OsuDB.ReadCollect(collectpath); }
            ChooseColl_CollectionTitle_List.Items.Clear();
            foreach (string key in Core.Collections.Keys)
            {
                ChooseColl_CollectionTitle_List.Items.Add(key);
            }
            if (ChooseColl_CollectionTitle_List.Items.Count != 0) { ChooseColl_CollectionTitle_List.SelectedIndex = 0; }
            NotifySystem.Showtip(1000, "OSUplayer", "刷新完毕！", System.Windows.Forms.ToolTipIcon.Info);
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChooseColl_CollectionTitle_List.SelectedItems.Count == 0) { return; }
            List<int> CollectionMaps = Core.Collections[ChooseColl_CollectionTitle_List.SelectedItem.ToString()];
            ChooseColl_CollectionContent_List.Items.Clear();
            tmpindex.Clear();
            foreach (int mapindex in CollectionMaps)
            {
                ChooseColl_CollectionContent_List.Items.Add(Core.allsets[mapindex].ToString());
                tmpindex.Add(mapindex);
            }
        }
        private void listBox2_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (ChooseColl_CollectionContent_List.SelectedItems.Count == 0) { return; }
            Core.AddSet(tmpindex[ChooseColl_CollectionContent_List.SelectedIndex]);
            NotifySystem.Showtip(1000, "OSUplayer", String.Format("成功导入{0}", ChooseColl_CollectionContent_List.SelectedItem.ToString()), System.Windows.Forms.ToolTipIcon.Info);
            ChooseColl_PlayListCurrentCount.Text = "当前播放列表曲目数:" + Core.PlayList.Count;
        }
        private void listBox1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (ChooseColl_CollectionTitle_List.SelectedItems.Count == 0) { return; }
            Core.AddRangeSet(tmpindex);
            NotifySystem.Showtip(1000, "OSUplayer", String.Format("成功导入{0}首曲目", tmpindex.Count.ToString()), System.Windows.Forms.ToolTipIcon.Info);
            ChooseColl_PlayListCurrentCount.Text = "当前播放列表曲目数:" + Core.PlayList.Count;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (Core.PlayList.Count == 0)
            {
                Core.initplaylist();
            }
            this.Dispose();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Core.PlayList.Clear();
            ChooseColl_PlayListCurrentCount.Text = "当前播放列表曲目数:0";
        }

        private void ChooseColl_Load(object sender, EventArgs e)
        {
            ChooseColl_PlayListCurrentCount.Text = "当前播放列表曲目数:"+Core.PlayList.Count;
            ChooseColl_GetCollections.PerformClick();
        }
    }
}
