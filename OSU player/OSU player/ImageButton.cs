using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace OSUplayer
{
    public partial class ImageButton
    {
        public ImageButton()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        [Category("外观"), Description("EnterImage")]
        public Image EnterImage { get; set; }
        [Category("外观"), Description("ClickImage")]
        public Image ClickImage { get; set; }
        [Category("外观"), Description("BaseImage")]
        public Image BaseImage { get; set; }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Cursor.Position.X > Parent.Location.X + Location.X + Width * 0.1
                && Cursor.Position.Y > Parent.Location.Y + Height * 0.15
                && Cursor.Position.X < Parent.Location.X + Location.X + Width * 0.9
                && Cursor.Position.Y < Parent.Location.Y + Height * 0.85)
            {

                BackgroundImage = EnterImage;
                base.OnMouseMove(e);
            }
            else
            {
                BackgroundImage = BaseImage;
            }

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Cursor.Position.X > Parent.Location.X + Location.X + Width * 0.1
     && Cursor.Position.Y > Parent.Location.Y + Height * 0.15
     && Cursor.Position.X < Parent.Location.X + Location.X + Width * 0.9
     && Cursor.Position.Y < Parent.Location.Y + Height * 0.85)
            {
                BackgroundImage = ClickImage;
                base.OnMouseDown(e);
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (Parent != null)
            {
                if (Cursor.Position.X > Parent.Location.X + Location.X + Width * 0.1
        && Cursor.Position.Y > Parent.Location.Y + Height * 0.15
        && Cursor.Position.X < Parent.Location.X + Location.X + Width * 0.9
        && Cursor.Position.Y < Parent.Location.Y + Height * 0.85)
                {

                    base.OnMouseClick(e);
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {

                BackgroundImage = EnterImage;
            
            base.OnMouseUp(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            BackgroundImage = BaseImage;
            base.OnMouseLeave(e);
        }

    }
}
