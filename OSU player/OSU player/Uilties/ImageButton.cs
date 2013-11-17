using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
namespace OSUplayer
{
    public partial class ImageButton
    {
        public ImageButton()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        [CategoryAttribute("外观"), DescriptionAttribute("EnterImage")]
        public Image EnterImage { get; set; }
        [CategoryAttribute("外观"), DescriptionAttribute("ClickImage")]
        public Image ClickImage { get; set; }
        [CategoryAttribute("外观"), DescriptionAttribute("BaseImage")]
        public Image BaseImage { get; set; }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Cursor.Position.X > Parent.Location.X + this.Location.X + this.Width * 0.1
                && Cursor.Position.Y > Parent.Location.Y + this.Height * 0.15
                && Cursor.Position.X < Parent.Location.X + this.Location.X + this.Width * 0.9
                && Cursor.Position.Y < Parent.Location.Y + this.Height * 0.85)
            {

                this.BackgroundImage = EnterImage;
                base.OnMouseMove(e);
            }
            else
            {
                this.BackgroundImage = BaseImage;
            }

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Cursor.Position.X > Parent.Location.X + this.Location.X + this.Width * 0.1
     && Cursor.Position.Y > Parent.Location.Y + this.Height * 0.15
     && Cursor.Position.X < Parent.Location.X + this.Location.X + this.Width * 0.9
     && Cursor.Position.Y < Parent.Location.Y + this.Height * 0.85)
            {
                this.BackgroundImage = ClickImage;
                base.OnMouseDown(e);
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (Parent != null)
            {
                if (Cursor.Position.X > Parent.Location.X + this.Location.X + this.Width * 0.1
        && Cursor.Position.Y > Parent.Location.Y + this.Height * 0.15
        && Cursor.Position.X < Parent.Location.X + this.Location.X + this.Width * 0.9
        && Cursor.Position.Y < Parent.Location.Y + this.Height * 0.85)
                {

                    base.OnMouseClick(e);
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {

                this.BackgroundImage = EnterImage;
            
            base.OnMouseUp(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            this.BackgroundImage = BaseImage;
            base.OnMouseLeave(e);
        }

    }
}
