using System;
using System.Drawing;
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


    class TaskBarIconMenu
    {

        public ContextMenuStrip TrayIcon_Menu;
        private ToolStripMenuItem TrayIcon_Artist;
        private ToolStripMenuItem TrayIcon_Title;
        private ToolStripMenuItem TrayIcon_Diff;
        private ToolStripMenuItem TrayIcon_Play;
        private ToolStripMenuItem TrayIcon_PlayNext;
        private ToolStripMenuItem TrayIcon_Exit;
        public TaskBarIconMenu()
        {
            TrayIcon_Menu = new ContextMenuStrip();
            TrayIcon_Artist = new ToolStripMenuItem();
            TrayIcon_Title = new ToolStripMenuItem();
            TrayIcon_Diff = new ToolStripMenuItem();
            TrayIcon_Play = new ToolStripMenuItem();
            TrayIcon_PlayNext = new ToolStripMenuItem();
            TrayIcon_Exit = new ToolStripMenuItem();
            TrayIcon_Menu.SuspendLayout();

            TrayIcon_Menu.Items.AddRange(new ToolStripItem[] {
            TrayIcon_Artist,
            TrayIcon_Title,
            TrayIcon_Diff,
            TrayIcon_Play,
            TrayIcon_PlayNext,
            TrayIcon_Exit});
            TrayIcon_Menu.Name = "TrayIcon_Menu";
            TrayIcon_Menu.Size = new Size(176, 176);
            // 
            // TrayIcon_Artist
            // 
            TrayIcon_Artist.Enabled = false;
            TrayIcon_Artist.Name = "TrayIcon_Artist";
            TrayIcon_Artist.Size = new Size(175, 24);
            TrayIcon_Artist.Text = LanguageManager.Get("TrayIcon_Aritst_Text");
            TrayIcon_Artist.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            // 
            // TrayIcon_Title
            // 
            TrayIcon_Title.Enabled = false;
            TrayIcon_Title.Name = "TrayIcon_Title";
            TrayIcon_Title.Size = new Size(175, 24);
            TrayIcon_Title.Text = LanguageManager.Get("TrayIcon_Title_Text");
            TrayIcon_Title.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            // 
            // TrayIcon_Diff
            // 
            TrayIcon_Diff.Enabled = false;
            TrayIcon_Diff.Name = "TrayIcon_Diff";
            TrayIcon_Diff.Size = new Size(175, 24);
            TrayIcon_Diff.Text = LanguageManager.Get("TrayIcon_Diff_Text");
            TrayIcon_Diff.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            // 
            // TrayIcon_Play
            // 
            TrayIcon_Play.Name = "TrayIcon_Play";
            TrayIcon_Play.Size = new Size(175, 24);
            TrayIcon_Play.Text = LanguageManager.Get("TrayIcon_Play_Pause_Text");
            TrayIcon_Play.Click += TrayIcon_Play_Click;
            // 
            // TrayIcon_PlayNext
            // 
            TrayIcon_PlayNext.Name = "TrayIcon_PlayNext";
            TrayIcon_PlayNext.Size = new Size(175, 24);
            TrayIcon_PlayNext.Text = LanguageManager.Get("TrayIcon_PlayNext_Text");
            TrayIcon_PlayNext.Click += TrayIcon_PlayNext_Click;
            // 
            // TrayIcon_Exit
            // 
            TrayIcon_Exit.Enabled = false;
            TrayIcon_Exit.Name = "TrayIcon_Exit";
            TrayIcon_Exit.Size = new Size(175, 24);
            TrayIcon_Exit.Text = LanguageManager.Get("TrayIcon_Exit_Text");
            TrayIcon_Menu.ResumeLayout(false);
        }

        static void TrayIcon_PlayNext_Click(object sender, EventArgs e)
        {
            SendKeys.Send("%{RIGHT}");
        }

        static void TrayIcon_Play_Click(object sender, EventArgs e)
        {
            SendKeys.Send("%{F5}");
        }
        public void RefreashMenu()
        {
            if (Core.CurrentBeatmap == null) return;
            TrayIcon_Artist.Text = LanguageManager.Get("TrayIcon_Aritst_Text") + Core.CurrentBeatmap.Artist;
            TrayIcon_Title.Text = LanguageManager.Get("TrayIcon_Title_Text") + Core.CurrentBeatmap.Title;
            TrayIcon_Diff.Text = LanguageManager.Get("TrayIcon_Diff_Text") + Core.CurrentBeatmap.Version;
        }
    }
}