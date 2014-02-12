using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using OSUplayer.OsuFiles;
namespace OSUplayer.Uilties
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
        private void Scanforset(string path)
        {
            string[] osufiles = Directory.GetFiles(path, "*.osu");
            if (osufiles.Length != 0)
            {
                foreach (var osufile in osufiles)
                {
                    Beatmap tmp = new Beatmap(osufile, path);
                    tmpbms.Add(tmp);
                }
                this.BackgroundWorker1.ReportProgress(0);
            }
            else
            {
                string[] tmpfolder = Directory.GetDirectories(path);
                all += tmpfolder.Length;
                this.BackgroundWorker1.ReportProgress(0);
                foreach (string subfolder in tmpfolder)
                {
                    Scanforset(subfolder);
                }
            }
            ok++;
        }
        private void Gethashs()
        {
            for (int i = 0; i < tmpbms.Count; i++)
            {
                tmpbms[i].GetHash();
                this.BackgroundWorker2.ReportProgress(i);
            }
        }
        private void Scanfordul(int index)
        {
            this.BackgroundWorker3.ReportProgress(0);
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
        private void Adddul()
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
                    DelDulp_MainView.Items.Add(tmp);
                }
                now++;
            }
            DelDulp_MainView.Columns[0].Width = -1;
            DelDulp_DeleteSelected.Enabled = true;
            DelDulp_AutoSelect.Enabled = true;
            DelDulp_ClearSelected.Enabled = true;
        }
        private void BackgroundWorker1DoWork(object sender, DoWorkEventArgs e)
        {
            DelDulp_Status_Label.Text = "扫描歌曲目录";
            Scanforset(e.Argument.ToString());
        }
        private void BackgroundWorker1ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DelDulp_ProgressBar.Maximum = all;
            DelDulp_ProgressBar.Value1 = ok;
            DelDulp_Status_Label.Text = String.Format("扫描歌曲目录{0}/{1}", ok, all);
        }
        private void BackgroundWorker1RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int i = 1; i < tmpbms.Count; i++)
            {
                while (i < tmpbms.Count && tmpbms[i].Location == tmpbms[i - 1].Location) { tmpbms.RemoveAt(i); }
            }
            DelDulp_ProgressBar.Maximum = tmpbms.Count;
            DelDulp_ProgressBar.Value1 = 0;
            this.BackgroundWorker2.RunWorkerAsync(0);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DelDulp_StartSearch.Enabled = false;
            DelDulp_Cancel.Enabled = false;
            try
            {
                if (Directory.Exists(Path.Combine(Core.Osupath, "Songs")))
                {
                    this.BackgroundWorker1.RunWorkerAsync(Path.Combine(Core.Osupath, "Songs"));
                }
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw (new FormatException("Failed to read song path", ex));
            }
        }
        private void BackgroundWorker2DoWork(object sender, DoWorkEventArgs e)
        {
            Gethashs();
        }
        private void BackgroundWorker2ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DelDulp_ProgressBar.Value1++;
            DelDulp_Status_Label.Text = String.Format("获得歌曲信息{0}/{1}", DelDulp_ProgressBar.Value1, tmpbms.Count);
        }
        private void BackgroundWorker2RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DelDulp_ProgressBar.Value1 = 0;
            this.BackgroundWorker3.RunWorkerAsync(0);
        }
        private void BackgroundWorker3DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < tmpbms.Count; i++)
            {
                Scanfordul(i);
            }
        }
        private void BackgroundWorker3ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DelDulp_ProgressBar.Value1++;
            DelDulp_Status_Label.Text = String.Format("寻找重复map{0}/{1}", DelDulp_ProgressBar.Value1, tmpbms.Count);
        }
        private void BackgroundWorker3RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (dul.Count == 0) { RadMessageBox.Show("没有神马要删除的！><"); this.Dispose(); }
            else
            {
                NotifySystem.Showtip(1000, "OSUplayer", string.Format("扫描完毕，发现重复曲目{0}个", dul.Count), System.Windows.Forms.ToolTipIcon.Info);
                DelDulp_Status_Label.Text = string.Format("扫描完毕，发现重复曲目{0}个", dul.Count);
                Adddul();
            }
            DelDulp_Cancel.Enabled = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (DelDulp_MainView.CheckedItems.Count == 0)
            {
                RadMessageBox.Show("没有神马要删除的！><");
                return;
            }
            int count = 0;
            for (int i = 0; i < DelDulp_MainView.Items.Count; i++)
            {
                string md5 = DelDulp_MainView.Items[i].SubItems[1].Text;
                int con = 0;
                while (i + con < DelDulp_MainView.Items.Count
                    && DelDulp_MainView.Items[i + con].Checked
                    && con < dul[md5].Count) { con++; }
                if (con == dul[md5].Count) { count++; }
                i += dul[md5].Count - 1;
            }
            if (count != 0)
            {
                if (RadMessageBox.Show(string.Format("有{0}组被全部选中，确定继续？", count), "提示", MessageBoxButtons.YesNo) != DialogResult.Yes) { return; }
            }
            if (RadMessageBox.Show(string.Format("将删除{0}个，确定继续？", DelDulp_MainView.CheckedItems.Count), "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                List<string> filedelete = new List<string>();
                foreach (ListViewItem item in DelDulp_MainView.CheckedItems)
                {
                    filedelete.Add(item.Text);
                }
                Win32.Delete(filedelete);
                NotifySystem.Showtip(1000, "OSUplayer", string.Format("删除完毕，共删除{0}个", DelDulp_MainView.CheckedItems.Count), System.Windows.Forms.ToolTipIcon.Info);
                this.Dispose();
            }
        }
        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            if (DelDulp_MainView.Width > DelDulp_MainView.Columns[0].Width)
            { DelDulp_MainView.Columns[0].Width = DelDulp_MainView.Width - 10; }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            DelDulp_MainView.Select();
            DelDulp_MainView.SelectedItems.Clear();
            foreach (ListViewItem tmp in DelDulp_MainView.CheckedItems)
            {
                tmp.Checked = false;
            }
            for (int i = 0; i < DelDulp_MainView.Items.Count - 1; i++)
            {
                if (DelDulp_MainView.Items[i].BackColor == DelDulp_MainView.Items[i + 1].BackColor)
                {
                    DelDulp_MainView.Items[i].Selected = true;
                    DelDulp_MainView.Items[i].Checked = true;
                    continue;
                }
                DelDulp_MainView.Items[i].Selected = false;
                DelDulp_MainView.Items[i].Checked = false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem tmp in DelDulp_MainView.SelectedItems)
            {
                tmp.Checked = true;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            DelDulp_MainView.SelectedItems.Clear();
            foreach (ListViewItem tmp in DelDulp_MainView.CheckedItems)
            {
                tmp.Checked = false;
            }
        }
        private void DelDulp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!DelDulp_Cancel.Enabled) { e.Cancel = true; }
        }
    }
}
