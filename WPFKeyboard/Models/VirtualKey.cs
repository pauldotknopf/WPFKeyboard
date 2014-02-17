using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;
using WindowsInput.Native;

namespace WPFKeyboard.Models
{
    public class VirtualKey : BaseOnScreenKeyViewModel, IButtonEventListener, IKeyEventListener
    {
        private readonly VirtualKeyCode _virtualKey;

        public VirtualKey(VirtualKeyCode virtualKey)
        {
            _virtualKey = virtualKey;
            Display = "Esc";
        }

        public void ButtonDown()
        {

        }

        public void ButtonUp()
        {
            Keyboard.Simulator.Keyboard.KeyPress(_virtualKey);
        }

        public void KeyDown(System.Windows.Forms.KeyEventArgs args)
        {
            if ((int)args.KeyCode == (int)_virtualKey)
            {
                IsActive = true;
            }
        }

        public void KeyPressed(System.Windows.Forms.KeyPressEventArgs character)
        {
        }

        public void KeyUp(System.Windows.Forms.KeyEventArgs args)
        {
            if ((int)args.KeyCode == (int)_virtualKey)
            {
                IsActive = false;
            }
        }
    }
}
