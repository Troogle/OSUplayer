using OSUplayer.Uilties;
using System.ComponentModel;
using System.Windows.Forms;

namespace OSUplayer
{
    public partial class HintTextBox : TextBox
    {
        public HintTextBox()
        {
            InitializeComponent();
        }
        [Localizable(true)]
        public string Hint
        {
            get { return _hint; }
            set { _hint = value; Win32.UpdateTextBoxHint(this, _hint); }
        }

        private string _hint;
    }
}
