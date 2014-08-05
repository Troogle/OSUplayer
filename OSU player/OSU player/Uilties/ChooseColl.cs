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
        private void ChooseColl_CollectionTitle_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChooseColl_CollectionTitle_List.SelectedItems.Count == 0) { return; }
            var collectionMaps = Core.Collections[ChooseColl_CollectionTitle_List.SelectedItem.ToString()];
            ChooseColl_CollectionContent_List.Items.Clear();
            foreach (var mapindex in collectionMaps)
            {
                ChooseColl_CollectionContent_List.Items.Add(Core.Allsets[mapindex].ToString());
            }
        }
        private void ChooseColl_CollectionTitle_List_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (ChooseColl_CollectionTitle_List.SelectedItems.Count == 0) { return; }
            Core.CurrentListName = ChooseColl_CollectionTitle_List.SelectedItem.ToString();
            NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), String.Format("成功切换到{0}", Core.CurrentListName));
            ChooseColl_PlayListCurrentCount.Text = LanguageManager.Get("Current_Count_Text") + Core.PlayList.Count;
            if (Core.PlayList.Count == 0)
            {
                Core.CurrentListName = "Full";
            }
            this.Dispose();
        }
        private void ChooseColl_Load(object sender, EventArgs e)
        {
            ChooseColl_PlayListCurrentCount.Text = LanguageManager.Get("Current_Count_Text") + Core.PlayList.Count;
            ChooseColl_CollectionTitle_List.Items.Clear();
            foreach (string key in Core.Collections.Keys)
            {
                ChooseColl_CollectionTitle_List.Items.Add(key);
            }
            if (ChooseColl_CollectionTitle_List.Items.Count != 0) { ChooseColl_CollectionTitle_List.SelectedIndex = 0; }
        }
    }
}
