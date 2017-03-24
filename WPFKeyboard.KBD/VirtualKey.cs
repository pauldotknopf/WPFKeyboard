using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WindowsInput.Native;
using WPFKeyboard.Models;
using WPFKeyboardNative;

namespace WPFKeyboard
{
    public class VirtualKey : BaseOnScreenKeyViewModel, IButtonEventListener, IKeyEventListener
    {
        private readonly KPDOnScreenKeyboardViewModel _viewModel;
        private readonly VirtualKeyCode _virtualKey;
        private readonly List<VirtualKeyCharacter> _characters;
        private readonly bool _isAffectedByCapsLock;
        private readonly bool _isStickyKey;
        private bool _isStickyEnabled;
        private readonly string _displayText;

        public VirtualKey(KPDOnScreenKeyboardViewModel viewModel,
            VirtualKeyCode virtualKey,
            string displayText,
            List<VirtualKeyCharacter> characters,
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
            return code == (int)VirtualKeyCode.LSHIFT || code == (int)VirtualKeyCode.LMENU ||
                   code == (int)VirtualKeyCode.LCONTROL ||
                   code == (int)VirtualKeyCode.RSHIFT || code == (int)VirtualKeyCode.RMENU ||
                   code == (int)VirtualKeyCode.RCONTROL;
        }

        public VirtualKeyCode Key { get { return _virtualKey; } }

        private string GetDisplayValue(IModiferStateManager modifierState)
        {
            if (!string.IsNullOrEmpty(_displayText))
                return _displayText;

            if (_characters == null || _characters.Count == 0)
                return _virtualKey.ToString();

            var modState = modifierState.ModifierState;

            if (!_isAffectedByCapsLock)
            {
                // we need to determine if the shift bit was turned on solely because of the caps lock.
                // if so, we need to turn it off, because, for this key, it isn't affected by caps,
                // so the shift shouldn't be toggled because of that
                // so, what we need to do, is simply update that bit to represent exactly what shift is
                foreach (var modifier in _viewModel.KeyboardLayout.CharModifiers)
                {
                    if (modifier.VirtualKey == (int)VirtualKeyCode.SHIFT)
                    {
                        // we found the bit that coresponds to the shift virtual key
                        if (modifierState.IsShiftOn && !modifierState.IsCapsLockOn)
                        {
                            modState = modState & modifier.ModifierBits;
                        }
                        else if (modifierState.IsShiftOn && modifierState.IsCapsLockOn)
                        {
                            // For keys not affected by caps lock when caps lock is on 
                            // and shift is on it is treated as just a normal shift
                            modState = 0001;
                        }
                        else
                        {
                            modState = modState & ~(modifier.ModifierBits);
                        }
                        break;
                    }
                }
            }

            if (modState < _viewModel.KeyboardLayout.ModifierBits.Count)
            {
                return CharacterAtIndex(_viewModel.KeyboardLayout.ModifierBits[modState]);
            }

            // no character for the given modification.
            // lets load normal/shift values

            int shiftModBit = -1;

            foreach (var value in _viewModel.KeyboardLayout.CharModifiers)
            {
                if (value.VirtualKey == (int)VirtualKeyCode.SHIFT)
                {
                    shiftModBit = value.ModifierBits;
                    break;
                }
            }

            if (shiftModBit != -1)
            {
                modState = modState & shiftModBit;
                return CharacterAtIndex(_viewModel.KeyboardLayout.ModifierBits[modState]);
            }

            return "";

            //var min = Math.Min(modState, _viewModel.KeyboardLayout.ModifierBits.Count - 1);

            //var index = _viewModel.KeyboardLayout.ModifierBits[min];

            //var character = CharacterAtIndex(index);

            //while (character == Constants.WchNone || CharUnicodeInfo.GetUnicodeCategory((char)character) == UnicodeCategory.Control)
            //{
            //    if (index == 0)
            //    {
            //        character = CharacterAtIndex(index);
            //        break;
            //    }
            //    index--;
            //    // make sure this index has a combination of bits that are in our current modification state
            //    character = (_viewModel.ModifierBitsSortedByIndex[index] & modState) ==
            //                modState ? CharacterAtIndex(index) : Constants.WchNone;
            //}

            //return ((char)character).ToString(CultureInfo.InvariantCulture);
        }

        private string CharacterAtIndex(int index)
        {
            var returnString = "";

            if (index >= _characters.Count)
            {
                index = 0;
            }

            if (_characters[index].IsLig)
            {
                var stringBuilder = new StringBuilder();

                foreach (int i in _characters[index].Ligs)
                {
                    stringBuilder.Append(char.ConvertFromUtf32(i));
                }

                //Normalize will accurately show the ligature
                returnString = stringBuilder.ToString().Normalize();
            }
            else
            {
                returnString = ((char)_characters[index].Character).ToString(CultureInfo.InvariantCulture);
            }

            return returnString;
        }

        public override void UpdateDisplay(IModiferStateManager modiferStateManager)
        {
            Display = GetDisplayValue(modiferStateManager);
        }
    }
}
