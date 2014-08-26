using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;
using WindowsInput.Native;
using NUnit.Framework;
using WPFKeyboardNative;

namespace WPFKeyboard.KBD.Tests
{
    [TestFixture]
    public class ModiferStateManagerTests
    {
        private IModiferStateManager _modiferStateManager;
        private KeyboardLayout _keyboardLayout;

        [Test]
        public void Shift_updates_modifer_state()
        {
            // Act
            Keyboard.Simulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
            try
            {
                // Arrange
                _modiferStateManager.Refresh();

                // Assert
                Assert.That((_modiferStateManager.ModifierState & 1), Is.EqualTo(1));

                // Act
                Keyboard.Simulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);

                // Arrange
                _modiferStateManager.Refresh();

                // Assert
                Assert.That((_modiferStateManager.ModifierState & 1), Is.EqualTo(0));
            }
            finally
            {
                Keyboard.Simulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
            }
        }

        [Test]
        public void Shift_updates_modifer_state_when_hinted()
        {
            // Act
            Keyboard.Simulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
            try
            {
                // Arrange
                _modiferStateManager.Refresh(keyDown:VirtualKeyCode.SHIFT);

                // Assert
                Assert.That((_modiferStateManager.ModifierState & 1), Is.EqualTo(1));

                // Act
                Keyboard.Simulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);

                // Arrange
                _modiferStateManager.Refresh(keyUp: VirtualKeyCode.SHIFT);

                // Assert
                Assert.That((_modiferStateManager.ModifierState & 1), Is.EqualTo(0));
            }
            finally
            {
                Keyboard.Simulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);     
            }
        }

        [SetUp]
        public void Setup()
        {
            _keyboardLayout =
                KeyboardLayoutHelper.GetLayout(
                    string.Format(((IntPtr.Size == 8) || NativeMethods.InternalCheckIsWow64())
                        ? @"C:\Windows\SysWOW64\{0}"
                        : @"C:\Windows\System32\{0}", KeyboardHelper.InstalledKeyboardLayouts["US"].LayoutFile));
            _modiferStateManager = new ModiferStateManager();
            _modiferStateManager.SetModifierKeys(_keyboardLayout.CharModifiers.ToDictionary(x => x.ModifierBits,
                x => (VirtualKeyCode)x.VirtualKey));
        }
    }
}
