using System.Windows.Forms;
using OSUplayer.Properties;
namespace OSUplayer.Uilties
{
    static internal class NotifySystem
    {
        private static readonly NotifyIcon TaskbarIcon=new NotifyIcon
        {
            Icon=Resources.icon,
            Text="OSUplayer",
            Visible=true,
            
        };
        private static System.EventHandler _clickEvent;
        public static void RegisterClick(System.EventHandler clicktodo)
        {
            TaskbarIcon.Click -= _clickEvent;
            _clickEvent = clicktodo;
            TaskbarIcon.Click += _clickEvent;
        }
        public static void Showtip(int time,string title,string content,ToolTipIcon icon,bool force=false)
        {
            if (Settings.Default.ShowPopup||force)
            {
                TaskbarIcon.ShowBalloonTip(time,title,content,icon);
            }
        }
        public static void SetText(string content)
        {
            TaskbarIcon.Text = content;
        }
    }
}