using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OSUplayer.Uilties;

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
