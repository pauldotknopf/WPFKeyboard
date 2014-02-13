using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WPFKeyboard.Models
{
    public class OnScreenKeyboardViewModel
    {
        readonly ObservableCollection<OnScreenKeyboardSectionViewModel> _sections = new ObservableCollection<OnScreenKeyboardSectionViewModel>();

        public ObservableCollection<OnScreenKeyboardSectionViewModel> Sections
        {
            get { return _sections; }
        }
    }
}
