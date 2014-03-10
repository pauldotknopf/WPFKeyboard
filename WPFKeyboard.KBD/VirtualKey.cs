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
        private readonly List<int> _modBits;
        private readonly string _displayText;
        private const int ShftInvalid = 0x0F;
        private const int WchNone = 0xF000;
        private const int WchDead = 0xF001;
        private const int WchLgtr = 0xF002;

        public VirtualKey(VirtualKeyCode virtualKey,
            string displayText,
            List<int> characters,
            bool isAffectedByCapsLock,
            ModiferState modifierState,
            List<int> modBits)
        {
            _virtualKey = virtualKey;
            _characters = characters;
            _isAffectedByCapsLock = isAffectedByCapsLock;
            _modBits = modBits;
            _displayText = displayText;

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

        public void KeyDown(System.Windows.Forms.KeyEventArgs args, ModiferState modifierState)
        {
            if ((int)args.KeyCode == (int)_virtualKey)
            {
                IsActive = true;
            }


            Display = GetDisplayValue(modifierState);
        }

        public void KeyPressed(System.Windows.Forms.KeyPressEventArgs character, ModiferState modifierState)
        {
        }

        public void KeyUp(System.Windows.Forms.KeyEventArgs args, ModiferState modifierState)
        {
            if ((int)args.KeyCode == (int)_virtualKey)
            {
                IsActive = false;
            }

            Display = GetDisplayValue(modifierState);
        }

        public VirtualKeyCode Key { get { return _virtualKey; } }

        private string GetDisplayValue(ModiferState modifierState)
        {
            if (!string.IsNullOrEmpty(_displayText))
                return _displayText;

            if (_characters == null || _characters.Count == 0)
                return _virtualKey.ToString();

            var index = _modBits[modifierState.ModifierState];

            if (index == ShftInvalid)
                return string.Empty;

            var character = WchNone;

            if (index < _characters.Count)
                character = _characters[index];

            if (character == WchNone)
            {
                // slowly remote the modifer bits, starting from the least to most significant bit
                var state = modifierState.ModifierState;
                foreach (var modifierKey in modifierState.GetModifierKeys())
                {
                    state = state & ~(modifierKey.Key);

                    index = _modBits[state];

                    if(index == ShftInvalid) continue;

                    if (index < _characters.Count)
                        character = _characters[index];
                    else
                        character = WchNone;

                    if(character == WchNone) continue;

                    return ((char)character).ToString();
                }
            }

            return ((char) character).ToString();
        }
    }
}
