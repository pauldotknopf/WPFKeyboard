using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WPFKeyboard
{
    /// <summary>
    /// Key view models that implement this interface will receive notifications when a key has been pressed
    /// </summary>
    public interface IKeyEventListener
    {
        /// <summary>
        /// Occurs when a key is pressed. 
        /// </summary>
        void KeyDown(KeyEventArgs args);

        /// <summary>
        /// Occurs when a key is pressed.
        /// </summary>
        /// <remarks>
        /// Key events occur in the following order: 
        /// <list type="number">
        /// <item>KeyDown</item>
        /// <item>KeyPress</item>
        /// <item>KeyUp</item>
        /// </list>
        /// The KeyPress event is not raised by non-character keys; however, the non-character keys do raise the KeyDown and KeyUp events. 
        /// Use the KeyChar property to sample keystrokes at run time and to consume or modify a subset of common keystrokes. 
        /// To handle keyboard events only in your application and not enable other applications to receive keyboard events, 
        /// set the <see cref="KeyPressEventArgs.Handled"/> property in your form's KeyPress event-handling method to <b>true</b>. 
        /// </remarks>
        void KeyPressed(KeyPressEventArgs character);

        /// <summary>
        /// Occurs when a key is released. 
        /// </summary>
        void KeyUp(KeyEventArgs args);
    }
}
