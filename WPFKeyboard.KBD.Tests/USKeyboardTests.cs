using NUnit.Framework;
using WindowsInput.Native;

namespace WPFKeyboard.KBD.Tests
{
    [TestFixture]
    public class USKeyboardTests : BaseKeyboardTest
    {
        //All the tests fail due to some issues with SimulateModifierKeys not working

        [Test]
        public void Can_do_single_keys_and_single_sticky_key_combo()
        {
            var virtualKey = GetVirtualKeyModel(VirtualKeyCode.VK_E);
            var virtualShiftKey = GetVirtualKeyModel(VirtualKeyCode.LSHIFT);
            Assert.That(virtualKey.Display, Is.EqualTo("e"));

            SimulateModiferKeys(VirtualKeyCode.LSHIFT);
            virtualKey.UpdateDisplay(_modifierStateManager.Object);
            virtualShiftKey.UpdateDisplay(_modifierStateManager.Object);
            Assert.That(virtualKey.Display, Is.EqualTo("E"));
            Assert.AreEqual(virtualShiftKey.IsActive, true);
        }

        [Test]
        public void Can_do_multiple_sticky_key_combos()
        {
            var virtualKeyE = GetVirtualKeyModel(VirtualKeyCode.VK_E);
            var virtualShiftKey = GetVirtualKeyModel(VirtualKeyCode.LSHIFT);
            var virtualControlKey = GetVirtualKeyModel(VirtualKeyCode.LCONTROL);
            Assert.That(virtualKeyE.Display, Is.EqualTo("e"));

            SimulateModiferKeys(VirtualKeyCode.SHIFT, VirtualKeyCode.CONTROL);
            virtualKeyE.UpdateDisplay(_modifierStateManager.Object);
            virtualShiftKey.UpdateDisplay(_modifierStateManager.Object);
            virtualControlKey.UpdateDisplay(_modifierStateManager.Object);
            Assert.That(virtualKeyE.Display, Is.EqualTo("E"));
            Assert.AreEqual(virtualShiftKey.IsActive, true);
            Assert.AreEqual(virtualControlKey.IsActive, true);
        }

        [Test]
        public void Caps_lock_key_works()
        {
            var virtualCapsLock = GetVirtualKeyModel(VirtualKeyCode.CAPITAL);
            var virtualLKey = GetVirtualKeyModel(VirtualKeyCode.VK_L);
            var virtualXKey = GetVirtualKeyModel(VirtualKeyCode.VK_X);
            var virtualWKey = GetVirtualKeyModel(VirtualKeyCode.VK_W);

            SimulateModiferKeys(VirtualKeyCode.CAPITAL);
            virtualCapsLock.UpdateDisplay(_modifierStateManager.Object);
            virtualLKey.UpdateDisplay(_modifierStateManager.Object);
            virtualXKey.UpdateDisplay(_modifierStateManager.Object);
            virtualWKey.UpdateDisplay(_modifierStateManager.Object);

            Assert.AreEqual(virtualCapsLock.IsActive, true);
            Assert.That(virtualLKey.Display, Is.EqualTo("L"));
            Assert.That(virtualXKey.Display, Is.EqualTo("X"));
            Assert.That(virtualWKey.Display, Is.EqualTo("W"));
        }

        [Test]
        public void Top_row_keys_affected_by_shift()
        {
            var virtualShiftKey = GetVirtualKeyModel(VirtualKeyCode.LSHIFT);
            var virtualOneKey = GetVirtualKeyModel(VirtualKeyCode.VK_1);
            Assert.That(virtualOneKey.Display, Is.EqualTo("1"));

            SimulateModiferKeys(VirtualKeyCode.SHIFT);
            virtualShiftKey.UpdateDisplay(_modifierStateManager.Object);
            virtualOneKey.UpdateDisplay(_modifierStateManager.Object);

            Assert.That(virtualOneKey.Display, Is.EqualTo("!"));
        }

        [Test]
        public void Top_row_keys_not_affected_by_caps()
        {
            var virtualCapLockKey = GetVirtualKeyModel(VirtualKeyCode.CAPITAL);
            var virtualOneKey = GetVirtualKeyModel(VirtualKeyCode.VK_1);
            Assert.That(virtualOneKey.Display, Is.EqualTo("1"));

            SimulateModiferKeys(VirtualKeyCode.CAPITAL);
            virtualCapLockKey.UpdateDisplay(_modifierStateManager.Object);
            virtualOneKey.UpdateDisplay(_modifierStateManager.Object);

            Assert.That(virtualOneKey.Display, Is.EqualTo("1"));
        }
        
        

        public override InstalledKeyboardLayout GetKeyboardLayout()
        {
            return KeyboardHelper.InstalledKeyboardLayouts["US"];
        }
    }
}
