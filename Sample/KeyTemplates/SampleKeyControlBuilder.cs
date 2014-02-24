using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WindowsInput.Native;
using WPFKeyboard.Models;

namespace Sample.KeyTemplates
{
    public class SampleKeyControlBuilder : WPFKeyboard.IOnScreenKeyControlBuilder
    {
        public object BuildControlForKey(BaseOnScreenKeyViewModel keyViewModel)
        {
            var virtualKeyViewModel = keyViewModel as VirtualKey;
            if (virtualKeyViewModel != null)
            {
                if (virtualKeyViewModel.Key == VirtualKeyCode.LWIN || virtualKeyViewModel.Key == VirtualKeyCode.RWIN)
                {
                    return new WindowsKeyTemplate()
                    {
                        DataContext = keyViewModel
                    };
                }
            }
            var template = new DefaultKeyTemplate
            {
                DataContext = keyViewModel
            };
            return template;
        }
    }
}
