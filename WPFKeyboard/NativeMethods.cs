using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WPFKeyboard
{
    public class NativeMethods
    {
        private NativeMethods() { }

        #region Forms

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public extern static void LockWindowUpdate(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public extern static IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern Int32 GetDeviceCaps(IntPtr hdc, Int32 capindex);


        #endregion

        #region AppMutex

        public const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern int IsIconic(IntPtr hWnd);

        #endregion

        #region Keyboard

        [DllImport("user32.dll", CharSet = CharSet.Unicode,
            EntryPoint = "MapVirtualKeyExW", ExactSpelling = true)]
        public static extern uint MapVirtualKeyEx(
            uint uCode,
            uint uMapType,
            IntPtr dwhkl);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetKeyboardLayout(int idThread);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int GetKeyboardLayoutList(int nBuff, [Out, MarshalAs(UnmanagedType.LPArray)] int[] lpList);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "LoadKeyboardLayoutW", ExactSpelling = true)]
        public static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnloadKeyboardLayout(IntPtr hkl);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, ThrowOnUnmappableChar = true)]
        public static extern int ToUnicodeEx(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags,
            IntPtr hkl);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetKeyboardLayoutName([Out] StringBuilder pwszKLID);

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint SHLoadIndirectString(string pszSource, StringBuilder pszOutBuf, uint cchOutBuf, IntPtr ppvReserved);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        // Can be used to get loaclised names for modifier keys :: L10N
        // 	[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        // 	public static extern int GetKeyNameText(uint lParam, [Out] StringBuilder lpString, int nSize);

        public const int KEYEVENTF_EXTENDEDKEY = 0x1;
        public const int KEYEVENTF_KEYUP = 0x2;
        public const int KL_NAMELENGTH = 9;
        public const int KLF_ACTIVATE = 0x00000001;
        public const uint KLF_NOTELLSHELL = 0x00000080;
        public const uint KLF_SUBSTITUTE_OK = 0x00000002;

        #endregion

        #region Registry

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        public static extern int RegOpenKeyEx
            (UIntPtr hKey,
            string lpSubKey,
            uint ulOptions,
            int samDesired,
            out UIntPtr phkResult);

        [DllImport("advapi32.dll")]
        public extern static uint RegQueryInfoKey
            (UIntPtr hkey,
            IntPtr lpClass,
            IntPtr lpcbClass,
            IntPtr lpReserved,
            IntPtr lpcSubKeys,
            IntPtr lpcbMaxSubKeyLen,
            IntPtr lpcbMaxClassLen,
            IntPtr lpcValues,
            IntPtr lpcbMaxValueNameLen,
            IntPtr lpcbMaxValueLen,
            IntPtr lpcbSecurityDescriptor,
            out Int64 lpftLastWriteTime);

        [DllImport("Advapi32.dll")]
        public static extern uint RegCloseKey(UIntPtr hKey);

        #endregion

        #region KeyPictureBox

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        #endregion

        [DllImport("comctl32.dll", CharSet = CharSet.Unicode, EntryPoint = "ImplementsTaskDialog")]
        public static extern int TaskDialog(IntPtr hWndParent, IntPtr hInstance, String pszWindowTitle, String pszMainInstruction, String pszContent, int dwCommonButtons, IntPtr pszIcon, out int pnButton);


    }
}
