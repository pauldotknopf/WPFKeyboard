using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using WindowsInput.Native;
using WPFKeyboard.Models;
using WPFKeyboardNative;

namespace WPFKeyboard.Keyboards
{
    /// <summary>
    /// This view model uses a KPD dll to build the keyboard layout.
    /// Supporting KPD files allows us to instantly support every windows language.
    /// </summary>
    public class KPDOnScreenKeyboardViewModel : OnScreenKeyboardViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KPDOnScreenKeyboardViewModel"/> class.
        /// </summary>
        /// <param name="kpdFileLocation">The KPD file location.</param>
        public KPDOnScreenKeyboardViewModel(string kpdFileLocation)
        {
            BuildKeyboardLayout(KeyboardLayoutHelper.GetLayout(kpdFileLocation));
        }

        /// <summary>
        /// Build the keyboard layout based on the given class that represents the KPD file given in the constructor.
        /// </summary>
        /// <param name="keyboardLayout">The keyboard layout.</param>
        public void BuildKeyboardLayout(KeyboardLayout keyboardLayout)
        {
            var mainSection = new OnScreenKeyboardSectionViewModel();
            mainSection.Rows.Add(BuildRow1(keyboardLayout));
            mainSection.Rows.Add(BuildRow2(keyboardLayout));
            mainSection.Rows.Add(BuildRow3(keyboardLayout));
            mainSection.Rows.Add(BuildRow4(keyboardLayout));
            mainSection.Rows.Add(BuildRow5(keyboardLayout));
            Sections.Add(mainSection);
        }

        private OnScreenKeyboardRowViewModel BuildRow1(KeyboardLayout layout)
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x29, layout));
            row.Keys.Add(KeyForScanCode(0x02, layout));
            row.Keys.Add(KeyForScanCode(0x03, layout));
            row.Keys.Add(KeyForScanCode(0x04, layout));
            row.Keys.Add(KeyForScanCode(0x05, layout));
            row.Keys.Add(KeyForScanCode(0x06, layout));
            row.Keys.Add(KeyForScanCode(0x07, layout));
            row.Keys.Add(KeyForScanCode(0x08, layout));
            row.Keys.Add(KeyForScanCode(0x09, layout));
            row.Keys.Add(KeyForScanCode(0x0A, layout));
            row.Keys.Add(KeyForScanCode(0x0B, layout));
            row.Keys.Add(KeyForScanCode(0x0C, layout));
            row.Keys.Add(KeyForScanCode(0x0D, layout));
            row.Keys.Add(KeyForScanCode(0x0E, layout, 20));
            return row;
        }

        private OnScreenKeyboardRowViewModel BuildRow2(KeyboardLayout layout)
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x0F, layout, 15));
            row.Keys.Add(KeyForScanCode(0x10, layout));
            row.Keys.Add(KeyForScanCode(0x11, layout));
            row.Keys.Add(KeyForScanCode(0x12, layout));
            row.Keys.Add(KeyForScanCode(0x13, layout));
            row.Keys.Add(KeyForScanCode(0x14, layout));
            row.Keys.Add(KeyForScanCode(0x15, layout));
            row.Keys.Add(KeyForScanCode(0x16, layout));
            row.Keys.Add(KeyForScanCode(0x17, layout));
            row.Keys.Add(KeyForScanCode(0x18, layout));
            row.Keys.Add(KeyForScanCode(0x19, layout));
            row.Keys.Add(KeyForScanCode(0x1A, layout));
            row.Keys.Add(KeyForScanCode(0x1B, layout));
            row.Keys.Add(KeyForScanCode(0x2B, layout, 15));
            return row;
        }

        private OnScreenKeyboardRowViewModel BuildRow3(KeyboardLayout layout)
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x3A, layout, 17));
            row.Keys.Add(KeyForScanCode(0x1E, layout));
            row.Keys.Add(KeyForScanCode(0x1F, layout));
            row.Keys.Add(KeyForScanCode(0x20, layout));
            row.Keys.Add(KeyForScanCode(0x21, layout));
            row.Keys.Add(KeyForScanCode(0x22, layout));
            row.Keys.Add(KeyForScanCode(0x23, layout));
            row.Keys.Add(KeyForScanCode(0x24, layout));
            row.Keys.Add(KeyForScanCode(0x25, layout));
            row.Keys.Add(KeyForScanCode(0x26, layout));
            row.Keys.Add(KeyForScanCode(0x27, layout));
            row.Keys.Add(KeyForScanCode(0x28, layout));
            row.Keys.Add(KeyForScanCode(0x1C, layout, 21));
            return row;
        }

        private OnScreenKeyboardRowViewModel BuildRow4(KeyboardLayout layout)
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x2A, layout, 21));
            row.Keys.Add(KeyForScanCode(0x2C, layout));
            row.Keys.Add(KeyForScanCode(0x2D, layout));
            row.Keys.Add(KeyForScanCode(0x2E, layout));
            row.Keys.Add(KeyForScanCode(0x2F, layout));
            row.Keys.Add(KeyForScanCode(0x30, layout));
            row.Keys.Add(KeyForScanCode(0x31, layout));
            row.Keys.Add(KeyForScanCode(0x32, layout));
            row.Keys.Add(KeyForScanCode(0x33, layout));
            row.Keys.Add(KeyForScanCode(0x34, layout));
            row.Keys.Add(KeyForScanCode(0x35, layout));
            row.Keys.Add(KeyForScanCode(0x36, layout, 25));
            return row;
        }

        private OnScreenKeyboardRowViewModel BuildRow5(KeyboardLayout layout)
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x1D, layout));
            row.Keys.Add(new EmptyKeyViewModel());
            row.Keys.Add(KeyForScanCode(0x38, layout));
            row.Keys.Add(KeyForScanCode(0x39, layout));
            row.Keys.Add(new EmptyKeyViewModel());
            row.Keys.Add(new EmptyKeyViewModel());
            row.Keys.Add(new EmptyKeyViewModel());
            row.Keys.Add(new EmptyKeyViewModel());
            row.Keys.Add(new EmptyKeyViewModel());
            row.Keys.Add(new EmptyKeyViewModel());
            row.Keys.Add(new EmptyKeyViewModel());
            row.Keys.Add(new EmptyKeyViewModel());
            return row;
        }

        private BaseOnScreenKeyViewModel KeyForScanCode(int scanCode, KeyboardLayout layout, int widthWeight = 10)
        {
            var t = layout.ScanCodes.Where(x => x.VirtualKey == 91).FirstOrDefault();

            var sc = layout.ScanCodes.SingleOrDefault(x => x.Code == scanCode);

            if (sc == null)
                throw new Exception(string.Format("The scan code {0:X} isn't valid.", scanCode));

            var virtualKeyInfo = layout.VirtualKeys.SingleOrDefault(x => x.Key == sc.VirtualKey);

            if (virtualKeyInfo == null)
            {
                // there is no info about this key. it is simply a virtual key (no characters, etc).
                return new Models.VirtualKey((VirtualKeyCode)sc.VirtualKey)
                {
                    ButtonWidth = new GridLength(widthWeight, GridUnitType.Star)
                };
            }

            return new ShiftSensitiveKey((VirtualKeyCode)sc.VirtualKey, virtualKeyInfo.Characters.First(),
                virtualKeyInfo.Characters.Skip(1).First())
            {
                ButtonWidth = new GridLength(widthWeight, GridUnitType.Star)
            };
        }
    }
}
