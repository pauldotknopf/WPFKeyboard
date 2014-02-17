using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WPFKeyboard.Models;

namespace Sample.KeyTemplates
{
    public class SampleKeyControlBuilder : WPFKeyboard.IOnScreenKeyControlBuilder
    {
        public object BuildControlForKey(BaseOnScreenKeyViewModel keyViewModel)
        {
            var template = new DefaultKeyTemplate();
            template.DataContext = keyViewModel;
            return template;
        }
    }
}
