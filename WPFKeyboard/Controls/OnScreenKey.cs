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
        private bool _isActive;

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
            PreviewMouseDown += OnPreviewMouseDown;
            PreviewMouseUp += OnPreviewMouseUp;
            IsMouseDirectlyOverChanged += OnIsMouseDirectlyOverChanged;
            PreviewTouchDown += OnPreviewTouchDown;
            PreviewTouchUp += OnPreviewTouchUp;
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
        /// Raised when the user leaves the button.
        /// This is used to detect if the user has pressed the button, but slide the mouse out so that the "up" event never gets raised, leaving a key pressed continously.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!IsMouseDirectlyOver && _isActive)
            {
                _isActive = false;
                var buttonEvent = DataContext as IButtonEventListener;
                if (buttonEvent != null)
                {
                    _onScreenKeyboard.IsVirtual = true;
                    buttonEvent.ButtonUp();
                    _onScreenKeyboard.IsVirtual = false;
                }
            }
        }

        /// <summary>
        /// Raised when the user clicks the mouse up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void OnPreviewMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (_isActive)
            {
                _isActive = false;
                var buttonEvent = DataContext as IButtonEventListener;
                if (buttonEvent != null)
                {
                    _onScreenKeyboard.IsVirtual = true;
                    buttonEvent.ButtonUp();
                    _onScreenKeyboard.IsVirtual = false;
                }
            }
        }

        /// <summary>
        /// This is raised when the mouse is pressed (but not released)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseButtonEventArgs"></param>
        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (!_isActive)
            {
                _isActive = true;
                var buttonEvent = DataContext as IButtonEventListener;
                if (buttonEvent != null)
                {
                    _onScreenKeyboard.IsVirtual = true;
                    buttonEvent.ButtonDown();
                    _onScreenKeyboard.IsVirtual = false;
                }
            }
        }

        private void OnPreviewTouchUp(object sender, TouchEventArgs touchEventArgs)
        {
            if (_isActive)
            {
                _isActive = false;
                var buttonEvent = DataContext as IButtonEventListener;
                if (buttonEvent != null)
                {
                    _onScreenKeyboard.IsVirtual = true;
                    buttonEvent.ButtonUp();
                    _onScreenKeyboard.IsVirtual = false;
                }
            }
        }

        private void OnPreviewTouchDown(object sender, TouchEventArgs touchEventArgs)
        {
            if (!_isActive)
            {
                _isActive = true;
                var buttonEvent = DataContext as IButtonEventListener;
                if (buttonEvent != null)
                {
                    _onScreenKeyboard.IsVirtual = true;
                    buttonEvent.ButtonDown();
                    _onScreenKeyboard.IsVirtual = false;
                }
            }
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

        #endregion
    }
}
