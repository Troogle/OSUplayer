using System.Drawing;
using System.Windows.Forms;
using System;
using Telerik.WinControls;

namespace OSUplayer.Uilties
{
    class TaskBarIconMenu : IDisposable
    {

        public ContextMenuStrip TrayIcon_Menu;
        private ToolStripMenuItem TrayIcon_Artist;
        private ToolStripMenuItem TrayIcon_Title;
        private ToolStripMenuItem TrayIcon_Diff;
        private ToolStripMenuItem TrayIcon_Play;
        private ToolStripMenuItem TrayIcon_PlayNext;
        private ToolStripMenuItem TrayIcon_Exit;
        public void Dispose()
        {
            if (TrayIcon_Menu != null)
            {
                TrayIcon_Menu.Dispose();
                TrayIcon_Menu = null;
            }
            if (TrayIcon_Artist != null)
            {
                TrayIcon_Artist.Dispose();
                TrayIcon_Artist = null;
            }
            if (TrayIcon_Title != null)
            {
                TrayIcon_Title.Dispose();
                TrayIcon_Title = null;
            }
            if (TrayIcon_Diff != null)
            {
                TrayIcon_Diff.Dispose();
                TrayIcon_Diff = null;
            }
            if (TrayIcon_Play != null)
            {
                TrayIcon_Play.Dispose();
                TrayIcon_Play = null;
            }
            if (TrayIcon_PlayNext != null)
            {
                TrayIcon_PlayNext.Dispose();
                TrayIcon_PlayNext = null;
            }
            if (TrayIcon_Exit != null)
            {
                TrayIcon_Exit.Dispose();
                TrayIcon_Exit = null;
            }
        }
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
            // TrayIcon_Artist
            TrayIcon_Artist.Enabled = false;
            TrayIcon_Artist.Name = "TrayIcon_Artist";
            TrayIcon_Artist.Text = LanguageManager.Get("TrayIcon_Aritst_Text");
            TrayIcon_Artist.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            // TrayIcon_Title
            TrayIcon_Title.Enabled = false;
            TrayIcon_Title.Name = "TrayIcon_Title";
            TrayIcon_Title.Text = LanguageManager.Get("TrayIcon_Title_Text");
            TrayIcon_Title.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            // TrayIcon_Diff
            TrayIcon_Diff.Enabled = false;
            TrayIcon_Diff.Name = "TrayIcon_Diff";
            TrayIcon_Diff.Text = LanguageManager.Get("TrayIcon_Diff_Text");
            TrayIcon_Diff.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            // TrayIcon_Play
            TrayIcon_Play.Name = "TrayIcon_Play";
            TrayIcon_Play.Text = LanguageManager.Get("TrayIcon_Play_Pause_Text");
            TrayIcon_Play.Click += delegate { SendKeys.Send("%{F5}"); };
            // TrayIcon_PlayNext
            TrayIcon_PlayNext.Name = "TrayIcon_PlayNext";
            TrayIcon_PlayNext.Text = LanguageManager.Get("TrayIcon_PlayNext_Text");
            TrayIcon_PlayNext.Click += delegate { SendKeys.Send("%{RIGHT}"); };
            // TrayIcon_Exit
            TrayIcon_Exit.Name = "TrayIcon_Exit";
            TrayIcon_Exit.Text = LanguageManager.Get("TrayIcon_Exit_Text");
            TrayIcon_Exit.Click += delegate
            {
                if (
                    RadMessageBox.Show(LanguageManager.Get("Comfirm_Exit_Text"), LanguageManager.Get("Tip_Text"),
                        MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                Core.MainIsVisible = false;
                Core.Exit();
                Application.ExitThread();
                Application.Exit();
            };
            TrayIcon_Menu.ResumeLayout(false);
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