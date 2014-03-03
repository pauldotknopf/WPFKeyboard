using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput.Native;
using WPFKeyboard.Models;

namespace WPFKeyboard
{
    /// <summary>
    /// A key that displays and enters a character
    /// </summary>
    public class CharacterKey : BaseOnScreenKeyViewModel, IButtonEventListener
    {
        private readonly VirtualKeyCode _virtualKey;

        public CharacterKey(VirtualKeyCode virtualKey)
        {
            _virtualKey = virtualKey;
        }

        #region IButtonEventListener

        /// <summary>
        /// The button was pressed down
        /// </summary>
        public void ButtonDown()
        {
            Keyboard.Simulator.Keyboard.KeyUp(_virtualKey);
        }

        /// <summary>
        /// The buttonw as pressed up
        /// </summary>
        public void ButtonUp()
        {
            Keyboard.Simulator.Keyboard.KeyUp(_virtualKey);
        }

        #endregion
    }
}
