using System.Collections.Generic;
using WindowsInput.Native;

namespace WPFKeyboard
{
    public interface IModiferStateManager
    {
        /// <summary>
        /// Set the modifier keys and the bits that they represent so that we can build the "ModifierState" for external usage
        /// </summary>
        /// <param name="modifierKeys"></param>
        void SetModifierKeys(Dictionary<int, VirtualKeyCode> modifierKeys);

        /// <summary>
        /// Refresh the "ModifierState". This will go through each of the modifiers set using SetModifierKeys, and determine if they are in effect.
        /// If they are, its matching bit will be turned on within the "ModifierState" property.
        /// </summary>
        /// <param name="keyUp"></param>
        /// <param name="keyDown"></param>
        void Refresh(VirtualKeyCode? keyUp = null, VirtualKeyCode? keyDown = null);

        /// <summary>
        /// The current modifer state. This is used to determine which key to display, based on the current modifiers.
        /// </summary>
        int ModifierState { get; }
    }
}