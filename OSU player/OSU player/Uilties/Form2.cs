using System.Collections.Generic;
using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls;
using System.ComponentModel;
using OSU_player.Uilties;
namespace OSU_player
{
    public partial class Form2 : RadForm
    {
        public Form2()
        {
            InitializeComponent();
            refreash.DoWork += refreash_DoWork;
        }
        public QQ qq = new QQ();
        List<QQInfo> tmp = new List<QQInfo>();
        private void Button2_Click(object sender, EventArgs e)
        {
            if (ListView1.Visible)
            {
                try
                {
                    Core.uin = ListView1.SelectedItems[0].Text;
                    if (Core.uin == "清空QQ号") { Core.uin = "0"; }
                    this.Dispose();
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
                        this.Dispose();
                    }
                }
            }
            else
            {
                Int64 tmp = new Int64();
                if ((!Int64.TryParse(textBox1.Text, out tmp)) || (textBox1.Text.Trim() == ""))
                {
                    RadMessageBox.Show("亲正常点~请输入QQ号~~");
                }
                else
                {
                    Core.uin = textBox1.Text;
                    if (Core.uin == "0") { Core.syncQQ = false; } else { Core.syncQQ = true; }
                    this.Dispose();
                }
            }
        }
        private void RefreshQQ(object sender, EventArgs e)
        {
            this.Focus();
            ListView1.Clear();
            ListView1.Columns.Add("ID", 100);
            ListView1.Columns.Add("昵称", 100);
            tmp = qq.GetQQList;
            try
            {
                foreach (QQInfo t in tmp)
                {
                    ListViewItem Tmp = new ListViewItem();
                    Tmp.Text = t.uin.ToString();
                    Tmp.SubItems.Add(t.nick);
                    ListView1.Items.Add(Tmp);
                }
                ListViewItem Tmpl = new ListViewItem();
                Tmpl.Text = "清空QQ号";
                Tmpl.SubItems.Add("");
                ListView1.Items.Add(Tmpl);
            }
            catch (Exception)
            {
                RadMessageBox.Show("获取当前在线QQ出错！");
            }
        }
        BackgroundWorker refreash = new BackgroundWorker();
        void refreash_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(RefreshQQ));
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
