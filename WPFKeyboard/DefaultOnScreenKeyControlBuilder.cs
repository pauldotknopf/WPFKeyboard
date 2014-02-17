using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFKeyboard
{
    /// <summary>
    /// The default control builder for the keys.
    /// It builds a simple textbox that binds the to the Display property of the base key view model.
    /// </summary>
    public class DefaultOnScreenKeyControlBuilder : IOnScreenKeyControlBuilder
    {
        /// <summary>
        /// Build a WPF control to use as the template for the given key view model
        /// </summary>
        /// <param name="keyViewModel">The key view model.</param>
        /// <returns></returns>
        public object BuildControlForKey(Models.BaseOnScreenKeyViewModel keyViewModel)
        {
            var textBlock = new TextBlock();
            textBlock.SetBinding(TextBlock.TextProperty, new Binding("Display"));
            return textBlock;
        }
    }
}
