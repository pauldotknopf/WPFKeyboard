using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sample
{
    /// <summary>
    /// Interaction logic for Form.xaml
    /// </summary>
    public partial class Form
    {
        readonly PopupKeyboard _keyboardWindow = new PopupKeyboard();

        public Form()
        {
            InitializeComponent();
            Keyboard.AddGotKeyboardFocusHandler(this, (sender, args) =>
            {
                var current = args.NewFocus as FrameworkElement;
                if (current != null && current.GetType().IsAssignableFrom(typeof(TextBox)))
                {
                    _keyboardWindow.Show(current);
                }
                else
                {
                    _keyboardWindow.Hide();
                }
            });
            IsKeyboardFocusedChanged += OnIsKeyboardFocusedChanged;
            Deactivated += OnDeactivated;
        }

        private void OnDeactivated(object sender, EventArgs x)
        {
            _keyboardWindow.Hide();
        }

        private void OnIsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
        }
    }
}
