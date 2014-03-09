using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsInput.Native;
using WPFKeyboard.Models;
using WPFKeyboardNative;

namespace WPFKeyboard
{
    public class VirtualKey : BaseOnScreenKeyViewModel, IButtonEventListener, IKeyEventListener
    {
        private readonly VirtualKeyCode _virtualKey;
        private readonly IntPtr _keyboardLayout;
        private readonly string _displayText;
        private readonly bool _isAffectedByCapsLock;

        public VirtualKey(VirtualKeyCode virtualKey,
            IntPtr keyboardLayout,
            string displayText,
            ModiferState modifierState,
            bool isAffectedByCapsLock)
        {
            _virtualKey = virtualKey;
            _keyboardLayout = keyboardLayout;
            _displayText = displayText;
            _isAffectedByCapsLock = isAffectedByCapsLock;

            Display = virtualKey.ToString();
            //Display = GetDisplayValue(false, false);
            Display = GetDisplayValue(modifierState);
        }

        public void ButtonDown()
        {
            Keyboard.Simulator.Keyboard.KeyDown(_virtualKey);
        }

        public void ButtonUp()
        {
            Keyboard.Simulator.Keyboard.KeyUp(_virtualKey);
        }

        public void KeyDown(KeyEventArgs args, ModiferState modiferState)
        {
            if ((int)args.KeyCode == (int)_virtualKey)
            {
                IsActive = true;
            }

            Display = GetDisplayValue(modiferState);
        }

        public void KeyPressed(KeyPressEventArgs character, ModiferState modiferState)
        {
        }

        public void KeyUp(KeyEventArgs args, ModiferState modiferState)
        {
            if ((int)args.KeyCode == (int)_virtualKey)
            {
                IsActive = false;
            }

            Display = GetDisplayValue(modiferState);
        }

        public VirtualKeyCode Key { get { return _virtualKey; } }

        private string GetDisplayValue(ModiferState modifierState)
        {
            if (!string.IsNullOrEmpty(_displayText))
                return _displayText;

            return KeyboardHelper.GetKeyNameFromVirtualKey(_keyboardLayout, _virtualKey, modifierState, _isAffectedByCapsLock);
        }
    }
}
