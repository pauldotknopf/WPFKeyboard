using System.Windows;
namespace WPFKeyboard.Models
{
    public abstract class BaseOnScreenKeyViewModel : BaseViewModel
    {
        GridLength _buttonWidth = new GridLength(1, GridUnitType.Star);

        public GridLength ButtonWidth
        {
            get { return _buttonWidth; }
            set
            {
                _buttonWidth = value;
                RaisePropertyChanged("ButtonWidth");
            }
        } 
    }
}
