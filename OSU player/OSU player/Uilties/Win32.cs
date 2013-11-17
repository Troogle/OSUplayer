using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OSU_player.Uilties
{
    [Flags()]
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
    }
    public class HotkeyHelper : IMessageFilter
    {

        Hashtable keyIDs = new Hashtable();
        IntPtr hWnd;
        public event HotkeyEventHandler OnHotkey;

        public HotkeyHelper(IntPtr hWnd)
        {
            this.hWnd = hWnd;
            Application.AddMessageFilter(this);
        }
        public int RegisterHotkey(Keys Key, KeyModifiers keyflags)
        {
            UInt32 hotkeyid = Win32.GlobalAddAtom(System.Guid.NewGuid().ToString());
            Win32.RegisterHotKey((IntPtr)hWnd, hotkeyid, (UInt32)keyflags, (UInt32)Key);
            keyIDs.Add(hotkeyid, hotkeyid);
            return (int)hotkeyid;
        }
        public void UnregisterHotkeys()
        {
            Application.RemoveMessageFilter(this);
            foreach (UInt32 key in keyIDs.Values)
            {
                Win32.UnregisterHotKey(hWnd, key);
                Win32.GlobalDeleteAtom(key);
            }
        }
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x312)
            {
                if (OnHotkey != null)
                {
                    foreach (UInt32 key in keyIDs.Values)
                    {
                        if ((UInt32)m.WParam == key)
                        {
                            OnHotkey((int)m.WParam);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}