using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFKeyboard.Models
{
    public class OnScreenKeyboardRowViewModel : BaseViewModel
    {
        GridLength _rowHeight = new GridLength(1, GridUnitType.Star);
        ObservableCollection<BaseOnScreenKeyViewModel> _keys = new ObservableCollection<BaseOnScreenKeyViewModel>();

        public GridLength RowHeight
        {
            get { return _rowHeight; }
            set
            {
                _rowHeight = value;
                RaisePropertyChanged("RowHeight");
            }
        }

        public ObservableCollection<BaseOnScreenKeyViewModel> Keys
        {
            get { return _keys; }
        }
    }
}
