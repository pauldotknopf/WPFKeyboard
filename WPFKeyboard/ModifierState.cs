using System;
using System.Collections.Generic;
using System.Linq;
using WindowsInput.Native;

namespace WPFKeyboard
{
    public class ModiferState
    {
        private readonly Dictionary<int, VirtualKeyCode> _modifierKeys = new Dictionary<int, VirtualKeyCode>();
        private readonly Dictionary<int, bool> _state;
        private int _modifierState = 0;

        public ModiferState(Dictionary<int, VirtualKeyCode> modifierKeys)
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
            foreach (var bit in _modifierKeys.Keys.ToList())
            {
                var virtualKey = _modifierKeys[bit];
                if (virtualKey == VirtualKeyCode.SHIFT)
                {
                    if (keyDown.HasValue)
                    {
                        if (keyDown.Value == VirtualKeyCode.SHIFT || keyDown.Value == VirtualKeyCode.LSHIFT ||
                            keyDown.Value == VirtualKeyCode.RSHIFT)
                        {
                            _state[bit] = true;
                            continue;
                        }
                    }
                    if (keyUp.HasValue)
                    {
                        if (keyUp.Value == VirtualKeyCode.SHIFT || keyUp.Value == VirtualKeyCode.LSHIFT ||
                            keyUp.Value == VirtualKeyCode.RSHIFT)
                        {
                            _state[bit] = false;
                            continue;
                        }
                    }
                }
                _state[bit] = IsToggleKey(virtualKey)
                    ? Keyboard.InputDeviceStateAdapter.IsTogglingKeyInEffect(virtualKey)
                    : Keyboard.InputDeviceStateAdapter.IsKeyDown(virtualKey);
            }
            _modifierState = 0;
            foreach (var bit in _state.Keys.Where(bit => _state[bit]))
            {
                _modifierState |= bit;
            }
        }

        public IEnumerable<VirtualKeyCode> GetVirtualKeys()
        {
            return _modifierKeys.Values.ToList();
        }

        public int ModifierState { get { return _modifierState; } }

        private bool IsToggleKey(VirtualKeyCode key)
        {
            return key != VirtualKeyCode.SHIFT && key != VirtualKeyCode.MENU && key != VirtualKeyCode.CONTROL;
        }
    }
}
