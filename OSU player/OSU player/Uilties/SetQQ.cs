using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace OSUplayer.Uilties
{
    public partial class SetQQ : RadForm
    {
        private readonly BackgroundWorker refreash = new BackgroundWorker();
        private QQ qq = new QQ();
        private List<QQInfo> QQInfos = new List<QQInfo>();

        public SetQQ()
        {
            InitializeComponent();
            refreash.DoWork += refreash_DoWork;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (SetQQ_MainView.Visible)
            {
                try
                {
                    Core.uin = SetQQ_MainView.SelectedItems[0].Text;
                    if (Core.uin == "清空QQ号")
                    {
                        Core.uin = "0";
                    }
                    Dispose();
                }
                catch (Exception)
                {
                    if (SetQQ_MainView.SelectedItems.Count == 0)
                    {
                        RadMessageBox.Show("别卖萌不选啊-0-");
                    }
                    else
                    {
                        Core.uin = "0";
                        Core.syncQQ = false;
                        Dispose();
                    }
                }
            }
            else
            {
                var isNumeric = new Regex(@"^\d+$");             
                if ((!isNumeric.IsMatch(SetQQ_Manual.Text)) || (SetQQ_Manual.Text.Trim() == ""))
                {
                    RadMessageBox.Show("亲正常点~请输入QQ号~~");
                }
                else
                {
                    Core.uin = SetQQ_Manual.Text;
                    Core.syncQQ = Core.uin != "0";
                    Dispose();
                }
            }
        }

        private void RefreshQQ(object sender, EventArgs e)
        {
            Focus();
            SetQQ_MainView.Clear();
            SetQQ_MainView.Columns.Add("ID", 100);
            SetQQ_MainView.Columns.Add("昵称", 100);
            QQInfos = qq.GetQQList;
            try
            {
                ListViewItem tmpl;
                foreach (var t in QQInfos)
                {
                    tmpl = new ListViewItem (t.uin);
                    tmpl.SubItems.Add(t.nick);
                    SetQQ_MainView.Items.Add(tmpl);
                }
                tmpl = new ListViewItem("清空QQ号");
                tmpl.SubItems.Add("");
                SetQQ_MainView.Items.Add(tmpl);
            }
            catch (Exception)
            {
                RadMessageBox.Show("获取当前在线QQ出错！");
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

        private void Button1_Click(object sender, EventArgs e)
        {
            if (SetQQ_MainView.Visible)
            {
                SetQQ_GetQQ.Text = "自动获取";
                SetQQ_MainView.Visible = false;
                SetQQ_Manual.Visible = true;
            }
            else
            {
                SetQQ_GetQQ.Text = "手动获取";
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