using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Windows.Forms;
using WindowsInput.Native;
using WPFKeyboardNative;

namespace WPFKeyboard.KBD.Tests
{
    [TestFixture]
    public abstract class BaseKeyboardTest
    {
// ReSharper disable InconsistentNaming
        protected KPDOnScreenKeyboardViewModel _viewModel;
        protected Mock<IModiferStateManager> _modifierStateManager;
        protected KeyboardLayout _keyboardLayout;
        protected Dictionary<VirtualKeyCode, int> _modifierKeys;
// ReSharper restore InconsistentNaming

        [SetUp]
        public void Setup()
        {
            _modifierStateManager = new Mock<IModiferStateManager>();
            _keyboardLayout = KeyboardLayoutHelper.GetLayout(string.Format(((IntPtr.Size == 8) || NativeMethods.InternalCheckIsWow64())
               ? @"C:\Windows\SysWOW64\{0}"
               : @"C:\Windows\System32\{0}", GetKeyboardLayout().LayoutFile));
            _modifierKeys = _keyboardLayout.CharModifiers.ToDictionary(x => (VirtualKeyCode) x.VirtualKey,
                x => x.ModifierBits);
            _viewModel = new KPDOnScreenKeyboardViewModel(GetKeyboardLayout(), _modifierStateManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
        }

        protected void SimulateModiferKeys(params VirtualKeyCode[] modiferKeys)
        {
            _modifierStateManager.Setup(x => x.ModifierState).Returns(modiferKeys.Aggregate(0, (current, key) => current | _modifierKeys[key]));
        }

        public abstract InstalledKeyboardLayout GetKeyboardLayout();

        protected VirtualKey GetVirtualKeyModel(VirtualKeyCode virtualKey)
        {
            return _viewModel.Sections.SelectMany(x => x.Rows)
                .SelectMany(x => x.Keys)
                .OfType<VirtualKey>()
                .Single(x => x.Key == virtualKey);
        }
    }
}
