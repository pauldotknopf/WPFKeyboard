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

        public static string GetKeyNameFromVirtualKey(IntPtr keyboardLayout, VirtualKeyCode virtualKey, ModiferState modifierState, bool capsAffectsShift)
        {
            var keyState = new byte[256];
            foreach (var modifierVirtualKey in modifierState.GetVirtualKeys())
            {
                if (modifierVirtualKey == VirtualKeyCode.CAPITAL) continue;

                if (modifierVirtualKey == VirtualKeyCode.SHIFT && capsAffectsShift)
                {
                    if (modifierState.GetModifierState(VirtualKeyCode.CAPITAL))
                    {
                        keyState[(int)modifierVirtualKey] = (byte)(modifierState.GetModifierState(modifierVirtualKey) ? 0x00 : 0x80);
                    }
                    else
                    {
                        keyState[(int)modifierVirtualKey] = (byte)(modifierState.GetModifierState(modifierVirtualKey) ? 0x80 : 0x00);
                    }
                }
                else
                {
                    if (modifierState.GetModifierState(modifierVirtualKey))
                        keyState[(int)modifierVirtualKey] = 0x80;
                    else
                        keyState[(int)modifierVirtualKey] = 0x00;
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

        //public static string GetKeyName(int scancode, ref bool overlong)
        //{
        //    byte[] keyState = new byte[256];

        //    // Set all the IME bits on (only works for Japanese = Korean & Chinese still don't work..?)
        //    //keyState[(int)Keys.KanaMode] = 0x00;
        //    //keyState[(int)Keys.HanguelMode] = 0x00;
        //    //keyState[(int)Keys.JunjaMode] = 0x00;
        //    //keyState[(int)Keys.FinalMode] = 0x00;
        //    //keyState[(int)Keys.HanjaMode] = 0x00;
        //    //keyState[(int)Keys.KanjiMode] = 0x00;

        //    StringBuilder result = new StringBuilder();

        //    const int bufferLength = 10;

        //    // Get the key itself:
        //    StringBuilder sbUnshifted = new StringBuilder(bufferLength);

        //    uint vk = NativeMethods.MapVirtualKeyEx((uint)scancode, 1, _currentInputLocaleIdentifier);
        //    // 	Console.WriteLine((Keys)vk + " - " + vk.ToString() + " - " + "Scancode: " + scancode.ToString());

        //    int rc = NativeMethods.ToUnicodeEx(vk, (uint)scancode, keyState, sbUnshifted, sbUnshifted.Capacity, 0, _currentInputLocaleIdentifier);

        //    if (rc > 1)
        //    {
        //        // this is an out parameter: many unicode glyphs are more than one "character"
        //        overlong = true;
        //    }

        //    if (rc < 0)
        //    {
        //        // This is a dead key - a key which only combines with the next pressed to form an accent etc.
        //        // In order to stop it combining with the shifted state, we need to flush out what's stored in the keyboard state
        //        // by calling the function again now.
        //        // ref: http://blogs.msdn.com/michkap/archive/2006/03/24/559169.aspx

        //        NativeMethods.ToUnicodeEx(
        //            (uint)VirtualKeyCode.SPACE,
        //            NativeMethods.MapVirtualKeyEx((uint)VirtualKeyCode.SPACE, 0, _currentInputLocaleIdentifier),
        //            keyState,
        //            sbUnshifted,
        //            sbUnshifted.Capacity,
        //            0,
        //            _currentInputLocaleIdentifier);

        //        // There is one character stored in our buffer though:
        //        rc = 1;
        //    }

        //    if (rc < sbUnshifted.Length)
        //    {
        //        sbUnshifted.Remove(rc, sbUnshifted.Length - rc);
        //    }

        //    if (rc > 0)
        //    {
        //        result.Append(sbUnshifted.ToString().ToUpper());
        //    }

        //    // Set SHIFT on..
        //    keyState[(int)VirtualKeyCode.SHIFT] = 0x80;

        //    StringBuilder sbShifted = new StringBuilder(bufferLength);

        //    rc = NativeMethods.ToUnicodeEx(
        //        vk,
        //        (uint)scancode,
        //        keyState,
        //        sbShifted,
        //        sbShifted.Capacity,
        //        0,
        //        _currentInputLocaleIdentifier);

        //    // If unshifter was a dead key, so will be shifted.
        //    if (rc < 0)
        //    {
        //        int dummy = NativeMethods.ToUnicodeEx(
        //            (uint)VirtualKeyCode.SPACE,
        //            NativeMethods.MapVirtualKeyEx((uint)VirtualKeyCode.SPACE, 0, _currentInputLocaleIdentifier),
        //            keyState,
        //            sbUnshifted,
        //            sbUnshifted.Capacity,
        //            0,
        //            _currentInputLocaleIdentifier);

        //        // There will be one character stored in our buffer though:
        //        // (well, at least one, but we have no way of knowing if more)
        //        rc = 1;
        //    }

        //    if (rc > 1)
        //    {
        //        overlong = true;
        //    }

        //    if (rc < sbShifted.Length)
        //    {
        //        sbShifted.Remove(rc, sbShifted.Length - rc);
        //    }

        //    // If this shifted state the same as the unshifted.ToUpper
        //    // (e.g. e and E) then don't add it.
        //    if (rc > 0 & (String.Compare(sbShifted.ToString(), sbUnshifted.ToString(), System.StringComparison.OrdinalIgnoreCase) != 0))
        //    {
        //        // Not wanting to do this for letters and the like..
        //        result.Append(" " + sbShifted);
        //    }

        //    return result.ToString();

        //}

        public static string GetCurrentKeyboardLocale()
        {
            var buffer = new StringBuilder(new string(' ', NativeMethods.KL_NAMELENGTH));
            // GetKeyboardLayoutName puts the current locale into the passed buffer
            int result = NativeMethods.GetKeyboardLayoutName(buffer);
            if (result == 0)
                return null;
            return buffer.ToString();
        }

        public static void GetInstalledKeyboardList()
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

        public static IEnumerable<string> GetInstalledKeyboardListInNameOrder()
        {
            var results = new string[InstalledKeyboardLayouts.Count];
            InstalledKeyboardLayouts.Keys.CopyTo(results, 0);
            Array.Sort(results);
            return results;
        }

        public static InstalledKeyboardLayout BuildKeyboardLayout(string locale)
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
