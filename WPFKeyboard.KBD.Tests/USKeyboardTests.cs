using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput.Native;
using NUnit.Framework;

namespace WPFKeyboard.KBD.Tests
{
    [TestFixture]
    public class USKeyboardTests : BaseKeyboardTest
    {
        [Test]
        public void Can_get_character_for_modifications()
        {
            var virtualKey = GetVirtualKeyModel(VirtualKeyCode.VK_E);

            Assert.That(virtualKey.Display, Is.EqualTo("e"));

            SimulateModiferKeys(VirtualKeyCode.SHIFT);
            virtualKey.UpdateDisplay(_modifierStateManager.Object);
            Assert.That(virtualKey.Display, Is.EqualTo("E"));

            SimulateModiferKeys(VirtualKeyCode.CONTROL);
            virtualKey.UpdateDisplay(_modifierStateManager.Object);
            Assert.That(virtualKey.Display, Is.EqualTo("e"));

            SimulateModiferKeys(VirtualKeyCode.MENU);
            virtualKey.UpdateDisplay(_modifierStateManager.Object);
            Assert.That(virtualKey.Display, Is.EqualTo("e"));
        }

        public override InstalledKeyboardLayout GetKeyboardLayout()
        {
            return KeyboardHelper.InstalledKeyboardLayouts["US"];
        }
    }
}
