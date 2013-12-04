using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace OSUplayer.Uilties
{
    public partial class Form2 : RadForm
    {
        private readonly BackgroundWorker refreash = new BackgroundWorker();
        private QQ qq = new QQ();
        private List<QQInfo> QQInfos = new List<QQInfo>();

        public Form2()
        {
            InitializeComponent();
            refreash.DoWork += refreash_DoWork;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (ListView1.Visible)
            {
                try
                {
                    Core.uin = ListView1.SelectedItems[0].Text;
                    if (Core.uin == "清空QQ号")
                    {
                        Core.uin = "0";
                    }
                    Dispose();
                }
                catch (Exception)
                {
                    if (ListView1.SelectedItems.Count == 0)
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
                if ((!isNumeric.IsMatch(textBox1.Text)) || (textBox1.Text.Trim() == ""))
                {
                    RadMessageBox.Show("亲正常点~请输入QQ号~~");
                }
                else
                {
                    Core.uin = textBox1.Text;
                    Core.syncQQ = Core.uin != "0";
                    Dispose();
                }
            }
        }

        private void RefreshQQ(object sender, EventArgs e)
        {
            Focus();
            ListView1.Clear();
            ListView1.Columns.Add("ID", 100);
            ListView1.Columns.Add("昵称", 100);
            QQInfos = qq.GetQQList;
            try
            {
                ListViewItem tmpl;
                foreach (var t in QQInfos)
                {
                    tmpl = new ListViewItem (t.uin);
                    tmpl.SubItems.Add(t.nick);
                    ListView1.Items.Add(tmpl);
                }
                tmpl = new ListViewItem("清空QQ号");
                tmpl.SubItems.Add("");
                ListView1.Items.Add(tmpl);
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
            if (ListView1.Visible)
            {
                Button1.Text = "自动获取";
                ListView1.Visible = false;
                textBox1.Visible = true;
            }
            else
            {
                Button1.Text = "手动获取";
                ListView1.Visible = true;
                textBox1.Visible = false;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            refreash.RunWorkerAsync();
        }
    }
}