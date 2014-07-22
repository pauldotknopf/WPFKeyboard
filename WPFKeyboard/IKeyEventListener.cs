using System.Windows.Forms;

namespace WPFKeyboard
{
    /// <summary>
    /// Key view models that implement this interface will receive notifications when a key has been pressed
    /// </summary>
    public interface IKeyEventListener
    {
        /// <summary>
        /// Occurs when a key is pressed.
        /// </summary>
        /// <param name="args">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        /// <param name="modiferState">State of the modifer.</param>
        /// <param name="isVirtual">if set to <c>true</c> [is virtual].</param>
        void KeyDown(KeyEventArgs args, IModiferStateManager modiferState, bool isVirtual);

        /// <summary>
        /// Occurs when a key is released.
        /// </summary>
        /// <param name="args">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        /// <param name="modiferState">State of the modifer.</param>
        /// <param name="isVirtual">if set to <c>true</c> [is virtual].</param>
        void KeyUp(KeyEventArgs args, IModiferStateManager modiferState, bool isVirtual);
    }
}
