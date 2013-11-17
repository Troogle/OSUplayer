using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using OSUplayer.OSUFiles;
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
            string collectpath = Path.Combine(Core.osupath, "collection.db");
            if (File.Exists(collectpath)) { OsuDB.ReadCollect(collectpath); }
            listBox1.Items.Clear();
            foreach (string key in Core.Collections.Keys)
            {
                listBox1.Items.Add(key);
            }
            if (listBox1.Items.Count != 0) { listBox1.SelectedIndex = 0; }
            Core.notifyIcon1.ShowBalloonTip(1000, "OSUplayer", "刷新完毕！", System.Windows.Forms.ToolTipIcon.Info);
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0) { return; }
            List<int> CollectionMaps = Core.Collections[listBox1.SelectedItem.ToString()];
            listBox2.Items.Clear();
            tmpindex.Clear();
            foreach (int mapindex in CollectionMaps)
            {
                listBox2.Items.Add(Core.allsets[mapindex].ToString());
                tmpindex.Add(mapindex);
            }
        }
        private void listBox2_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (listBox2.SelectedItems.Count == 0) { return; }
            Core.AddSet(tmpindex[listBox2.SelectedIndex]);
            Core.notifyIcon1.ShowBalloonTip(1000, "OSUplayer", String.Format("成功导入{0}", listBox2.SelectedItem.ToString()), System.Windows.Forms.ToolTipIcon.Info);
            label2.Text = "当前播放列表曲目数:" + Core.PlayList.Count;
        }
        private void listBox1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0) { return; }
            Core.AddRangeSet(tmpindex);
            Core.notifyIcon1.ShowBalloonTip(1000, "OSUplayer", String.Format("成功导入{0}首曲目", tmpindex.Count.ToString()), System.Windows.Forms.ToolTipIcon.Info);
            label2.Text = "当前播放列表曲目数:" + Core.PlayList.Count;
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
            label2.Text = "当前播放列表曲目数:0";
        }

        private void ChooseColl_Load(object sender, EventArgs e)
        {
            label2.Text = "当前播放列表曲目数:"+Core.PlayList.Count;
            button1.PerformClick();
        }
    }
}
