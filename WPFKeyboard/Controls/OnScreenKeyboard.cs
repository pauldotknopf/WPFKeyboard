using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPFKeyboard.Models;

namespace WPFKeyboard.Controls
{
    public class OnScreenKeyboard : Grid
    {
        OnScreenKeyboardViewModel _viewModel;

        public OnScreenKeyboard()
        {
            DataContextChanged += OnDataContextChanged;
        }

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
                Children.Add(new OnScreenKeyboardSection());
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
    }
}
