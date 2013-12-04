using System;
using System.Diagnostics;
using System.Windows.Forms;
using Telerik.WinControls.UI;
namespace OSUplayer.Uilties
{
    public partial class About : RadForm
    {
        public About()
        {
            InitializeComponent();
        }
        private void About_Load(object sender, EventArgs e)
        {
            label2.Text = string.Format("OSU Player Ver {0}\n{1}", Core.Version, label2.Text);
            label1.SendToBack();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/Troogle/OSUplayer/");
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://osu.ppy.sh/u/3281474");
        }
    }
}
