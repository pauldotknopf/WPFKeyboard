using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFKeyboard
{
    /// <summary>
    /// Key view models that implement this interface will have methods that get called when the button is pressed/released
    /// </summary>
    public interface IButtonEventListener
    {
        /// <summary>
        /// The button was pressed down
        /// </summary>
        void ButtonDown();

        /// <summary>
        /// The buttonw as pressed up
        /// </summary>
        void ButtonUp();
    }
}
