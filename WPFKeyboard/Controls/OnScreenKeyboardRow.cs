using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPFKeyboard.Models;

namespace WPFKeyboard.Controls
{
    public class OnScreenKeyboardRow : Grid
    {
        readonly OnScreenKeyboard _onScreenKeyboard;
        OnScreenKeyboardRowViewModel _viewModel;

        public OnScreenKeyboardRow(OnScreenKeyboard onScreenKeyboard)
        {
            _onScreenKeyboard = onScreenKeyboard;
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (_viewModel != null)
            {
                _viewModel.Keys.CollectionChanged -= OnKeysCollectionChanged;
                _viewModel = null;
                OnKeysCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            if (DataContext is OnScreenKeyboardRowViewModel)
            {
                _viewModel = DataContext as OnScreenKeyboardRowViewModel;
                _viewModel.Keys.CollectionChanged += OnKeysCollectionChanged;
                OnKeysCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        private void OnKeysCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (_viewModel == null) return;

            // remove extra children
            while (Children.Count > _viewModel.Keys.Count)
            {
                var child = (OnScreenKey)Children[Children.Count - 1];
                child.DataContext = null;

                // remote the extra child and its column definition
                Children.Remove(child);
                ColumnDefinitions[ColumnDefinitions.Count - 1].ClearValue(ColumnDefinition.WidthProperty);
                ColumnDefinitions.RemoveAt(ColumnDefinitions.Count - 1);
            }

            // add enough row controls to match the number of key view models we have
            while (Children.Count < _viewModel.Keys.Count)
            {
                Children.Add(new OnScreenKey(_onScreenKeyboard));
                ColumnDefinitions.Add(new ColumnDefinition());
            }

            // now that we have an exact number of columns, 
            // matching the exact number of controls,
            // update all the data contexts
            foreach (var key in _viewModel.Keys)
            {
                var index = _viewModel.Keys.IndexOf(key);
                var control = ((OnScreenKey)Children[index]);
                control.DataContext = key;
                SetColumn(control, index);
                ColumnDefinitions[index].SetBinding(ColumnDefinition.WidthProperty, new Binding("ButtonWidth") { Source = key });
            }
        }
    }
}
