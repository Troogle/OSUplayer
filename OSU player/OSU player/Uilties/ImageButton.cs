using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
namespace OSU_player
{
    public partial class ImageButton
    {
        public ImageButton()
        {
            InitializeComponent();
            BaseImage = BackgroundImage;
        }
        [CategoryAttribute("外观"), DescriptionAttribute("EnterImage")]
        public Image EnterImage { get; set; }
        [CategoryAttribute("外观"), DescriptionAttribute("ClickImage")]
        public Image ClickImage { get; set; }
        public Image BaseImage { get; set; }
        protected override void OnMouseEnter(EventArgs e)
        {
            this.BackgroundImage = EnterImage;
            base.OnMouseEnter(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.BackgroundImage = ClickImage;
            base.OnMouseDown(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            this.BackgroundImage = BaseImage;
            base.OnMouseLeave(e);
        }
    }
}
