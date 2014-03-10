using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFKeyboard
{
    public class InstalledKeyboardLayout
    {
        public InstalledKeyboardLayout(string locale, string layoutDisplayName, string layoutFile, string layoutText)
        {
            Locale = locale;
            LayoutDisplayName = layoutDisplayName;
            LayoutFile = layoutFile;
            LayoutText = layoutText;
        }

        public string Locale { get; private set; }

        public string LayoutDisplayName { get; private set; }

        public string LayoutFile { get; private set; }

        public string LayoutText { get; private set; }
    }
}
