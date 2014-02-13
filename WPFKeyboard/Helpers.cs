using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFKeyboard
{
    public static class Helpers
    {
        private static bool? _isInDesignMode;

        /// <summary>
        /// Gets a value indicating whether the control is in design mode (running in Blend
        /// or Visual Studio).
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    _isInDesignMode
                    = (bool)DependencyPropertyDescriptor
                    .FromProperty(prop, typeof(FrameworkElement))
                    .Metadata.DefaultValue;

                    if (!_isInDesignMode.Value)
                        if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.StartsWith(@"devenv"))
                            _isInDesignMode = true;

                }
                return _isInDesignMode.Value;
            }
        }
    }
}
