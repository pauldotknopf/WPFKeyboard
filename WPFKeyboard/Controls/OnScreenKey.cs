using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPFKeyboard.Models;

namespace WPFKeyboard.Controls
{
    /// <summary>
    /// This is the button that represents a key on our keyboard
    /// </summary>
    public class OnScreenKey : Button
    {
        readonly OnScreenKeyboard _onScreenKeyboard;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnScreenKey"/> class.
        /// </summary>
        public OnScreenKey(OnScreenKeyboard onScreenKeyboard)
        {
            _onScreenKeyboard = onScreenKeyboard;
            DataContextChanged += OnDataContextChanged;
            Focusable = false;
            SetBinding(IsActiveProperty, new Binding("IsActive"));
            SetBinding(StyleProperty, new Binding("OnScreenKeyStyle") { Source = _onScreenKeyboard });
            PreviewMouseDown += OnMouseDown;
            Click += OnClick;
        }

        #region Properties

        /// <summary>
        /// See IsActive
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(OnScreenKey), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Is the key currently active (pressed)?
        /// </summary>
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        /// <summary>
        /// The key view model for this control
        /// </summary>
        public BaseOnScreenKeyViewModel ViewModel { get { return DataContext as BaseOnScreenKeyViewModel; } }

        #endregion

        #region Events

        /// <summary>
        /// This is raised when the button is released/clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedEventArgs"></param>
        private void OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var buttonEvent = DataContext as IButtonEventListener;
            if (buttonEvent != null)
                buttonEvent.ButtonUp();
        }

        /// <summary>
        /// This is raised when the mouse is pressed (but not released)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var buttonEvent = DataContext as IButtonEventListener;
            if (buttonEvent != null)
                buttonEvent.ButtonDown();
        }

        /// <summary>
        /// This is raised when the data context changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Content = ViewModel != null ? _onScreenKeyboard.BuildContentControlForKey(ViewModel) : null;
        }

        protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsPressedChanged(e);
            if (ViewModel != null)
            {
                ViewModel.IsActive = IsPressed;
            }
        }

        #endregion
    }
}
