using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace WPFKeyboard.Models
{
    public abstract class BaseOnScreenKeyViewModel : BaseViewModel
    {
        GridLength _buttonWidth = new GridLength(1, GridUnitType.Star);
        string _display;
        bool _isActive;
        private object _content;

        public GridLength ButtonWidth
        {
            get { return _buttonWidth; }
            set
            {
                _buttonWidth = value;
                RaisePropertyChanged("ButtonWidth");
            }
        }

        public string Display
        {
            get { return _display; }
            set
            {
                _display = value;
                RaisePropertyChanged("Display");
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                RaisePropertyChanged("IsActive");
            }
        }
    }
}
