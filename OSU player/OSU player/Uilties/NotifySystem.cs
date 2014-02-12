using System;
using System.Windows.Forms;
using OSUplayer.Properties;
using System.Runtime.InteropServices;

namespace OSUplayer.Uilties
{
    static internal class NotifySystem
    {
        private static readonly TaskBarIconMenu TrayIcon_MenuClass = new TaskBarIconMenu();
        private static readonly NotifyIcon TaskbarIcon = new NotifyIcon
        {
            Icon = Resources.icon,
            Text = "OSUplayer",
            Visible = true,
            ContextMenuStrip = TrayIcon_MenuClass.TrayIcon_Menu
        };
        private static System.EventHandler _clickEvent;
        public static void RegisterClick(System.EventHandler clicktodo)
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
        public static void Showtip(int time, string title, string content, ToolTipIcon icon, bool force = false)
        {
            if (Settings.Default.ShowPopup || force)
            {
                TaskbarIcon.ShowBalloonTip(time, title, content, icon);
            }
        }

        public static void ClearText()
        {
            QQ.Send2QQ("");
            TaskbarIcon.Text = "OSUPlayer";
        }
        public static void SetText(string content)
        {
            TaskbarIcon.Text = String.Format("OSUPlayer\n{0}", content);
            QQ.Send2QQ(content);
            TrayIcon_MenuClass.RefreashMenu();
        }
    }


    class TaskBarIconMenu
    {

        public System.Windows.Forms.ContextMenuStrip TrayIcon_Menu;
        private System.Windows.Forms.ToolStripMenuItem TrayIcon_Artist;
        private System.Windows.Forms.ToolStripMenuItem TrayIcon_Title;
        private System.Windows.Forms.ToolStripMenuItem TrayIcon_Diff;
        private System.Windows.Forms.ToolStripMenuItem TrayIcon_Play;
        private System.Windows.Forms.ToolStripMenuItem TrayIcon_PlayNext;
        private System.Windows.Forms.ToolStripMenuItem TrayIcon_Exit;
        public TaskBarIconMenu()
        {
            this.TrayIcon_Menu = new System.Windows.Forms.ContextMenuStrip();
            this.TrayIcon_Artist = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayIcon_Title = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayIcon_Diff = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayIcon_Play = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayIcon_PlayNext = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayIcon_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayIcon_Menu.SuspendLayout();

            this.TrayIcon_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TrayIcon_Artist,
            this.TrayIcon_Title,
            this.TrayIcon_Diff,
            this.TrayIcon_Play,
            this.TrayIcon_PlayNext,
            this.TrayIcon_Exit});
            this.TrayIcon_Menu.Name = "TrayIcon_Menu";
            this.TrayIcon_Menu.Size = new System.Drawing.Size(176, 176);
            // 
            // TrayIcon_Artist
            // 
            this.TrayIcon_Artist.Enabled = false;
            this.TrayIcon_Artist.Name = "TrayIcon_Artist";
            this.TrayIcon_Artist.Size = new System.Drawing.Size(175, 24);
            this.TrayIcon_Artist.Text = "当前Aritst";
            this.TrayIcon_Artist.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // TrayIcon_Title
            // 
            this.TrayIcon_Title.Enabled = false;
            this.TrayIcon_Title.Name = "TrayIcon_Title";
            this.TrayIcon_Title.Size = new System.Drawing.Size(175, 24);
            this.TrayIcon_Title.Text = "当前Title";
            this.TrayIcon_Title.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // TrayIcon_Diff
            // 
            this.TrayIcon_Diff.Enabled = false;
            this.TrayIcon_Diff.Name = "TrayIcon_Diff";
            this.TrayIcon_Diff.Size = new System.Drawing.Size(175, 24);
            this.TrayIcon_Diff.Text = "当前Diff";
            this.TrayIcon_Diff.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // TrayIcon_Play
            // 
            this.TrayIcon_Play.Name = "TrayIcon_Play";
            this.TrayIcon_Play.Size = new System.Drawing.Size(175, 24);
            this.TrayIcon_Play.Text = "播放/暂停";
            this.TrayIcon_Play.Click += TrayIcon_Play_Click;
            // 
            // TrayIcon_PlayNext
            // 
            this.TrayIcon_PlayNext.Name = "TrayIcon_PlayNext";
            this.TrayIcon_PlayNext.Size = new System.Drawing.Size(175, 24);
            this.TrayIcon_PlayNext.Text = "下一首";
            this.TrayIcon_PlayNext.Click += TrayIcon_PlayNext_Click;
            // 
            // TrayIcon_Exit
            // 
            this.TrayIcon_Exit.Enabled = false;
            this.TrayIcon_Exit.Name = "TrayIcon_Exit";
            this.TrayIcon_Exit.Size = new System.Drawing.Size(175, 24);
            this.TrayIcon_Exit.Text = "退出";

            this.TrayIcon_Menu.ResumeLayout(false);
        }
        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        private static extern void keybd_event(
        byte bVk, //虚拟键值  
        byte bScan,// 一般为0  
        int dwFlags, //这里是整数类型 0 为按下，2为释放  
        int dwExtraInfo //这里是整数类型 一般情况下设成为0  
        );
        void TrayIcon_PlayNext_Click(object sender, EventArgs e)
        {
            keybd_event(176, 0, 0, 0);
            keybd_event(176, 0, 2, 0);
        }

        void TrayIcon_Play_Click(object sender, EventArgs e)
        {
            keybd_event(179, 0, 0, 0);
            keybd_event(179, 0, 2, 0);
        }
        public void RefreashMenu()
        {
            if (Core.CurrentBeatmap == null) return;
            this.TrayIcon_Artist.Text = "Artist:" + Core.CurrentBeatmap.Artist;
            this.TrayIcon_Title.Text = "Title:" + Core.CurrentBeatmap.Title;
            this.TrayIcon_Diff.Text = "Diff:" + Core.CurrentBeatmap.Version;
        }
    }
}