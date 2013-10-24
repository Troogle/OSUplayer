using System.Collections.Generic;
using System;
using System.Windows.Forms;

namespace OSU_player
{
    public partial class Form2
    {
        public Form2()
        {
            InitializeComponent();

            //Added to support default instance behavour in C#
            if (defaultInstance == null)
                defaultInstance = this;
        }

        #region Default Instance

        private static Form2 defaultInstance;

        /// <summary>
        /// Added by the VB.Net to C# Converter to support default instance behavour in C#
        /// </summary>
        public static Form2 Default
        {
            get
            {
                if (defaultInstance == null)
                {
                    defaultInstance = new Form2();
                    defaultInstance.FormClosed += new FormClosedEventHandler(defaultInstance_FormClosed);
                }

                return defaultInstance;
            }
        }

        static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
        {
            defaultInstance = null;
        }

        #endregion
        public QQ qq = new QQ();
        List<QQInfo> tmp = new List<QQInfo>();
        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                Form1.Default.LabelQQ.Text = "当前同步QQ：" + ListView1.SelectedItems[0].Text;
                Core.uin = Convert.ToInt32(ListView1.SelectedItems[0].Text);
                this.Dispose();
            }
            catch (Exception)
            {
                if (ListView1.Items.Count != 0)
                {
                    MessageBox.Show("别卖萌不选啊-0-");
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            tmp = qq.GetQQList();
            ListView1.Clear();
            ListView1.Columns.Add("ID", 100);
            ListView1.Columns.Add("昵称", 100);
            try
            {

                foreach (QQInfo t in tmp)
                {
                    ListViewItem Tmp = new ListViewItem();
                    Tmp.Text = t.uin.ToString();
                    Tmp.SubItems.Add(t.nick);
                    ListView1.Items.Add(Tmp);
                }

            }
            catch (Exception)
            {
                MessageBox.Show("获取当前在线QQ出错！");
            }
        }
    }
}
