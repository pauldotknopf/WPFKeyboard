using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WindowsInput.Native;

namespace WPFKeyboard.KBD.Tests
{
    [TestFixture]
    public class ToUnicodeTests : BaseKeyboardTest
    {
        [Test]
        public void Can_get_characters_from_mod_bits() 
        {
            var handle = InputLanguage.InstalledInputLanguages.Cast<InputLanguage>().First().Handle;
            SimulateModiferKeys(false, false, VirtualKeyCode.SHIFT, VirtualKeyCode.SHIFT);
            var resut = KeyboardHelper.GetKeyNameFromVirtualKey(handle, WindowsInput.Native.VirtualKeyCode.VK_A, _modifierStateManager.Object.ModifierState, _modifierKeys, true);
        }

        public override InstalledKeyboardLayout GetKeyboardLayout()
        {
            return KeyboardHelper.InstalledKeyboardLayouts["US"];
        }
    }
}
