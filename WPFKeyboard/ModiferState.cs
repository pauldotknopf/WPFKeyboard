using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput.Native;

namespace WPFKeyboard
{
    public class ModiferState
    {
        private readonly Dictionary<VirtualKeyCode, bool> _state = new Dictionary<VirtualKeyCode, bool>(); 

        public ModiferState(IEnumerable<VirtualKeyCode> modiferVirtualKeys)
        {
            foreach (var modiferVirtualKey in modiferVirtualKeys)
                _state.Add(modiferVirtualKey, false);
            if(!_state.ContainsKey(VirtualKeyCode.CAPITAL))
                _state.Add(VirtualKeyCode.CAPITAL, false);
            Refresh();
        }

        public void Refresh()
        {
            foreach (var virtualKey in _state.Keys.ToList())
            {
                _state[virtualKey] = IsToggleKey(virtualKey)
                    ? Keyboard.InputDeviceStateAdapter.IsTogglingKeyInEffect(virtualKey)
                    : Keyboard.InputDeviceStateAdapter.IsKeyDown(virtualKey);
            }
        }

        public IEnumerable<VirtualKeyCode> GetVirtualKeys()
        {
            return _state.Keys.ToList();
        }

        public bool GetModifierState(VirtualKeyCode virtualKey)
        {
            if(!_state.ContainsKey(virtualKey))
                throw new Exception("The virtual key " + virtualKey + " is invalid.");

            return _state[virtualKey];
        }

        private bool IsToggleKey(VirtualKeyCode key)
        {
            return key != VirtualKeyCode.SHIFT && key != VirtualKeyCode.MENU && key != VirtualKeyCode.CONTROL;
        }
    }
}
