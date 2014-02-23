using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using WindowsInput.Native;
using WPFKeyboard.Models;
using Binding = System.Windows.Data.Binding;

namespace WPFKeyboard.Controls
{
    public class OnScreenKeyboard : Grid
    {
        OnScreenKeyboardViewModel _viewModel;

        public OnScreenKeyboard()
        {
            DataContextChanged += OnDataContextChanged;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        #region Properties

        /// <summary>
        /// See OnScreenKeyStyle
        /// </summary>
        public static readonly DependencyProperty OnScreenKeyStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(OnScreenKeyboard), new PropertyMetadata(default(Style)));

        /// <summary>
        /// The style to be applied to the on screen key
        /// </summary>
        public Style OnScreenKeyStyle
        {
            get { return (Style)GetValue(OnScreenKeyStyleProperty); }
            set { SetValue(OnScreenKeyStyleProperty, value); }
        }

        /// <summary>
        /// See OnScreenKeyControlBuilder
        /// </summary>
        public static readonly DependencyProperty OnScreenKeyControlBuilderProperty =
            DependencyProperty.Register("OnScreenKeyControlBuilder", typeof(IOnScreenKeyControlBuilder), typeof(OnScreenKeyboard), new PropertyMetadata(new DefaultOnScreenKeyControlBuilder(), OnScreenKeyControlBuilderPropertyChangedCallback), OnScreenKeyControlBuilderValidateValueCallback);

        private static void OnScreenKeyControlBuilderPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var keyboard = (OnScreenKeyboard)dependencyObject;
            keyboard.RefreshKeyContentControls();
        }

        private static bool OnScreenKeyControlBuilderValidateValueCallback(object value)
        {
            if (value == null)
                return false;

            return value is IOnScreenKeyControlBuilder;
        }

        /// <summary>
        /// The control builder that is used to 
        /// </summary>
        public IOnScreenKeyControlBuilder OnScreenKeyControlBuilder
        {
            get { return (IOnScreenKeyControlBuilder)GetValue(OnScreenKeyControlBuilderProperty); }
            set { SetValue(OnScreenKeyControlBuilderProperty, value); }
        }

        #endregion

        #region Events

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (_viewModel != null)
            {
                _viewModel.Sections.CollectionChanged -= OnSectionsCollectionChanged;
                _viewModel = null;
                OnSectionsCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            if (DataContext is OnScreenKeyboardViewModel)
            {
                _viewModel = DataContext as OnScreenKeyboardViewModel;
                _viewModel.Sections.CollectionChanged += OnSectionsCollectionChanged;
                OnSectionsCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        private void OnSectionsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_viewModel == null) return;

            // remove extra children
            while (Children.Count > _viewModel.Sections.Count)
            {
                var child = (OnScreenKeyboardSection)Children[Children.Count - 1];
                child.DataContext = null;

                // remote the extra child and its column definition
                Children.Remove(child);
                ColumnDefinitions[ColumnDefinitions.Count - 1].ClearValue(ColumnDefinition.WidthProperty);
                ColumnDefinitions.RemoveAt(ColumnDefinitions.Count - 1);
            }

            // add enough section controls to match the number of section view models we have
            while (Children.Count < _viewModel.Sections.Count)
            {
                Children.Add(new OnScreenKeyboardSection(this));
                ColumnDefinitions.Add(new ColumnDefinition());
            }

            // now that we have an exact number of columns, 
            // matching the exact number of controls,
            // update all the data contexts
            foreach (var section in _viewModel.Sections)
            {
                var index = _viewModel.Sections.IndexOf(section);
                var keyboardSection = ((OnScreenKeyboardSection)Children[index]);
                keyboardSection.DataContext = section;
                SetColumn(keyboardSection, index);
                ColumnDefinitions[index].SetBinding(ColumnDefinition.WidthProperty, new Binding("SectionWidth") { Source = section });
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Helpers.IsInDesignMode) return;
            Keyboard.KeyDown += OnKeyDown;
            Keyboard.KeyPress += OnKeyPress;
            Keyboard.KeyUp += OnKeyUp;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Helpers.IsInDesignMode) return;
            Keyboard.KeyDown -= OnKeyDown;
            Keyboard.KeyPress -= OnKeyPress;
            Keyboard.KeyUp -= OnKeyUp;
        }

        private void OnKeyUp(object sender, System.Windows.Forms.KeyEventArgs args)
        {
            Debug.WriteLine((int)args.KeyCode);
            //PrintState();
            bool? isShifting = null;
            bool? isCapsLockOn = null;

            foreach (var section in _viewModel.Sections)
            {
                foreach (var row in section.Rows)
                {
                    foreach (var key in row.Keys)
                    {
                        if (key is IKeyEventListener)
                        {
                            if (!isShifting.HasValue)
                            {
                                isShifting = Keyboard.InputDeviceStateAdapter.IsKeyDown(VirtualKeyCode.SHIFT);
                                if (args.KeyCode == Keys.Shift 
                                    || args.KeyCode == Keys.LShiftKey 
                                    || args.KeyCode == Keys.RShiftKey)
                                    isShifting = false;
                            }
                            if (!isCapsLockOn.HasValue)
                                isCapsLockOn = Keyboard.InputDeviceStateAdapter.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL);
                            (key as IKeyEventListener).KeyUp(args, isShifting.Value, isCapsLockOn.Value);
                        }
                    }
                }
            }
        }

        private void OnKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs args)
        {
            //PrintState();
            bool? isShifting = null;
            bool? isCapsLockOn = null;

            foreach (var section in _viewModel.Sections)
            {
                foreach (var row in section.Rows)
                {
                    foreach (var key in row.Keys)
                    {
                        if (key is IKeyEventListener)
                        {
                            if (!isShifting.HasValue)
                                isShifting = Keyboard.InputDeviceStateAdapter.IsKeyDown(VirtualKeyCode.SHIFT);
                            if (!isCapsLockOn.HasValue)
                                isCapsLockOn = Keyboard.InputDeviceStateAdapter.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL);
                            (key as IKeyEventListener).KeyPressed(args, isShifting.Value, isCapsLockOn.Value);
                        }
                    }
                }
            }
        }

        private void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs args)
        {
            //PrintState();
            bool? isShifting = null;
            bool? isCapsLockOn = null;

            foreach (var section in _viewModel.Sections)
            {
                foreach (var row in section.Rows)
                {
                    foreach (var key in row.Keys)
                    {
                        if (key is IKeyEventListener)
                        {
                            if (!isShifting.HasValue)
                            {
                                isShifting = Keyboard.InputDeviceStateAdapter.IsKeyDown(VirtualKeyCode.SHIFT);
                                if (args.KeyCode == Keys.Shift
                                    || args.KeyCode == Keys.LShiftKey
                                    || args.KeyCode == Keys.RShiftKey)
                                    isShifting = true;
                            }
                            if (!isCapsLockOn.HasValue)
                                isCapsLockOn = Keyboard.InputDeviceStateAdapter.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL);
                            (key as IKeyEventListener).KeyDown(args, isShifting.Value, isCapsLockOn.Value);
                        }
                    }
                }
            }
        }

        private void PrintState()
        {
            Debug.WriteLine("IsKeyDown(VirtualKeyCode.SHIFT) == {0}", Keyboard.InputDeviceStateAdapter.IsKeyDown(VirtualKeyCode.SHIFT));
            Debug.WriteLine("IsKeyDown(VirtualKeyCode.RSHIFT) == {0}", Keyboard.InputDeviceStateAdapter.IsKeyDown(VirtualKeyCode.RSHIFT));
            Debug.WriteLine("IsKeyDown(VirtualKeyCode.LSHIFT) == {0}", Keyboard.InputDeviceStateAdapter.IsKeyDown(VirtualKeyCode.LSHIFT));
            Debug.WriteLine("IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL) == {0}", Keyboard.InputDeviceStateAdapter.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calling this method will rebuild the controls for every key
        /// </summary>
        private void RefreshKeyContentControls()
        {
            var controlBuilder = OnScreenKeyControlBuilder;
            foreach (OnScreenKeyboardSection section in Children)
                foreach (OnScreenKeyboardRow row in section.Children)
                    foreach (OnScreenKey key in row.Children)
                        key.Content = controlBuilder.BuildControlForKey(key.ViewModel);
        }

        /// <summary>
        /// Build the control that is to be used for the given key view model
        /// </summary>
        /// <param name="keyViewModel">The key view model.</param>
        /// <returns></returns>
        public object BuildContentControlForKey(BaseOnScreenKeyViewModel keyViewModel)
        {
            return OnScreenKeyControlBuilder.BuildControlForKey(keyViewModel);
        }

        #endregion


    }
}
