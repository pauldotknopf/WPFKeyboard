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
            // We aren't dealing with the visuals of ALT and Control
            _modifierKeys.Remove((int)VirtualKeyCode.MENU);
            _modifierKeys.Remove((int)VirtualKeyCode.CONTROL);
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

                _state[bit] = IsToggleKey(virtualKey)
                    ? Keyboard.InputDeviceStateAdapter.IsTogglingKeyInEffect(virtualKey)
                    : Keyboard.InputDeviceStateAdapter.IsKeyDown(virtualKey);

                if (virtualKey == VirtualKeyCode.SHIFT || virtualKey == VirtualKeyCode.LSHIFT ||
                    virtualKey == VirtualKeyCode.RSHIFT)
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

            // TODO: If caps is pressed or toggled, toggle shift bit
        }

        public int ModifierState { get { return _modifierState; } }

        public bool IsShiftOn { get; set; }
        public bool IsCapsLockOn { get; set; }

        private bool IsToggleKey(VirtualKeyCode key)
        {
            return key != VirtualKeyCode.SHIFT && key != VirtualKeyCode.MENU && key != VirtualKeyCode.CONTROL;
        }
    }
}
