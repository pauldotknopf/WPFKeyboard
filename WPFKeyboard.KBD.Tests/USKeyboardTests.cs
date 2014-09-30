using NUnit.Framework;
using WindowsInput.Native;

namespace WPFKeyboard.KBD.Tests
{
    [TestFixture]
    public class USKeyboardTests : BaseKeyboardTest
    {
        [Test]
        public void Top_row_keys_not_affected_by_caps()
        {
            var keyA = GetVirtualKeyModel(VirtualKeyCode.VK_A);
            Assert.That(keyA.Display, Is.EqualTo("a"));

            SimulateModiferKeys(false, false, VirtualKeyCode.SHIFT);
            keyA.UpdateDisplay(_modifierStateManager.Object);
            Assert.That(keyA.Display, Is.EqualTo("A"));

            SimulateModiferKeys(false, false, VirtualKeyCode.MENU);
            keyA.UpdateDisplay(_modifierStateManager.Object);
            Assert.That(keyA.Display, Is.EqualTo("a"));

            SimulateModiferKeys(false, false, VirtualKeyCode.CONTROL);
            keyA.UpdateDisplay(_modifierStateManager.Object);
            Assert.That(keyA.Display, Is.EqualTo("a"));

            SimulateModiferKeys(false, false, VirtualKeyCode.CONTROL, VirtualKeyCode.MENU);
            keyA.UpdateDisplay(_modifierStateManager.Object);
            Assert.That(keyA.Display, Is.EqualTo("a"));

            SimulateModiferKeys(false, false, VirtualKeyCode.CONTROL, VirtualKeyCode.MENU, VirtualKeyCode.SHIFT);
            keyA.UpdateDisplay(_modifierStateManager.Object);
            Assert.That(keyA.Display, Is.EqualTo("A"));
        }

        public override InstalledKeyboardLayout GetKeyboardLayout()
        {
            return KeyboardHelper.InstalledKeyboardLayouts["US"];
        }
    }
}
