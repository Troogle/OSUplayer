using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OSUplayer.Properties;
using OSUplayer.OsuFiles;
namespace OSUplayer.Uilties
{
    public partial class DelDulp : Form
    {
        public DelDulp()
        {
            InitializeComponent();
            LanguageManager.ApplyLanguage(this);
        }
        private readonly List<Beatmap> _allBeatmap = new List<Beatmap>();
        private readonly Dictionary<string, List<int>> _dulpdict = new Dictionary<string, List<int>>();
        private int _ok;
        private int _all = 1;
        private void Scanforset(string path)
        {
            string[] osufiles = Directory.GetFiles(path, "*.osu");
            if (osufiles.Length != 0)
            {
                foreach (var osufile in osufiles)
                {
                    var tmp = new Beatmap(osufile, path);
                    _allBeatmap.Add(tmp);
                }
                BackgroundWorker1.ReportProgress(0);
            }
            else
            {
                var tmpfolder = Directory.GetDirectories(path);
                _all += tmpfolder.Length;
                BackgroundWorker1.ReportProgress(0);
                foreach (string subfolder in tmpfolder)
                {
                    Scanforset(subfolder);
                }
            }
            _ok++;
        }
        private void Gethashs()
        {
            for (int i = 0; i < _allBeatmap.Count; i++)
            {
                _allBeatmap[i].GetHash();
                BackgroundWorker2.ReportProgress(i);
            }
        }
        private void Scanfordul(int index)
        {
            BackgroundWorker3.ReportProgress(0);
            var hash = _allBeatmap[index].GetHash();
            var tmp = new List<int> {index};
            for (var i = index + 1; i < _allBeatmap.Count; i++)
            {
                if (hash == _allBeatmap[i].GetHash())
                {
                    tmp.Add(i);
                }
            }
            if (tmp.Count == 1) return;
            if (!_dulpdict.ContainsKey(hash)) { _dulpdict.Add(hash, tmp); }
            else
            {
                _dulpdict[hash].AddRange(tmp);
            }
        }
        private void Adddul()
        {
            var now = 0;
            foreach (var md5 in _dulpdict.Keys)
            {
                var nowcolor = (now % 2 == 0) ? Color.White : Color.WhiteSmoke;
                foreach (var mapcount in _dulpdict[md5])
                {
                    var tmp = new ListViewItem(_allBeatmap[mapcount].Location) { BackColor = nowcolor };
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
            DelDulp_Status_Label.Text = String.Format(LanguageManager.Get("Scan_Folder_Text"), 0, 0);
            Scanforset(e.Argument.ToString());
        }
        private void BackgroundWorker1ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DelDulp_ProgressBar.Maximum = _all;
            DelDulp_ProgressBar.Value = _ok;
            DelDulp_Status_Label.Text = String.Format(LanguageManager.Get("Scan_Folder_Text"), _ok, _all);
        }
        private void BackgroundWorker1RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (var i = 1; i < _allBeatmap.Count; i++)
            {
                while (i < _allBeatmap.Count && _allBeatmap[i].Location == _allBeatmap[i - 1].Location) { _allBeatmap.RemoveAt(i); }
            }
            DelDulp_ProgressBar.Maximum = _allBeatmap.Count;
            DelDulp_ProgressBar.Value = 0;
            BackgroundWorker2.RunWorkerAsync(0);
        }
        private void DelDulp_StartSearch_Click(object sender, EventArgs e)
        {
            DelDulp_StartSearch.Enabled = false;
            DelDulp_Cancel.Enabled = false;
            try
            {
                if (Directory.Exists(Path.Combine(Settings.Default.OSUpath, "Songs")))
                {
                    BackgroundWorker1.RunWorkerAsync(Path.Combine(Settings.Default.OSUpath, "Songs"));
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
            DelDulp_ProgressBar.Value++;
            DelDulp_Status_Label.Text = String.Format(LanguageManager.Get("Get_Song_Info_Text"), DelDulp_ProgressBar.Value, _allBeatmap.Count);
        }
        private void BackgroundWorker2RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DelDulp_ProgressBar.Value = 0;
            BackgroundWorker3.RunWorkerAsync(0);
        }
        private void BackgroundWorker3DoWork(object sender, DoWorkEventArgs e)
        {
            for (var i = 0; i < _allBeatmap.Count; i++)
            {
                Scanfordul(i);
            }
        }
        private void BackgroundWorker3ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DelDulp_ProgressBar.Value++;
            DelDulp_Status_Label.Text = String.Format(LanguageManager.Get("Scan_Duplicate_Text"), DelDulp_ProgressBar.Value, _allBeatmap.Count);
        }
        private void BackgroundWorker3RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_dulpdict.Count == 0) { MessageBox.Show(LanguageManager.Get("Scan_Zero_Text")); Dispose(); }
            else
            {
                NotifySystem.Showtip(1000, "OSUplayer", string.Format(LanguageManager.Get("Scan_Complete_Text"), _dulpdict.Count));
                DelDulp_Status_Label.Text = string.Format(LanguageManager.Get("Scan_Complete_Text"), _dulpdict.Count);
                Adddul();
            }
            DelDulp_Cancel.Enabled = true;
        }
        private void DelDulp_DeleteSelected_Click(object sender, EventArgs e)
        {
            if (DelDulp_MainView.CheckedItems.Count == 0)
            {
                MessageBox.Show(LanguageManager.Get("Scan_Zero_Text"));
                return;
            }
            var count = 0;
            for (var i = 0; i < DelDulp_MainView.Items.Count; i++)
            {
                var md5 = DelDulp_MainView.Items[i].SubItems[1].Text;
                var con = 0;
                while (i + con < DelDulp_MainView.Items.Count
                    && DelDulp_MainView.Items[i + con].Checked
                    && con < _dulpdict[md5].Count) { con++; }
                if (con == _dulpdict[md5].Count) { count++; }
                i += _dulpdict[md5].Count - 1;
            }
            if (count != 0)
            {
                if (MessageBox.Show(string.Format(LanguageManager.Get("Delete_All_Text"), count), LanguageManager.Get("Tip_Text"), MessageBoxButtons.YesNo) != DialogResult.Yes) { return; }
            }
            if (
                MessageBox.Show(string.Format(LanguageManager.Get("Delete_Comfirm_Text"), DelDulp_MainView.CheckedItems.Count),
                    LanguageManager.Get("Tip_Text"), MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            var filedelete = (from ListViewItem item in DelDulp_MainView.CheckedItems select item.Text).ToList();
            Win32.Delete(filedelete);
            NotifySystem.Showtip(1000, LanguageManager.Get("OSUplayer"), string.Format(LanguageManager.Get("Delete_Complete_Text"), DelDulp_MainView.CheckedItems.Count));
            Dispose();
        }
        private void DelDulp_MainView_SizeChanged(object sender, EventArgs e)
        {
            if (DelDulp_MainView.Width > DelDulp_MainView.Columns[0].Width)
            { DelDulp_MainView.Columns[0].Width = DelDulp_MainView.Width - 10; }
        }
        private void DelDulp_AutoSelect_Click(object sender, EventArgs e)
        {
            DelDulp_MainView.Select();
            DelDulp_MainView.SelectedItems.Clear();
            foreach (ListViewItem tmp in DelDulp_MainView.CheckedItems)
            {
                tmp.Checked = false;
            }
            for (var i = 0; i < DelDulp_MainView.Items.Count - 1; i++)
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
        private void DelDulp_Cancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        private void DelDulp_MainView_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem tmp in DelDulp_MainView.SelectedItems)
            {
                tmp.Checked = true;
            }
        }
        private void DelDulp_ClearSelected_Click(object sender, EventArgs e)
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
