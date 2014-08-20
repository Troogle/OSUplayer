using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
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
