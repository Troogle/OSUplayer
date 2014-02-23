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
            About_Content.Text = string.Format(About_Content.Text, Core.Version);
            About_Profile_Hint.SendToBack();
        }
        private void About_Program_Link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/Troogle/OSUplayer/");
        }
        private void About_PictureBox_Click(object sender, EventArgs e)
        {
            Process.Start("https://osu.ppy.sh/u/3281474");
        }
    }
}
