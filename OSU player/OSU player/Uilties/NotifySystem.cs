using System;
using System.Reflection;
using System.Windows.Forms;
using OSUplayer.Properties;

namespace OSUplayer.Uilties
{
    static internal class NotifySystem
    {
        private static readonly TaskBarIconMenu TrayIconMenuClass = new TaskBarIconMenu();
        private static readonly NotifyIcon TaskbarIcon = new NotifyIcon
        {
            Icon = Resources.icon,
            Text = LanguageManager.Get("OSUplayer"),
            Visible = true,
            ContextMenuStrip = TrayIconMenuClass.TrayIcon_Menu
        };
        private static EventHandler _clickEvent;
        public static void RegisterClick(EventHandler clicktodo)
        {
            TaskbarIcon.Click -= _clickEvent;
            TaskbarIcon.DoubleClick -= _clickEvent;
            _clickEvent = clicktodo;
            TaskbarIcon.Click += _clickEvent;
            TaskbarIcon.DoubleClick += _clickEvent;
        }
        public static void RegisterMenu(ContextMenuStrip menu)
        {
            TaskbarIcon.ContextMenuStrip = menu;
        }
        public static void Showtip(int time, string title, string content, ToolTipIcon icon = ToolTipIcon.Info, bool force = false)
        {
            if (Settings.Default.ShowPopup || force)
            {
                TaskbarIcon.ShowBalloonTip(time, title, content, icon);
            }
        }

        public static void ClearText()
        {
            QQ.Send2QQ("");
            TaskbarIcon.Text = LanguageManager.Get("OSUplayer");
        }
        private static void SetNotifyIconText(NotifyIcon ni, string text)
        {
            if (text.Length >= 128)
                text = text.Substring(0, 127);//throw new ArgumentOutOfRangeException("Text limited to 127 characters");
            var t = typeof(NotifyIcon);
            const BindingFlags hidden = BindingFlags.NonPublic | BindingFlags.Instance;
            t.GetField("text", hidden).SetValue(ni, text);
            if ((bool)t.GetField("added", hidden).GetValue(ni))
                t.GetMethod("UpdateIcon", hidden).Invoke(ni, new object[] { true });
        }
        public static void SetText(string content)
        {
            SetNotifyIconText(TaskbarIcon, String.Format("OSUPlayer\n{0}", content));
            QQ.Send2QQ(content);
            TrayIconMenuClass.RefreashMenu();
        }
    }
}