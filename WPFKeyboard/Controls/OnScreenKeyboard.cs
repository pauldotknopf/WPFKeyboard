using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Navigation;
using WPFKeyboard.Models;
using Application = System.Windows.Application;
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
            DependencyProperty.Register("ButtonStyle", typeof (Style), typeof (OnScreenKeyboard), new PropertyMetadata(default(Style)));

        /// <summary>
        /// The style to be applied to the on screen key
        /// </summary>
        public Style OnScreenKeyStyle
        {
            get { return (Style)GetValue(OnScreenKeyStyleProperty); }
            set { SetValue(OnScreenKeyStyleProperty, value); }
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
            if(Helpers.IsInDesignMode) return;
            Keyboard.KeyDown += OnKeyDown;
            Keyboard.KeyPress += OnKeyPress;
            Keyboard.KeyUp += OnKeyUp;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if(Helpers.IsInDesignMode) return;
            Keyboard.KeyDown -= OnKeyDown;
            Keyboard.KeyPress -= OnKeyPress;
            Keyboard.KeyUp -= OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs args)
        {
            foreach (var section in _viewModel.Sections)
            {
                foreach (var row in section.Rows)
                {
                    foreach (var key in row.Keys)
                    {
                        if(key is IKeyEventListener)
                            (key as IKeyEventListener).KeyUp(args);
                    }
                }
            }
        }

        private void OnKeyPress(object sender, KeyPressEventArgs args)
        {
            foreach (var section in _viewModel.Sections)
            {
                foreach (var row in section.Rows)
                {
                    foreach (var key in row.Keys)
                    {
                        if (key is IKeyEventListener)
                            (key as IKeyEventListener).KeyPressed(args);
                    }
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs args)
        {
            foreach (var section in _viewModel.Sections)
            {
                foreach (var row in section.Rows)
                {
                    foreach (var key in row.Keys)
                    {
                        if (key is IKeyEventListener)
                            (key as IKeyEventListener).KeyDown(args);
                    }
                }
            }
        }

        #endregion
    }
}
