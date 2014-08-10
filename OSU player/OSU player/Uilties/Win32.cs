using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OSUplayer.Uilties
{
    [Flags]
    public enum KeyModifiers
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8
    }
    public delegate void HotkeyEventHandler(int hotKeyID);
    static class Win32
    {
        #region 删除API
        private const int FO_DELETE = 0x3;
        private const ushort FOF_ALLOWUNDO = 0x40;
        private const ushort FOF_WANTNUKEWARNING = 0x4000;
        [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int SHFileOperation([In, Out] _SHFILEOPSTRUCT str);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private class _SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public UInt32 wFunc;
            public string pFrom;
            public string pTo;
            public UInt16 fFlags;
            public Int32 fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }
        public static int Delete(List<string> path)
        {
            _SHFILEOPSTRUCT pm = new _SHFILEOPSTRUCT();
            pm.wFunc = FO_DELETE;
            pm.pFrom = path[0];
            for (int i = 1; i < path.Count; i++)
            {
                pm.pFrom += '\0' + path[i];
            }
            pm.pFrom += '\0';
            pm.pTo = null;
            pm.fFlags = FOF_ALLOWUNDO | FOF_WANTNUKEWARNING;
            return SHFileOperation(pm);
        }
        #endregion
        #region 热键API
        [DllImport("user32.dll")]
        public static extern UInt32 RegisterHotKey(IntPtr hWnd, UInt32 id, UInt32 fsModifiers, UInt32 vk);
        [DllImport("user32.dll")]
        public static extern UInt32 UnregisterHotKey(IntPtr hWnd, UInt32 id);
        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalAddAtom(String lpString);
        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalDeleteAtom(UInt32 nAtom);
        #endregion
        #region 窗体API
        [DllImport("gdi32.dll")]
        private static extern int CreateRoundRectRgn(int x1, int y1, int x2, int y2, int x3, int y3);
        [DllImport("user32.dll")]
        private static extern int SetWindowRgn(IntPtr hwnd, int hRgn, Boolean bRedraw);
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject", CharSet = CharSet.Ansi)]
        private static extern int DeleteObject(int hObject);
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        // ReSharper disable InconsistentNaming
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HTCAPTION = 2;
        // ReSharper restore InconsistentNaming
        [DllImport("User32.dll")]
        public static extern bool PtInRect(ref Rectangle r, Point p);
        /// <summary>
        /// 设置窗体的圆角矩形
        /// </summary>
        /// <param name="form">需要设置的窗体</param>
        /// <param name="rgnRadius">圆角矩形的半径</param>
        public static void SetFormRoundRectRgn(Form form, int rgnRadius)
        {
            var hRgn = 0;
            hRgn = CreateRoundRectRgn(0, 0, form.Width + 1, form.Height + 1, rgnRadius, rgnRadius);
            SetWindowRgn(form.Handle, hRgn, true);
            DeleteObject(hRgn);
        }
        public static void MoveWindow(Form form)
        {
            ReleaseCapture();
            SendMessage(form.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }
        #endregion
    }
    public class HotkeyHelper : IMessageFilter
    {

        private Hashtable keyIDs = new Hashtable();
        private IntPtr hWnd;
        public event HotkeyEventHandler OnHotkey;

        public HotkeyHelper(IntPtr hWnd)
        {
            this.hWnd = hWnd;
            Application.AddMessageFilter(this);
        }
        public int RegisterHotkey(Keys key, KeyModifiers keyflags)
        {
            UInt32 hotkeyid = Win32.GlobalAddAtom(Guid.NewGuid().ToString());
            Win32.RegisterHotKey((IntPtr)hWnd, hotkeyid, (UInt32)keyflags, (UInt32)key);
            keyIDs.Add(hotkeyid, hotkeyid);
            return (int)hotkeyid;
        }
        public void UnregisterHotkeys()
        {
            //Application.RemoveMessageFilter(this);
            foreach (UInt32 key in keyIDs.Values)
            {
                Win32.UnregisterHotKey(hWnd, key);
                Win32.GlobalDeleteAtom(key);
            }
            keyIDs.Clear();
            OnHotkey = null;
        }
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != 0x312) return false;
            if (OnHotkey == null) return false;
            foreach (UInt32 key in keyIDs.Values)
            {
                if ((UInt32) m.WParam != key) continue;
                OnHotkey((int)m.WParam);
                return true;
            }
            return false;
        }
    }
}