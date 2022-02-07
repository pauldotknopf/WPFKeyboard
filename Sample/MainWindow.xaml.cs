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
            var handle = ((NativeMethods.GetKeyboardLayout(0).ToInt32() >> 16) & 0xFFFF).ToString("x8");
            ((KPDOnScreenKeyboardViewModel)VirtualKeyboard.DataContext).Refresh(KeyboardHelper.InstalledKeyboardLayouts[handle], new ModiferStateManager());
        }
    }
}
