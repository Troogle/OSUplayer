using System.Windows.Forms;

namespace OSUplayer
{
    public partial class NoFocusButton : Button
    {
        public NoFocusButton()
        {
            InitializeComponent();
        }
        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }  
    }
}
