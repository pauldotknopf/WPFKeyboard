using Sample.KeyTemplates;
using WPFKeyboard.Keyboards;

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
            //VirtualKeyboard.DataContext = new KPDOnScreenKeyboardViewModel(@"C:\Windows\SysWOW64\KBDVNTC.DLL");
            //VirtualKeyboard.DataContext = new KPDOnScreenKeyboardViewModel(@"C:\Windows\SysWOW64\KBDUS.DLL");
            //VirtualKeyboard.DataContext = new KPDOnScreenKeyboardViewModel(@"C:\Windows\SysWOW64\KBDGR.DLL");
            VirtualKeyboard.DataContext = new KPDOnScreenKeyboardViewModel(@"C:\Windows\SysWOW64\KBDJPN.DLL");
        }
    }
}
