using OSUplayer.OsuFiles;
using System;
using System.Collections.Generic;
using System.IO;
using OSUplayer.Properties;
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
        private void ChooseColl_GetCollections_Click(object sender, EventArgs e)
        {
            string collectpath = Path.Combine(Settings.Default.OSUpath, "collection.db");
            if (File.Exists(collectpath)) { OsuDB.ReadCollect(collectpath); }
            ChooseColl_CollectionTitle_List.Items.Clear();
            foreach (string key in Core.Collections.Keys)
            {
                ChooseColl_CollectionTitle_List.Items.Add(key);
            }
            if (ChooseColl_CollectionTitle_List.Items.Count != 0) { ChooseColl_CollectionTitle_List.SelectedIndex = 0; }
            NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), LanguageManager.Get("Refresh_Complete_Text"));
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChooseColl_CollectionTitle_List.SelectedItems.Count == 0) { return; }
            var collectionMaps = Core.Collections[ChooseColl_CollectionTitle_List.SelectedItem.ToString()];
            ChooseColl_CollectionContent_List.Items.Clear();
            tmpindex.Clear();
            foreach (int mapindex in collectionMaps)
            {
                ChooseColl_CollectionContent_List.Items.Add(Core.Allsets[mapindex].ToString());
                tmpindex.Add(mapindex);
            }
        }
        private void ChooseColl_CollectionContent_List_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (ChooseColl_CollectionContent_List.SelectedItems.Count == 0) { return; }
            Core.AddSet(tmpindex[ChooseColl_CollectionContent_List.SelectedIndex]);
            NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), String.Format("成功导入{0}", ChooseColl_CollectionContent_List.SelectedItem));
            ChooseColl_PlayListCurrentCount.Text = LanguageManager.Get("Current_Count_Text") + Core.PlayList.Count;
        }
        private void ChooseColl_CollectionTitle_List_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (ChooseColl_CollectionTitle_List.SelectedItems.Count == 0) { return; }
            Core.AddRangeSet(tmpindex);
            NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), String.Format("成功导入{0}首曲目", tmpindex.Count));
            ChooseColl_PlayListCurrentCount.Text = LanguageManager.Get("Current_Count_Text") + Core.PlayList.Count;
        }
        private void ChooseColl_OK_Click(object sender, EventArgs e)
        {
            if (Core.PlayList.Count == 0)
            {
                Core.Initplaylist();
            }
            this.Dispose();
        }

        private void ChooseColl_ClearPlayList_Click(object sender, EventArgs e)
        {
            Core.PlayList.Clear();
            ChooseColl_PlayListCurrentCount.Text = LanguageManager.Get("Current_Count_Text") + Core.PlayList.Count;
        }

        private void ChooseColl_Load(object sender, EventArgs e)
        {
            ChooseColl_PlayListCurrentCount.Text = LanguageManager.Get("Current_Count_Text") + Core.PlayList.Count;
            ChooseColl_GetCollections.PerformClick();
        }

        private void ChooseColl_CollectionContent_List_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
