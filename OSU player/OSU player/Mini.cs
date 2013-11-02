using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace OSU_player
{
    public partial class Mini : Form
    {
        Bitmap bg;
        public Mini()
        {
            InitializeComponent();
            bg = new Bitmap(pictureBox1.Image);
        }
        private void Mini_Load(object sender, EventArgs e)
        {
            set(false);
        }
        private Bitmap Resize_Draw(Bitmap bmp, Bitmap Front)
        {
            try
            {
                Bitmap b = new Bitmap(300, 300);
                bmp.MakeTransparent();
                Front.MakeTransparent();
                b.MakeTransparent();
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, 300, 300), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.DrawImage(Front, new Rectangle(0, 0, Front.Width, Front.Height),
    new Rectangle(0, 0, Front.Width, Front.Height), GraphicsUnit.Pixel);
                g.DrawImage(Front, new Rectangle(0, 240, Front.Width, Front.Height),
new Rectangle(0, 0, Front.Width, Front.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }
        private void set(bool vis)
        {
            if (vis)
            {
                pictureBox1.Image = Resize_Draw(bg, new Bitmap(Properties.Resources.Front));
            }
            else
            {
                pictureBox1.Image = bg;
            }
            this.button1.Visible = vis;
            this.button2.Visible = vis;
            this.button3.Visible = vis;
            this.button4.Visible = vis;
            this.button5.Visible = vis;
            this.label1.Visible = vis;
            this.label2.Visible = vis;

        }
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            set(false);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            set(true);
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            return;
        }


    }
}
