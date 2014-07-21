using System;
using System.Collections.Generic;
using System.Globalization;
using WindowsInput.Native;
using WPFKeyboard.Models;

namespace WPFKeyboard
{
    public class VirtualKey : BaseOnScreenKeyViewModel, IButtonEventListener, IKeyEventListener
    {
        private readonly KPDOnScreenKeyboardViewModel _viewModel;
        private readonly VirtualKeyCode _virtualKey;
        private readonly List<int> _characters;
        private readonly bool _isAffectedByCapsLock;
        private readonly bool _isStickyKey;
        private bool _isStickyKeyDuplicateDown;
        private bool _isVirtualButtonPress;
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
            if (_virtualKey == VirtualKeyCode.LSHIFT || _virtualKey == VirtualKeyCode.LCONTROL || _virtualKey == VirtualKeyCode.LMENU
                || _virtualKey == VirtualKeyCode.RSHIFT || _virtualKey == VirtualKeyCode.RCONTROL || _virtualKey == VirtualKeyCode.RMENU)
                _isStickyKey = true;
            _displayText = displayText;

            Display = GetDisplayValue(_viewModel.ModiferStateManager);
        }

        /// <summary>
        /// The button was pressed down
        /// </summary>
        public void ButtonDown()
        {
            _isVirtualButtonPress = true;
            if (!_viewModel.IsStickyKeyHeld || _isStickyKey)
                Keyboard.Simulator.Keyboard.KeyDown(_virtualKey);
            else
            {
                Keyboard.Simulator.Keyboard.KeyDown(_viewModel.StickyVirtualKeyCode);
                Keyboard.Simulator.Keyboard.KeyDown(_virtualKey);
                Keyboard.Simulator.Keyboard.KeyUp(_viewModel.StickyVirtualKeyCode);
                _viewModel.IsStickyKeyHeld = false;
            }
        }

        public void ButtonUp()
        {
            Keyboard.Simulator.Keyboard.KeyUp(_virtualKey);
            _isVirtualButtonPress = false;
        }

        public void KeyDown(System.Windows.Forms.KeyEventArgs args, IModiferStateManager modifierState)
        {
            if ((int)args.KeyCode == (int)_virtualKey)
            {
                if (_isVirtualButtonPress)
                {
                    if (_isStickyKey && !_isStickyKeyDuplicateDown)
                    {
                        Console.WriteLine("_isStickyKey");
                        _viewModel.IsStickyKeyHeld = !_viewModel.IsStickyKeyHeld;
                        _viewModel.StickyVirtualKeyCode = _virtualKey;
                    }
                    else if (_isStickyKeyDuplicateDown)
                    {
                        _isStickyKeyDuplicateDown = false;
                    }
                }
                IsActive = true;
            }
            Display = GetDisplayValue(modifierState);
        }

        public void KeyPressed(System.Windows.Forms.KeyPressEventArgs character, IModiferStateManager modifierState)
        {
        }

        public void KeyUp(System.Windows.Forms.KeyEventArgs args, IModiferStateManager modifierState)
        {
            if ((int)args.KeyCode == (int)_virtualKey)
            {
                if (_isStickyKey && _isVirtualButtonPress)
                {
                    if (!_viewModel.IsStickyKeyHeld)
                        IsActive = false;
                    else
                    {
                        _isStickyKeyDuplicateDown = true;
                        Keyboard.Simulator.Keyboard.KeyDown(_virtualKey);
                    }
                }
                else
                    IsActive = false;
            }

            Display = GetDisplayValue(modifierState);
        }

        public VirtualKeyCode Key { get { return _virtualKey; } }

        private string GetDisplayValue(IModiferStateManager modifierState)
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

        public override void UpdateDisplay(IModiferStateManager modiferStateManager)
        {
            Display = GetDisplayValue(modiferStateManager);
        }
    }
}
