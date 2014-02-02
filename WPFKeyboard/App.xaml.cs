using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace WPFKeyboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += OnStartup;
        }

        private void OnStartup(object sender, StartupEventArgs startupEventArgs)
        {
            var result = WPFKeyboardNative.KeyboardLayoutHelper.GetLayout(@"C:\Windows\SysWOW64\KBDUSA.DLL");
        }
    }
}
