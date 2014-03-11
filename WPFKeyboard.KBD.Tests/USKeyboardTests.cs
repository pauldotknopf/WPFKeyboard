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

            using (new ModificationSession())
            {
                Assert.That(virtualKey.Display, Is.EqualTo("e"));
            }

            using (new ModificationSession(VirtualKeyCode.SHIFT))
            {
                Assert.That(virtualKey.Display, Is.EqualTo("E"));
            }

            using (new ModificationSession(VirtualKeyCode.CONTROL))
            {
                Assert.That(virtualKey.Display, Is.EqualTo("e"));
            }

            using (new ModificationSession(VirtualKeyCode.MENU))
            {
                Assert.That(virtualKey.Display, Is.EqualTo("e"));
            }
        }

        public override InstalledKeyboardLayout GetKeyboardLayout()
        {
            return KeyboardHelper.InstalledKeyboardLayouts["US"];
        }
    }
}
