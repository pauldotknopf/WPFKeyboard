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
            // TODO: Fix constructor!
            //VirtualKeyboard.DataContext = new KPDOnScreenKeyboardViewModel(KeyboardHelper.InstalledKeyboardLayouts["US"], new ModiferStateManager());
        }
    }
}
