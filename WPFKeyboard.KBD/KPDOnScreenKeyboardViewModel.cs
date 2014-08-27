using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WindowsInput.Native;
using WPFKeyboard.Models;
using WPFKeyboardNative;

namespace WPFKeyboard
{
    /// <summary>
    /// This view model uses a KPD dll to build the keyboard layout.
    /// Supporting KPD files allows us to instantly support every windows language.
    /// </summary>
    public class KPDOnScreenKeyboardViewModel : OnScreenKeyboardViewModel
    {
        #region Fields

        KeyboardLayout _keyboardLayout;
        ReadOnlyCollection<int> _modifierBitsSortedByIndex;

        #endregion

        #region Properties

        public KeyboardLayout KeyboardLayout
        {
            get { return _keyboardLayout; }
        }

        public IList<int> ModifierBitsSortedByIndex
        {
            get { return _modifierBitsSortedByIndex; }
        }

        #endregion

        #region Methods

        public void Refresh(InstalledKeyboardLayout installedKeyboardLayout, IModiferStateManager modiferStateManager)
        {
            _keyboardLayout = KeyboardLayoutHelper.GetLayout(string.Format(((IntPtr.Size == 8) || NativeMethods.InternalCheckIsWow64())
                ? @"C:\Windows\SysWOW64\{0}"
                : @"C:\Windows\System32\{0}", installedKeyboardLayout.LayoutFile));
            ModiferStateManager = modiferStateManager;
            ModiferStateManager.SetModifierKeys(_keyboardLayout.CharModifiers.ToDictionary(x => x.ModifierBits,
                x => (VirtualKeyCode)x.VirtualKey));

            BuildKeyboardLayout();
        }

        #region Private

        private void BuildKeyboardLayout()
        {
            _modifierBitsSortedByIndex = new ReadOnlyCollection<int>(KeyboardLayout.ModifierBits
                .Select(x => new
                {
                    ModBits = KeyboardLayout.ModifierBits.IndexOf(x),
                    Index = x
                })
                .Where(x => x.Index != Constants.ShftInvalid)
                .OrderBy(x => x.Index)
                .Select(x => x.ModBits)
                .ToList());

            Sections.Clear();
            var mainSection = new OnScreenKeyboardSectionViewModel();
            mainSection.Rows.Add(BuildRow1());
            mainSection.Rows.Add(BuildRow2());
            mainSection.Rows.Add(BuildRow3());
            mainSection.Rows.Add(BuildRow4());
            mainSection.Rows.Add(BuildRow5());
            Sections.Add(mainSection);
        }

        private OnScreenKeyboardRowViewModel BuildRow1()
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x29));             // ~
            row.Keys.Add(KeyForScanCode(0x02));             // 1
            row.Keys.Add(KeyForScanCode(0x03));             // 2
            row.Keys.Add(KeyForScanCode(0x04));             // 3
            row.Keys.Add(KeyForScanCode(0x05));             // 4
            row.Keys.Add(KeyForScanCode(0x06));             // 5 
            row.Keys.Add(KeyForScanCode(0x07));             // 6
            row.Keys.Add(KeyForScanCode(0x08));             // 7
            row.Keys.Add(KeyForScanCode(0x09));             // 8
            row.Keys.Add(KeyForScanCode(0x0A));             // 9
            row.Keys.Add(KeyForScanCode(0x0B));             // 0
            row.Keys.Add(KeyForScanCode(0x0C));             // -
            row.Keys.Add(KeyForScanCode(0x0D));             // +
            row.Keys.Add(KeyForScanCode(0x0E, 20));         // backspace
            return row;
        }

        private OnScreenKeyboardRowViewModel BuildRow2()
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x0F, 15));         // tab
            row.Keys.Add(KeyForScanCode(0x10));             // Q
            row.Keys.Add(KeyForScanCode(0x11));             // W
            row.Keys.Add(KeyForScanCode(0x12));             // E
            row.Keys.Add(KeyForScanCode(0x13));             // R
            row.Keys.Add(KeyForScanCode(0x14));             // T
            row.Keys.Add(KeyForScanCode(0x15));             // Y
            row.Keys.Add(KeyForScanCode(0x16));             // U
            row.Keys.Add(KeyForScanCode(0x17));             // I
            row.Keys.Add(KeyForScanCode(0x18));             // O
            row.Keys.Add(KeyForScanCode(0x19));             // P
            row.Keys.Add(KeyForScanCode(0x1A));             // [
            row.Keys.Add(KeyForScanCode(0x1B));             // ]
            row.Keys.Add(KeyForScanCode(0x2B, 15));         // \
            return row;
        }

        private OnScreenKeyboardRowViewModel BuildRow3()
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x3A, 17));         // caps lock
            row.Keys.Add(KeyForScanCode(0x1E));             // A
            row.Keys.Add(KeyForScanCode(0x1F));             // S
            row.Keys.Add(KeyForScanCode(0x20));             // D
            row.Keys.Add(KeyForScanCode(0x21));             // F
            row.Keys.Add(KeyForScanCode(0x22));             // G
            row.Keys.Add(KeyForScanCode(0x23));             // H
            row.Keys.Add(KeyForScanCode(0x24));             // J
            row.Keys.Add(KeyForScanCode(0x25));             // K
            row.Keys.Add(KeyForScanCode(0x26));             // L
            row.Keys.Add(KeyForScanCode(0x27));             // ;
            row.Keys.Add(KeyForScanCode(0x28));             // '
            row.Keys.Add(KeyForScanCode(0x1C, 21));         // enter
            return row;
        }

        private OnScreenKeyboardRowViewModel BuildRow4()
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x2A, 21));         // left shift
            row.Keys.Add(KeyForScanCode(0x2C));             // Z
            row.Keys.Add(KeyForScanCode(0x2D));             // X
            row.Keys.Add(KeyForScanCode(0x2E));             // C
            row.Keys.Add(KeyForScanCode(0x2F));             // V
            row.Keys.Add(KeyForScanCode(0x30));             // B
            row.Keys.Add(KeyForScanCode(0x31));             // N
            row.Keys.Add(KeyForScanCode(0x32));             // M
            row.Keys.Add(KeyForScanCode(0x33));             // ,
            row.Keys.Add(KeyForScanCode(0x34));             // .
            row.Keys.Add(KeyForScanCode(0x35));             // /
            row.Keys.Add(KeyForScanCode(0x36, 21));         // right shift
            return row;
        }

        private OnScreenKeyboardRowViewModel BuildRow5()
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x1D));             // right control
            row.Keys.Add(KeyForScanCode(0x5B, isE0: true)); // left windows
            row.Keys.Add(KeyForScanCode(0x38));             // left alt
            row.Keys.Add(KeyForScanCode(0x39, 45));         // space bar
            row.Keys.Add(KeyForScanCode(0x38, isE0: true)); // right alt
            row.Keys.Add(KeyForScanCode(0x5C, isE0: true)); // right windows
            row.Keys.Add(KeyForScanCode(0x5D, isE0: true)); // menu
            row.Keys.Add(KeyForScanCode(0x1D, isE0: true)); // right control
            return row;
        }

        private BaseOnScreenKeyViewModel KeyForScanCode(int scanCode, int widthWeight = 10, bool isE0 = false, bool isE1 = false)
        {
            var sc = KeyboardLayout.ScanCodes.FirstOrDefault(x => x.Code == scanCode && x.E0Set == isE0 && x.E1Set == isE1);

            if (sc == null)
                throw new Exception(string.Format("The scan code {0:X} isn't valid.", scanCode));

            var virtualKey = (VirtualKeyCode)(sc.VirtualKey & 0xFF);

            var virtualKeyInfo = KeyboardLayout.VirtualKeys.SingleOrDefault(x => x.Key == sc.VirtualKey);

            var scanCodeText = KeyboardLayout.CodeText.SingleOrDefault(x => x.ScanCode == scanCode);


            return new VirtualKey(
                this,
                virtualKey,
                scanCodeText != null ? scanCodeText.Text : null,
                virtualKeyInfo != null ? virtualKeyInfo.Characters.ToList() : new List<int>(),
                virtualKeyInfo != null && virtualKeyInfo.Attributes == 1)
                {
                    ButtonWidth = new GridLength(widthWeight, GridUnitType.Star)
                };
        }

        #endregion

        #endregion


    }
}
