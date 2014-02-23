using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsInput;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;

namespace WPFKeyboard
{
    /// <summary>
    /// Static class that allows us to easily listen to keyboard events without managing hooks/listeners, and also has the static instance of the input simulator
    /// </summary>
    public static class Keyboard
    {
        private static KeyboardHookListener _keyboardHookListener;
        private static InputSimulator _inputSimulator = new InputSimulator();
        private static IInputDeviceStateAdaptor _inputDeviceStateAdaptor = new WindowsInputDeviceStateAdaptor();
        private static readonly object Lock = new object();

        /// <summary>
        /// Internal method that ensures that our hook is created if it isn't already
        /// </summary>
        private static void EnsureHookCreated()
        {
            lock (Lock)
            {
                if (_keyboardHookListener == null)
                {
                    _keyboardHookListener = new KeyboardHookListener(new GlobalHooker());
                    _keyboardHookListener.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Get the input simulator
        /// </summary>
        public static InputSimulator Simulator { get { return _inputSimulator; } }

        /// <summary>
        /// Get the input device state adapter
        /// </summary>
        public static IInputDeviceStateAdaptor InputDeviceStateAdapter { get { return _inputDeviceStateAdaptor; } }

        /// <summary>
        /// Occurs when a key is pressed. 
        /// </summary>
        public static event KeyEventHandler KeyDown
        {
            add
            {
                EnsureHookCreated();
                _keyboardHookListener.KeyDown += value;
            }
            remove { _keyboardHookListener.KeyDown -= value; }
        }

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
        public static event KeyPressEventHandler KeyPress
        {
            add
            {
                EnsureHookCreated();
                _keyboardHookListener.KeyPress += value;
            }
            remove { _keyboardHookListener.KeyPress -= value; }
        }

        /// <summary>
        /// Occurs when a key is released. 
        /// </summary>
        public static event KeyEventHandler KeyUp
        {
            add
            {
                EnsureHookCreated();
                _keyboardHookListener.KeyUp += value;
            }
            remove { _keyboardHookListener.KeyUp -= value; }
        }
    }
}
