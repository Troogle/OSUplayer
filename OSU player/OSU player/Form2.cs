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
        List<QQ.QQInfo> @ref = new List<QQ.QQInfo>();

        private void Button1_Click(object sender, EventArgs e)
        {
            @ref = qq.GetQQList();
            ListView1.Clear();
            ListView1.Columns.Add("ID", 100);
            ListView1.Columns.Add("昵称", 100);
            try
            {
                for (var i = 0; i <= @ref.Count - 1; i++)
                {
                    ListViewItem Tmp = new ListViewItem();
                    Tmp.Text = (string)(@ref[System.Convert.ToInt32(i)].uin.ToString());
                    Tmp.SubItems.Add(@ref[System.Convert.ToInt32(i)].nick);
                    ListView1.Items.Add(Tmp);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("获取当前在线QQ出错！");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                Form1.Default.LabelQQ.Text = "当前同步QQ：" + ListView1.SelectedItems[0].Text;
                Form1.Default.uin = ListView1.SelectedItems[0].Text;
                this.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("别卖萌不选啊-0-");
            }
        }
    }
}
