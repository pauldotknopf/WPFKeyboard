using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using WindowsInput.Native;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;

namespace WPFKeyboard.Models
{
    public class ShiftSensitiveKey : BaseOnScreenKeyViewModel, IKeyEventListener
    {
        private readonly VirtualKeyCode _keyCode;
        private readonly string _normal;
        private readonly string _modified;
        private readonly string _displayText;

        public ShiftSensitiveKey(VirtualKeyCode keyCode, string normal, string modified, string displayText)
        {
            _keyCode = keyCode;
            _normal = normal;
            _modified = modified;
            _displayText = displayText;
            Display = !string.IsNullOrEmpty(_displayText) ? _displayText : _normal;
        }

        #region IKeyEventListener

        public void KeyDown(KeyEventArgs args)
        {
            if (args.KeyCode == Keys.ShiftKey || args.KeyCode == Keys.LShiftKey || args.KeyCode == Keys.RShiftKey)
            {
                Display = !string.IsNullOrEmpty(_displayText) ? _displayText : _modified;
            }else if ((int)args.KeyCode == (int)_keyCode)
            {
                IsActive = true;
            }
        }

        public void KeyPressed(KeyPressEventArgs character)
        {

        }

        public void KeyUp(KeyEventArgs args)
        {
            if (args.KeyCode == Keys.ShiftKey || args.KeyCode == Keys.LShiftKey || args.KeyCode == Keys.RShiftKey)
            {
                Display = !string.IsNullOrEmpty(_displayText) ? _displayText : _normal;
            }
            else if ((int)args.KeyCode == (int)_keyCode)
            {
                IsActive = false;
            }
        }

        #endregion
    }
}
