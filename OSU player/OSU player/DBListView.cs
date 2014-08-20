using System.Windows.Forms;

namespace OSUplayer
{
    public partial class DBListView : ListView
    {
        public DBListView()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }
    }
}
