using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPFKeyboard.Models;

namespace WPFKeyboard.Controls
{
    public class OnScreenKeyboardSection : Grid
    {
        readonly OnScreenKeyboard _onScreenKeyboard;
        OnScreenKeyboardSectionViewModel _viewModel;

        public OnScreenKeyboardSection(OnScreenKeyboard onScreenKeyboard)
        {
            _onScreenKeyboard = onScreenKeyboard;
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (_viewModel != null)
            {
                _viewModel.Rows.CollectionChanged -= OnRowsCollectionChanged;
                _viewModel = null;
                OnRowsCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            if (DataContext is OnScreenKeyboardSectionViewModel)
            {
                _viewModel = DataContext as OnScreenKeyboardSectionViewModel;
                _viewModel.Rows.CollectionChanged += OnRowsCollectionChanged;
                OnRowsCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        private void OnRowsCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (_viewModel == null) return;

            // remove extra children
            while (Children.Count > _viewModel.Rows.Count)
            {
                var child = Children[Children.Count - 1];
                if (child is FrameworkElement)
                    (child as FrameworkElement).DataContext = null;

                // remote the extra child and its row definition
                Children.Remove(child);
                RowDefinitions[RowDefinitions.Count - 1].ClearValue(RowDefinition.HeightProperty);
                RowDefinitions.RemoveAt(RowDefinitions.Count - 1);
            }

            // add enough row controls to match the number of row view models we have
            while (Children.Count < _viewModel.Rows.Count)
            {
                Children.Add(new OnScreenKeyboardRow(_onScreenKeyboard));
                RowDefinitions.Add(new RowDefinition());
            }

            // now that we have an exact number of rows, 
            // matching the exact number of controls,
            // update all the data contexts
            foreach (var row in _viewModel.Rows)
            {
                var index = _viewModel.Rows.IndexOf(row);
                var keyboardSectionRow = ((OnScreenKeyboardRow)Children[index]);
                keyboardSectionRow.DataContext = row;
                SetRow(keyboardSectionRow, index);
                RowDefinitions[index].SetBinding(RowDefinition.HeightProperty, new Binding("RowHeight") { Source = row });
            }
        }
    }
}
