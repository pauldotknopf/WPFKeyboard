using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WindowsInput;
using WindowsInput.Native;

namespace WPFKeyboard.Models
{
    public class VirtualKey : BaseOnScreenKeyViewModel, IButtonEventListener, IKeyEventListener
    {
        private readonly VirtualKeyCode _virtualKey;
        private readonly List<string> _characters;
        private readonly bool _isAffectedByCapsLock;
        private readonly string _displayText;

        public VirtualKey(VirtualKeyCode virtualKey,
            string displayText,
            List<string> characters,
            bool isAffectedByCapsLock)
        {
            _virtualKey = virtualKey;
            _characters = characters;
            _isAffectedByCapsLock = isAffectedByCapsLock;
            _displayText = displayText;
            Display = GetDisplayValue(false, false);
        }

        public void ButtonDown()
        {

        }

        public void ButtonUp()
        {
            Keyboard.Simulator.Keyboard.KeyPress(_virtualKey);
        }

        public void KeyDown(System.Windows.Forms.KeyEventArgs args, bool isShifting, bool isCapsLockOn)
        {
            if ((int)args.KeyCode == (int)_virtualKey)
            {
                IsActive = true;
            }

            Display = GetDisplayValue(isShifting, isCapsLockOn);
        }

        public void KeyPressed(System.Windows.Forms.KeyPressEventArgs character, bool isShifting, bool isCapsLockOn)
        {
        }

        public void KeyUp(System.Windows.Forms.KeyEventArgs args, bool isShifting, bool isCapsLockOn)
        {
            if ((int)args.KeyCode == (int)_virtualKey)
            {
                IsActive = false;
            }

            Display = GetDisplayValue(isShifting, isCapsLockOn);
        }

        private string GetDisplayValue(bool isShift, bool isCapsLockOn)
        {
            //return "temp";
            if (!string.IsNullOrEmpty(_displayText))
                return _displayText;

            if (_characters == null || _characters.Count == 0)
                return ((VirtualKeyCode) _virtualKey).ToString();

            if (_isAffectedByCapsLock)
            {
                if (isCapsLockOn)
                {
                    return isShift ? _characters.First() : _characters.Skip(1).First();
                }
                return isShift ? _characters.Skip(1).First() : _characters.First();
            }
            return isShift ? _characters.Skip(1).First() : _characters.First();
        }
    }
}
