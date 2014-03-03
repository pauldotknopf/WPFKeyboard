using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFKeyboard
{
    public class ModiferState
    {
        private Dictionary<int, bool> _state = new Dictionary<int, bool>(); 

        public ModiferState(IEnumerable<int> modiferVirtualKeys)
        {
            foreach (var modiferVirtualKey in modiferVirtualKeys)
                _state.Add(modiferVirtualKey, false);
            Refresh();
        }

        public void Refresh()
        {
            foreach (var virtualKey in _state.Keys)
            {
                //_state.
            }
        }
    }
}
