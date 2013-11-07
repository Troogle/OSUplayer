using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
namespace OSU_player
{
    public partial class DelDulp : RadForm
    {
        public DelDulp()
        {
            InitializeComponent();
        }
        private List<Beatmap> tmpbms = new List<Beatmap>();
        private Dictionary<string, List<int>> dul = new Dictionary<string, List<int>>();
        private int ok = 0;
        private int all = 1;
        private void scanforset(string path)
        {
            string[] osufiles = Directory.GetFiles(path, "*.osu");
            if (osufiles.Length != 0)
            {
                foreach (var osufile in osufiles)
                {
                    Beatmap tmp = new Beatmap(osufile, path);
                    tmpbms.Add(tmp);
                }
                this.backgroundWorker1.ReportProgress(0);
            }
            else
            {
                string[] tmpfolder = Directory.GetDirectories(path);
                all += tmpfolder.Length;
                this.backgroundWorker1.ReportProgress(0);
                foreach (string subfolder in tmpfolder)
                {
                    scanforset(subfolder);
                }
            }
            ok++;
        }
        private void gethashs()
        {
            for (int i = 0; i < tmpbms.Count; i++)
            {
                tmpbms[i].GetHash();
                this.backgroundWorker2.ReportProgress(i);
            }
        }
        private void scanfordul(int index)
        {
            this.backgroundWorker3.ReportProgress(0);
            string hash = tmpbms[index].GetHash();
            List<int> tmp = new List<int>();
            tmp.Add(index);
            for (int i = index + 1; i < tmpbms.Count; i++)
            {
                if (hash == tmpbms[i].GetHash())
                {
                    tmp.Add(i);
                }
            }
            if (tmp.Count != 1)
            {
                if (!dul.ContainsKey(hash)) { dul.Add(hash, tmp); }
                else
                {
                    dul[hash].AddRange(tmp);
                }
            }
        }
        private void adddul()
        {
            int now = 0;
            foreach (var md5 in dul.Keys)
            {
                Color nowcolor = (now % 2 == 0) ? Color.White : Color.WhiteSmoke;
                foreach (var mapcount in dul[md5])
                {
                    ListViewItem tmp = new ListViewItem(tmpbms[mapcount].Location);
                    tmp.BackColor = nowcolor;
                    tmp.SubItems.Add(md5);
                    listView1.Items.Add(tmp);
                }
                now++;
            }
            listView1.Columns[0].Width = -1;
            button1.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Label1.Text = "扫描歌曲目录";
            scanforset(e.Argument.ToString());
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Maximum = all;
            progressBar1.Value1 = ok;
            Label1.Text = String.Format("扫描歌曲目录{0}/{1}", ok, all);
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int i = 1; i < tmpbms.Count; i++)
            {
                while (i < tmpbms.Count && tmpbms[i].Location == tmpbms[i - 1].Location) { tmpbms.RemoveAt(i); }
            }
            progressBar1.Maximum = tmpbms.Count;
            progressBar1.Value1 = 0;
            this.backgroundWorker2.RunWorkerAsync(0);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button2.Enabled = false;
            try
            {
                if (Directory.Exists(Path.Combine(Core.osupath, "Songs")))
                {
                    this.backgroundWorker1.RunWorkerAsync(Path.Combine(Core.osupath, "Songs"));
                }
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw (new FormatException("Failed to read song path", ex));
            }
        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            gethashs();
        }
        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value1++;
            Label1.Text = String.Format("获得歌曲信息{0}/{1}", progressBar1.Value1, tmpbms.Count);
        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value1 = 0;
            this.backgroundWorker3.RunWorkerAsync(0);
        }
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < tmpbms.Count; i++)
            {
                scanfordul(i);
            }
        }
        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value1++;
            Label1.Text = String.Format("寻找重复map{0}/{1}", progressBar1.Value1, tmpbms.Count);
        }
        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (dul.Count == 0) { RadMessageBox.Show("没有神马要删除的！><"); this.Dispose(); }
            else
            {
                RadMessageBox.Show(string.Format("扫描完毕，发现重复曲目{0}个", dul.Count));
                Label1.Text = string.Format("扫描完毕，发现重复曲目{0}个", dul.Count);
                adddul();
            }
            button2.Enabled = true;
        }
        private const int FO_DELETE = 0x3;
        private const ushort FOF_ALLOWUNDO = 0x40;
        private const ushort FOF_WANTNUKEWARNING = 0x4000;
        [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int SHFileOperation([In, Out] _SHFILEOPSTRUCT str);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private class _SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public UInt32 wFunc;
            public string pFrom;
            public string pTo;
            public UInt16 fFlags;
            public Int32 fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }
        private int Delete(List<string> path)
        {
            _SHFILEOPSTRUCT pm = new _SHFILEOPSTRUCT();
            pm.wFunc = FO_DELETE;
            pm.pFrom = path[0];
            for (int i = 1; i < path.Count; i++)
            {
                pm.pFrom += '\0' + path[i];
            }
            pm.pFrom += '\0';
            pm.pTo = null;
            pm.fFlags = FOF_ALLOWUNDO | FOF_WANTNUKEWARNING;
            return SHFileOperation(pm);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.CheckedItems.Count == 0)
            {
                RadMessageBox.Show("没有神马要删除的！><");
                return;
            }
            int count = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string md5 = listView1.Items[i].SubItems[1].Text;
                int con = 0;
                while (i + con < listView1.Items.Count
                    && listView1.Items[i + con].Checked
                    && con < dul[md5].Count) { con++; }
                if (con == dul[md5].Count) { count++; }
                i += dul[md5].Count - 1;
            }
            if (count != 0)
            {
                if (RadMessageBox.Show(string.Format("有{0}组被全部选中，确定继续？", count), "提示", MessageBoxButtons.YesNo) != DialogResult.Yes) { return; }
            }
            if (RadMessageBox.Show(string.Format("将删除{0}个，确定继续？", listView1.CheckedItems.Count), "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<string> filedelete = new List<string>();
                foreach (ListViewItem item in listView1.CheckedItems)
                {
                    filedelete.Add(item.Text);
                }
                Delete(filedelete);
                RadMessageBox.Show(string.Format("删除完毕，共删除{0}个", listView1.CheckedItems.Count));
                this.Dispose();
            }
        }
        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            if (listView1.Width > listView1.Columns[0].Width)
            { listView1.Columns[0].Width = listView1.Width - 10; }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Select();
            listView1.SelectedItems.Clear();
            foreach (ListViewItem tmp in listView1.CheckedItems)
            {
                tmp.Checked = false;
            }
            for (int i = 0; i < listView1.Items.Count - 1; i++)
            {
                if (listView1.Items[i].BackColor == listView1.Items[i + 1].BackColor)
                {
                    listView1.Items[i].Selected = true;
                    listView1.Items[i].Checked = true;
                    continue;
                }
                listView1.Items[i].Selected = false;
                listView1.Items[i].Checked = false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem tmp in listView1.SelectedItems)
            {
                tmp.Checked = true;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            listView1.SelectedItems.Clear();
            foreach (ListViewItem tmp in listView1.CheckedItems)
            {
                tmp.Checked = false;
            }
        }
        private void DelDulp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!button2.Enabled) { e.Cancel = true; }
        }
    }
}
