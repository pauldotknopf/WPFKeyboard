using System.Collections.ObjectModel;
using System.Windows;

namespace WPFKeyboard.Models
{
    public class OnScreenKeyboardSectionViewModel : BaseViewModel
    {
        GridLength _sectionWidth = new GridLength(1, GridUnitType.Star);
        readonly ObservableCollection<OnScreenKeyboardRowViewModel> _rows = new ObservableCollection<OnScreenKeyboardRowViewModel>();

        public GridLength SectionWidth
        {
            get { return _sectionWidth; }
            set
            {
                _sectionWidth = value;
                RaisePropertyChanged("SectionWidth");
            }
        }

        public ObservableCollection<OnScreenKeyboardRowViewModel> Rows
        {
            get { return _rows; }
        }
    }
}
