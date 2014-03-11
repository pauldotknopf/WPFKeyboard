using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using WindowsInput.Native;
using WPFKeyboard.Models;
using WPFKeyboardNative;

namespace WPFKeyboard
{
    public class VirtualKey : BaseOnScreenKeyViewModel, IButtonEventListener, IKeyEventListener
    {
        private readonly KPDOnScreenKeyboardViewModel _viewModel;
        private readonly VirtualKeyCode _virtualKey;
        private readonly List<int> _characters;
        private readonly bool _isAffectedByCapsLock;
        private readonly string _displayText;
        
        public VirtualKey(KPDOnScreenKeyboardViewModel viewModel, 
            VirtualKeyCode virtualKey,
            string displayText,
            List<int> characters,
            bool isAffectedByCapsLock)
        {
            _viewModel = viewModel;
            _virtualKey = virtualKey;
            _characters = characters;
            _isAffectedByCapsLock = isAffectedByCapsLock;
            _displayText = displayText;

            Display = GetDisplayValue(_viewModel.ModiferState);
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

            var index = _viewModel.KeyboardLayout.ModifierBits[Math.Min(modifierState.ModifierState, _viewModel.KeyboardLayout.ModifierBits.Count - 1)];

            var character = CharacterAtIndex(index);

            while (character == Constants.WchNone || CharUnicodeInfo.GetUnicodeCategory((char)character) == UnicodeCategory.Control)
            {
                if (index == 0)
                {
                    character = CharacterAtIndex(index);
                    break;
                }
                index--;
                // make sure this index has a combination of bits that are in our current modifications tate
                if ((_viewModel.ModifierBitsSortedByIndex[index] & modifierState.ModifierState) ==
                    modifierState.ModifierState)
                    character = CharacterAtIndex(index);
                else
                    character = Constants.WchNone;
            }

            return ((char)character).ToString();
        }

        private int CharacterAtIndex(int index)
        {
            if (_characters.Count < index + 1)
                return Constants.WchNone;
            return _characters[index];
        }
    }
}
