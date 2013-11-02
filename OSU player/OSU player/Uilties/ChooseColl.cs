using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace OSU_player
{
    public partial class ChooseColl : Form
    {
        public ChooseColl()
        {
            InitializeComponent();
        }

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
            MessageBox.Show("刷新完毕！");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0) { return; }
            List<int> CollectionMaps = Core.Collections[listBox1.SelectedItem.ToString()];
            listBox2.Items.Clear();
            foreach (int mapindex in CollectionMaps)
            {
                listBox2.Items.Add(Core.allsets[mapindex].ToString());
            }
        }
    }
}
