using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OSUplayer.Properties;

namespace OSUplayer.Uilties
{
    public partial class SetQQ : Form
    {
        private readonly BackgroundWorker refreash = new BackgroundWorker();
        private readonly QQ qq = new QQ();
        private List<QQInfo> QQInfos = new List<QQInfo>();

        public SetQQ()
        {
            InitializeComponent();
            refreash.DoWork += refreash_DoWork;
        }

        private void SetQQ_OK_Click(object sender, EventArgs e)
        {
            if (SetQQ_MainView.Visible)
            {
                try
                {
                    Settings.Default.QQuin = SetQQ_MainView.SelectedItems[0].Text;
                    if (Settings.Default.QQuin == LanguageManager.Get("SetQQ_ClearQQ_Text"))
                    {
                        Settings.Default.QQuin = "0";
                    }
                    Dispose();
                }
                catch (Exception)
                {
                    if (SetQQ_MainView.SelectedItems.Count == 0)
                    {
                        MessageBox.Show(LanguageManager.Get("SetQQ_NoQQ_Text"), LanguageManager.Get("Tip_Text"));
                    }
                    else
                    {
                        Settings.Default.QQuin = "0";
                        Settings.Default.SyncQQ = false;
                        Dispose();
                    }
                }
            }
            else
            {
                var isNumeric = new Regex(@"^\d+$");
                if ((!isNumeric.IsMatch(SetQQ_Manual.Text)) || (SetQQ_Manual.Text.Trim() == ""))
                {
                    MessageBox.Show(LanguageManager.Get("SetQQ_NoInput_Text"), LanguageManager.Get("Tip_Text"));
                }
                else
                {
                    Settings.Default.QQuin = SetQQ_Manual.Text;
                    Settings.Default.SyncQQ = Settings.Default.QQuin != "0";
                    Dispose();
                }
            }
        }

        private void RefreshQQ(object sender, EventArgs e)
        {
            Focus();
            SetQQ_MainView.Items.Clear();
            QQInfos = qq.GetQQList;
            try
            {
                ListViewItem tmpl;
                foreach (var t in QQInfos)
                {
                    tmpl = new ListViewItem(t.Uin);
                    tmpl.SubItems.Add(t.Nick);
                    SetQQ_MainView.Items.Add(tmpl);
                }
                tmpl = new ListViewItem(LanguageManager.Get("SetQQ_ClearQQ_Text"));
                tmpl.SubItems.Add("");
                SetQQ_MainView.Items.Add(tmpl);
            }
            catch (Exception)
            {
                MessageBox.Show(LanguageManager.Get("SetQQ_Error_Text"), LanguageManager.Get("Error_Text"));
            }
        }

        private void refreash_DoWork(object sender, DoWorkEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler(RefreshQQ));
            }
            else
            {
                RefreshQQ(null, null);
            }
        }

        private void SetQQ_GetQQ_Click(object sender, EventArgs e)
        {
            if (SetQQ_MainView.Visible)
            {
                SetQQ_GetQQ.Text = LanguageManager.Get("SetQQ_AutoQQ_Text");
                SetQQ_MainView.Visible = false;
                SetQQ_Manual.Visible = true;
            }
            else
            {
                SetQQ_GetQQ.Text = LanguageManager.Get("SetQQ_GetQQ_Text");
                SetQQ_MainView.Visible = true;
                SetQQ_Manual.Visible = false;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            refreash.RunWorkerAsync();
        }
    }
}