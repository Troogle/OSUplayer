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