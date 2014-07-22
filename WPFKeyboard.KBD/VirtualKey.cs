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
        private bool _isStickyEnabled;
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
            _isStickyKey = IsAStickyKey(_virtualKey);
            _displayText = displayText;

            Display = GetDisplayValue(_viewModel.ModiferStateManager);
        }

        /// <summary>
        /// The button was pressed down
        /// </summary>
        public void ButtonDown()
        {
            if (_isStickyKey)
                _isStickyEnabled = !_isStickyEnabled;
            Keyboard.Simulator.Keyboard.KeyDown(_virtualKey);
        }

        public void ButtonUp()
        {
            if (!_isStickyKey || (_isStickyKey && !_isStickyEnabled))
                Keyboard.Simulator.Keyboard.KeyUp(_virtualKey);
        }

        public void KeyDown(System.Windows.Forms.KeyEventArgs args, IModiferStateManager modifierState, bool isVirtual)
        {
            if ((int)args.KeyCode == (int)_virtualKey)
            {
                IsActive = true;
            }
            Display = GetDisplayValue(modifierState);
        }

        public void KeyUp(System.Windows.Forms.KeyEventArgs args, IModiferStateManager modifierState, bool isVirtual)
        {
            if (IsActive)
            {
                var isActualKeyPressedAndNotLooping = (int)args.KeyCode == (int)_virtualKey;
                var isNonStickyKeyPressed = !IsAStickyKey((int)args.KeyCode);
                var isActualKeyPressedASticky = isActualKeyPressedAndNotLooping &&
                                               IsAStickyKey((VirtualKeyCode)args.KeyCode);
                if ((_isStickyKey && _isStickyEnabled) && ((isVirtual && isActualKeyPressedASticky) || (!isVirtual && isActualKeyPressedAndNotLooping)
                    || (isActualKeyPressedAndNotLooping) || (!isVirtual) || (isNonStickyKeyPressed)))
                {
                    IsActive = false;
                    // Turn _isStickyEnabled to false and fire a KeyUp so that the key isn't 'held' down anymore
                    _isStickyEnabled = false;
                    Keyboard.Simulator.Keyboard.KeyUp(_virtualKey);
                }
                else if (_isStickyKey && !_isStickyEnabled && isVirtual && isActualKeyPressedAndNotLooping)
                {
                    IsActive = false;
                    // Turn _isStickyEnabled to false and fire a KeyUp so that the key isn't 'held' down anymore
                    _isStickyEnabled = false;
                    Keyboard.Simulator.Keyboard.KeyUp(_virtualKey);
                }
                else if (!isVirtual && isActualKeyPressedAndNotLooping)
                {
                    // This is for hardware keys being pressed
                    IsActive = false;
                    if (_isStickyKey && _isStickyEnabled)
                    {
                        // If an on-screen sticky was toggled, need to turn off _isStickyEnabled if they hit the hardware sticky key
                        _isStickyEnabled = false;
                    }
                }
                else if (isVirtual && !_isStickyKey &&
                         isActualKeyPressedAndNotLooping)
                {
                    // This is a generic on-screen key press that isn't a sticky
                    IsActive = false;
                }
            }
            Display = GetDisplayValue(modifierState);
        }

        private bool IsAStickyKey(VirtualKeyCode code)
        {
            return code == VirtualKeyCode.LSHIFT || code == VirtualKeyCode.LMENU || code == VirtualKeyCode.LCONTROL ||
                   code == VirtualKeyCode.RSHIFT || code == VirtualKeyCode.RMENU || code == VirtualKeyCode.RCONTROL;
        }

        private bool IsAStickyKey(int code)
        {
            return code == (int)VirtualKeyCode.LSHIFT || code == (int)VirtualKeyCode.LMENU || code == (int)VirtualKeyCode.LCONTROL ||
                    code == (int)VirtualKeyCode.RSHIFT || code == (int)VirtualKeyCode.RMENU || code == (int)VirtualKeyCode.RCONTROL;
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
                character = (_viewModel.ModifierBitsSortedByIndex[index] & modifierState.ModifierState) ==
                            modifierState.ModifierState ? CharacterAtIndex(index) : Constants.WchNone;
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
