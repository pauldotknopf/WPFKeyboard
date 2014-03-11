using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput.Native;

namespace WPFKeyboard.KBD.Tests
{
    public class ModificationSession : IDisposable
    {
        private readonly VirtualKeyCode[] _virtualKeyCodes;

        public ModificationSession(params VirtualKeyCode[] virtualKeyCodes)
        {
            _virtualKeyCodes = virtualKeyCodes;

            foreach (var virtualKey in _virtualKeyCodes)
                Keyboard.Simulator.Keyboard.KeyDown(virtualKey);
        }

        public void Dispose()
        {
            foreach (var virtualKey in _virtualKeyCodes)
                Keyboard.Simulator.Keyboard.KeyUp(virtualKey);
        }
    }
}
