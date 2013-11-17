using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
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
            label2.Text = "OSU Player Ver " + Core.Version + "\n" + label2.Text;
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
