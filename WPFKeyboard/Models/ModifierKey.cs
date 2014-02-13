using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput.Native;

namespace WPFKeyboard.Models
{
    public class ModifierKey : BaseOnScreenKeyViewModel, IKeyEventListener
    {
        private readonly VirtualKeyCode _key;
        private readonly VirtualKeyCode[] _additional;

        public ModifierKey(VirtualKeyCode key, params VirtualKeyCode[] additional)
        {
            _key = key;
            _additional = additional;
            if(_additional == null)
                _additional = new VirtualKeyCode[0];
        }

        public void KeyDown(System.Windows.Forms.KeyEventArgs args)
        {
            if ((int)args.KeyCode == (int)_key || _additional.Any(x => (int)x == (int)args.KeyCode))
            {
                IsActive = true;
            }
        }

        public void KeyPressed(System.Windows.Forms.KeyPressEventArgs character)
        {
        }

        public void KeyUp(System.Windows.Forms.KeyEventArgs args)
        {
            if ((int)args.KeyCode == (int)_key || _additional.Any(x => (int)x == (int)args.KeyCode))
            {
                IsActive = false;
            }
        }
    }
}
