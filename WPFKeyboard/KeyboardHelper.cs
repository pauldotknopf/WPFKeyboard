using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using WindowsInput.Native;

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

        public static string GetKeyNameFromVirtualKey(IntPtr keyboardLayout, WindowsInput.Native.VirtualKeyCode virtualKey, int modifierState, Dictionary<VirtualKeyCode, int> modifierKeys, bool capsAffectsShift)
        {
            var keyState = new byte[256];

            if (!NativeMethods.GetKeyboardState(keyState))
                return null;

            foreach (var modBit in modifierKeys)
            {
                if ((modifierState & modBit.Value) == modBit.Value)
                {
                    keyState[(int)modBit.Key] = 129;
                }
            }

            var character = new StringBuilder(10);
            var result = NativeMethods.ToUnicodeEx((uint)virtualKey, 0, keyState, character, character.Capacity, 0, keyboardLayout);

            // If unshifter was a dead key, so will be shifted.
            if (result < 0)
            {
                int dummy = NativeMethods.ToUnicodeEx(
                    (uint)VirtualKeyCode.SPACE,
                    NativeMethods.MapVirtualKeyEx((uint)VirtualKeyCode.SPACE, 0, keyboardLayout),
                    keyState,
                    character,
                    character.Capacity,
                    0,
                    keyboardLayout);

                // There will be one character stored in our buffer though:
                // (well, at least one, but we have no way of knowing if more)
                result = 1;
            }

            return character.ToString();
        }
    }
}
