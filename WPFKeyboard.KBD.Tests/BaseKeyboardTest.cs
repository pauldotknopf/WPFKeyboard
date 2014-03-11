using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsInput.Native;
using NUnit.Framework;

namespace WPFKeyboard.KBD.Tests
{
    [TestFixture]
    public abstract class BaseKeyboardTest
    {
// ReSharper disable InconsistentNaming
        protected KPDOnScreenKeyboardViewModel _viewModel;
// ReSharper restore InconsistentNaming

        [SetUp]
        public void Setup()
        {
            if (IntPtr.Size == 4)
            {
                // 32-bit
            }
            else if (IntPtr.Size == 8)
            {
                // 64-bit
            }
            else
            {
                // The future is now!
            }

            _viewModel = new KPDOnScreenKeyboardViewModel(GetKeyboardLayout());
            Keyboard.KeyDown += OnKeyDown;
            Keyboard.KeyUp += OnKeyUp;
        }

        [TearDown]
        public void TearDown()
        {
            Keyboard.KeyDown -= OnKeyDown;
            Keyboard.KeyUp -= OnKeyUp;
        }

        private void OnKeyDown(object sender, KeyEventArgs args)
        {
            if (_viewModel.ModiferState != null)
                _viewModel.ModiferState.Refresh(keyDown: (VirtualKeyCode)args.KeyCode);

            foreach (var section in _viewModel.Sections)
            {
                foreach (var row in section.Rows)
                {
                    foreach (var key in row.Keys)
                    {
                        if (key is IKeyEventListener)
                        {
                            (key as IKeyEventListener).KeyDown(args, _viewModel.ModiferState);
                        }
                    }
                }
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs args)
        {
            if (_viewModel.ModiferState != null)
                _viewModel.ModiferState.Refresh((VirtualKeyCode)args.KeyCode);

            foreach (var section in _viewModel.Sections)
            {
                foreach (var row in section.Rows)
                {
                    foreach (var key in row.Keys)
                    {
                        if (key is IKeyEventListener)
                        {
                            (key as IKeyEventListener).KeyUp(args, _viewModel.ModiferState);
                        }
                    }
                }
            }
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
