using Sample.KeyTemplates;
using WPFKeyboard;

namespace Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            VirtualKeyboard.OnScreenKeyControlBuilder = new SampleKeyControlBuilder();
            VirtualKeyboard.DataContext = new KPDOnScreenKeyboardViewModel();
            ((KPDOnScreenKeyboardViewModel)VirtualKeyboard.DataContext).Refresh(KeyboardHelper.InstalledKeyboardLayouts["Spanish"], new ModiferStateManager());
        }
    }
}
