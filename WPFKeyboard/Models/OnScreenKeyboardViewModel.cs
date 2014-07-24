using System.Collections.ObjectModel;

namespace WPFKeyboard.Models
{
    public class OnScreenKeyboardViewModel
    {
        readonly ObservableCollection<OnScreenKeyboardSectionViewModel> _sections = new ObservableCollection<OnScreenKeyboardSectionViewModel>();

        public ObservableCollection<OnScreenKeyboardSectionViewModel> Sections
        {
            get { return _sections; }
        }

        public IModiferStateManager ModiferStateManager { get; protected set; }
    }
}
