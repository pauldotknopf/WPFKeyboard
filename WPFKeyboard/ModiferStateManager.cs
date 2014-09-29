using System;
using System.Collections.Generic;
using System.Linq;
using WindowsInput.Native;

namespace WPFKeyboard
{
    public class ModiferStateManager : IModiferStateManager
    {
        private Dictionary<int, VirtualKeyCode> _modifierKeys;
        private Dictionary<int, bool> _state;
        private int _modifierState;

        public void SetModifierKeys(Dictionary<int, VirtualKeyCode> modifierKeys)
        {
            if (modifierKeys == null) throw new ArgumentNullException("modifierKeys");

            _modifierKeys = modifierKeys;
            _state = new Dictionary<int, bool>();
            foreach (var bit in _modifierKeys.Keys)
                _state.Add(bit, false);
            Refresh();
        }

        public void Refresh(VirtualKeyCode? keyUp = null, VirtualKeyCode? keyDown = null)
        {
            if (_modifierKeys == null) return;

            foreach (var bit in _modifierKeys.Keys.ToList())
            {
                var virtualKey = _modifierKeys[bit];

                var isToggleKey = IsToggleKey(virtualKey);
                _state[bit] = isToggleKey
                    ? Keyboard.InputDeviceStateAdapter.IsTogglingKeyInEffect(virtualKey)
                    : (Keyboard.InputDeviceStateAdapter.IsHardwareKeyDown(virtualKey));

                if (keyDown.HasValue)
                {
                    if (virtualKey == VirtualKeyCode.SHIFT
                        || virtualKey == VirtualKeyCode.LSHIFT
                        || virtualKey == VirtualKeyCode.RSHIFT)
                        switch (keyDown.Value)
                        {
                            case VirtualKeyCode.LSHIFT:
                            case VirtualKeyCode.RSHIFT:
                            case VirtualKeyCode.SHIFT:
                                _state[bit] = true;
                                break;
                        }
                    if (virtualKey == VirtualKeyCode.MENU
                        || virtualKey == VirtualKeyCode.LMENU
                        || virtualKey == VirtualKeyCode.RMENU)
                        switch (keyDown.Value)
                        {
                            case VirtualKeyCode.MENU:
                            case VirtualKeyCode.LMENU:
                            case VirtualKeyCode.RMENU:
                                _state[bit] = true;
                                break;
                        }
                    if (virtualKey == VirtualKeyCode.CONTROL
                        || virtualKey == VirtualKeyCode.LCONTROL
                        || virtualKey == VirtualKeyCode.RCONTROL)
                        switch (keyDown.Value)
                        {
                            case VirtualKeyCode.CONTROL:
                            case VirtualKeyCode.LCONTROL:
                            case VirtualKeyCode.RCONTROL:
                                _state[bit] = true;
                                break;
                        }
                }
                if (keyUp.HasValue)
                {
                    if (virtualKey == VirtualKeyCode.SHIFT
                        || virtualKey == VirtualKeyCode.LSHIFT
                        || virtualKey == VirtualKeyCode.RSHIFT)
                        switch (keyUp.Value)
                        {
                            case VirtualKeyCode.LSHIFT:
                            case VirtualKeyCode.RSHIFT:
                            case VirtualKeyCode.SHIFT:
                                _state[bit] = false;
                                break;
                        }
                    if (virtualKey == VirtualKeyCode.MENU
                        || virtualKey == VirtualKeyCode.LMENU
                        || virtualKey == VirtualKeyCode.RMENU)
                        switch (keyUp.Value)
                        {
                            case VirtualKeyCode.MENU:
                            case VirtualKeyCode.LMENU:
                            case VirtualKeyCode.RMENU:
                                _state[bit] = false;
                                break;
                        }
                    if (virtualKey == VirtualKeyCode.CONTROL
                        || virtualKey == VirtualKeyCode.LCONTROL
                        || virtualKey == VirtualKeyCode.RCONTROL)
                        switch (keyUp.Value)
                        {
                            case VirtualKeyCode.CONTROL:
                            case VirtualKeyCode.LCONTROL:
                            case VirtualKeyCode.RCONTROL:
                                _state[bit] = false;
                                break;
                        }
                }

                if (virtualKey == VirtualKeyCode.SHIFT)
                {
                    IsShiftOn = _state[bit];
                    if (Keyboard.InputDeviceStateAdapter.IsTogglingKeyInEffect((VirtualKeyCode.CAPITAL)))
                    {
                        _state[bit] = !_state[bit];
                        IsCapsLockOn = true;
                    }
                    else IsCapsLockOn = false;
                }

            }

            _modifierState = 0;
            foreach (var bit in _state.Keys.Where(bit => _state[bit]))
            {
                _modifierState |= bit;
            }
        }

        public int ModifierState { get { return _modifierState; } }

        public bool IsShiftOn { get; set; }
        public bool IsCapsLockOn { get; set; }

        private bool IsToggleKey(VirtualKeyCode key)
        {
            return key == VirtualKeyCode.CAPITAL;
        }
    }
}
