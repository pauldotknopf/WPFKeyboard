using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WindowsInput.Native;
using Microsoft.Win32;

namespace WPFKeyboard
{
    public class KeyboardHelper
    {
        public static Dictionary<string, InstalledKeyboardLayout> InstalledKeyboardLayouts { get; private set; }

        static KeyboardHelper()
        {
            InstalledKeyboardLayouts = new Dictionary<string, InstalledKeyboardLayout>();
            GetInstalledKeyboardList();
        }

        public static IEnumerable<string> GetInstalledKeyboardListInNameOrder()
        {
            var results = new string[InstalledKeyboardLayouts.Count];
            InstalledKeyboardLayouts.Keys.CopyTo(results, 0);
            Array.Sort(results);
            return results;
        }

        private static void GetInstalledKeyboardList()
        {
            InstalledKeyboardLayouts.Clear();

            var registry = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Keyboard Layouts");

            if (registry == null)
                return;

            foreach (var locale in registry.GetSubKeyNames())
            {
                var layout = BuildKeyboardLayout(locale);
                InstalledKeyboardLayouts.Add(layout.LayoutText, layout);
            }
        }

        private static InstalledKeyboardLayout BuildKeyboardLayout(string locale)
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Keyboard Layouts\" + locale);

            if (key == null)
                throw new Exception("Couldn't open key layout for locale " + locale);

            var layoutText = key.GetValue("Layout Text").ToString();
            var layoutDisplayName = key.GetValue("Layout Display Name", "").ToString();
            var layoutFile = key.GetValue("Layout File").ToString();

            if (!string.IsNullOrEmpty(layoutDisplayName))
            {
                var sbName = new StringBuilder(260);
                layoutDisplayName = NativeMethods.SHLoadIndirectString(layoutDisplayName, sbName, (uint)sbName.Capacity, IntPtr.Zero) == 0
                                    ? sbName.ToString()
                                    : layoutText;
            }
            else
                layoutDisplayName = layoutText;

            return new InstalledKeyboardLayout(locale, layoutDisplayName, layoutFile, layoutText);
        }
    }
}
