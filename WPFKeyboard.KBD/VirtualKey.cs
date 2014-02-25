using System.Collections.Generic;
using WindowsInput.Native;
using WPFKeyboard.Models;
using WPFKeyboardNative;

namespace WPFKeyboard
{
    public class VirtualKey : BaseOnScreenKeyViewModel, IButtonEventListener, IKeyEventListener
    {
        private readonly VirtualKeyCode _virtualKey;
        private readonly List<int> _characters;
        private readonly bool _isAffectedByCapsLock;
        private readonly string _displayText;
        private readonly char characterBase;
        private readonly char characterShift;
        private readonly char characterAltGraphics;
        private readonly char characterControl;
        private readonly char characterShiftControl;
        private readonly char characterShiftAltGraphics;

        public VirtualKey(VirtualKeyCode virtualKey,
            string displayText,
            List<int> characters,
            bool isAffectedByCapsLock)
        {
            _virtualKey = virtualKey;
            _characters = characters;
            _isAffectedByCapsLock = isAffectedByCapsLock;
            _displayText = displayText;

            for (var x = 0; x < characters.Count; x++)
            {
                var character = default(char);
                var value = characters[x];
                if (value == Definitions.WCH_DEAD)
                {
                    character = ' ';
                }
                else if (value == Definitions.WCH_LGTR)
                {
                    character = ' ';
                }
                else if (value == Definitions.WCH_NONE)
                {
                    character = ' ';
                }
                else
                {
                    character = (char)value;
                }

                switch (x)
                {
                    case 0:
                        characterBase = character;
                        break;
                    case 1:
                        characterShift = character;
                        break;
                    case 2:
                        characterAltGraphics = character;
                        break;
                    case 3:
                        characterControl = character;
                        break;
                    case 4:
                        characterShiftControl = character;
                        break;
                    case 5:
                        characterShiftAltGraphics = character;
                        break;
                }
            }

            Display = GetDisplayValue(false, false);
        }

        public void ButtonDown()
        {
            Keyboard.Simulator.Keyboard.KeyDown(_virtualKey);
        }

        public void ButtonUp()
        {
            Keyboard.Simulator.Keyboard.KeyUp(_virtualKey);
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

        public VirtualKeyCode Key { get { return _virtualKey; } }

        private string GetDisplayValue(bool isShift, bool isCapsLockOn)
        {
            if (!string.IsNullOrEmpty(_displayText))
                return _displayText;

            if (_characters == null || _characters.Count == 0)
                return ((VirtualKeyCode)_virtualKey).ToString();

            if (_isAffectedByCapsLock)
            {
                if (isCapsLockOn)
                {
                    return new string(isShift ? characterBase : characterShift, 1);
                }
                return new string(isShift ? characterShift : characterBase, 1);
            }
            return new string(isShift ? characterShift : characterBase, 1);
        }
    }
}
