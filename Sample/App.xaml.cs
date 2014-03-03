using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput.Native;
using Microsoft.Win32;
using WPFKeyboard;
using WPFKeyboardNative;

namespace Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public const int KEYEVENTF_EXTENDEDKEY = 0x1;
        public const int KEYEVENTF_KEYUP = 0x2;
        public const int KL_NAMELENGTH = 9;
        public const int KLF_ACTIVATE = 0x00000001;
        public const uint KLF_NOTELLSHELL = 0x00000080;
        public const uint KLF_SUBSTITUTE_OK = 0x00000002;

        public App()
        {
            Startup += OnStartup;
        }

        private void OnStartup(object sender, StartupEventArgs startupEventArgs)
        {
            var result =  System.Windows.Forms.InputLanguage.InstalledInputLanguages;
            //var layout = KeyboardLayoutHelper.GetLayout(@"C:\Windows\SysWOW64\KBDJPN.DLL");

            //KeyboardHelper.GetInstalledKeyboardList();
            //var identifier = NativeMethods.LoadKeyboardLayout(KeyboardHelper.InstalledKeyboardLayouts["Japanese"].Locale, NativeMethods.KLF_ACTIVATE | NativeMethods.KLF_SUBSTITUTE_OK);

            //var state = new ModiferState(layout.CharModifiers.Select(x => (VirtualKeyCode)x.VirtualKey).ToList());

            //Task.Factory.StartNew(() =>
            //{
            //    while (true)
            //    {
            //        state.Refresh();
            //        foreach (var key in state.GetVirtualKeys())
            //        {
            //            Debug.WriteLine(key + ":" + state.GetModifierState(key));
            //        }
            //        Thread.Sleep(1000);
            //    }
            //});

            //foreach (var modifier in layout.CharModifiers)
            //{
            //    var keyState = new byte[256];
            //    //keyState[modifier.VirtualKey] = 0x80;
            //    keyState[(int)VirtualKeyCode.KANA] = 0x80;
            //    keyState[(int)VirtualKeyCode.SHIFT] = 0x80;
            //    //keyState[(int) VirtualKeyCode.SHIFT] = 0x80;
            //    const int bufferLength = 10;

            //    // Get the key itself:
            //    var character = new StringBuilder(bufferLength);

            //    var result = NativeMethods.ToUnicodeEx((uint)VirtualKeyCode.VK_A, 0, keyState, character,
            //        character.Capacity, 0, identifier);
            //}
        }
    }
}
