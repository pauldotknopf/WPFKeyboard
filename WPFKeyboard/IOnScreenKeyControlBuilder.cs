using WPFKeyboard.Models;

namespace WPFKeyboard
{
    /// <summary>
    /// Contract for building the content for keys.
    /// This allows use to change the template for the keys
    /// </summary>
    public interface IOnScreenKeyControlBuilder
    {
        /// <summary>
        /// Build a WPF control to use as the template for the given key view model
        /// </summary>
        /// <param name="keyViewModel">The key view model.</param>
        /// <returns></returns>
        object BuildControlForKey(BaseOnScreenKeyViewModel keyViewModel);
    }
}
