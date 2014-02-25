using System;
using System.Collections.Generic;
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
            //var sb = new StringBuilder();

            //sb.AppendLine("---------");
            //sb.AppendLine("CharModifiers");
            //sb.AppendLine("---------");
            //foreach (var charModifier in keyboardLayout.CharModifiers)
            //{
            //    sb.AppendLine(string.Format("ModifierBits:{0}:VirtualKey:{1}", charModifier.ModifierBits,
            //        charModifier.VirtualKey));
            //}

            //sb.AppendLine("---------");
            //sb.AppendLine("ScanCodeText");
            //sb.AppendLine("---------");
            //foreach (var scanCodeText in keyboardLayout.CodeText)
            //{
            //    sb.AppendLine(string.Format("ScanCode:{0:X}:Text:{1}", scanCodeText.ScanCode,
            //        scanCodeText.Text));
            //}

            //sb.AppendLine("---------");
            //sb.AppendLine("ScanCodes");
            //sb.AppendLine("---------");
            //foreach (var scanCode in keyboardLayout.ScanCodes.Where(x => !x.E0Set && !x.E1Set))
            //{
            //    sb.AppendLine(string.Format("ScanCode:{0:X}:VirtualKey:{1}:E0:{2}:E1:{3}", scanCode.Code,
            //        scanCode.VirtualKey, scanCode.E0Set, scanCode.E1Set));
            //}

            //sb.AppendLine("---------");
            //sb.AppendLine("ScanCodes E0");
            //sb.AppendLine("---------");
            //foreach (var scanCode in keyboardLayout.ScanCodes.Where(x => x.E0Set))
            //{
            //    sb.AppendLine(string.Format("ScanCode:{0:X}:VirtualKey:{1}:E0:{2}:E1:{3}", scanCode.Code,
            //        scanCode.VirtualKey, scanCode.E0Set, scanCode.E1Set));
            //}

            //sb.AppendLine("---------");
            //sb.AppendLine("ScanCodes E1");
            //sb.AppendLine("---------");
            //foreach (var scanCode in keyboardLayout.ScanCodes.Where(x => x.E1Set))
            //{
            //    sb.AppendLine(string.Format("ScanCode:{0:X}:VirtualKey:{1}:E0:{2}:E1:{3}", scanCode.Code,
            //        scanCode.VirtualKey, scanCode.E0Set, scanCode.E1Set));
            //}

            //sb.AppendLine("---------");
            //sb.AppendLine("VirtualKeys");
            //sb.AppendLine("---------");
            //foreach (var virtualkey in keyboardLayout.VirtualKeys)
            //{
            //    sb.AppendLine(string.Format("VirtualKey:{0}:Attributes:{1}:Characters:{2}", virtualkey.Key,
            //        virtualkey.Attributes, string.Join(" - ", virtualkey.Characters.Select(x => string.Format("{0:X}", x)))));
            //}

            //var result = sb.ToString();

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
            row.Keys.Add(KeyForScanCode(0x29, layout));             // ~
            row.Keys.Add(KeyForScanCode(0x02, layout));             // 1
            row.Keys.Add(KeyForScanCode(0x03, layout));             // 2
            row.Keys.Add(KeyForScanCode(0x04, layout));             // 3
            row.Keys.Add(KeyForScanCode(0x05, layout));             // 4
            row.Keys.Add(KeyForScanCode(0x06, layout));             // 5 
            row.Keys.Add(KeyForScanCode(0x07, layout));             // 6
            row.Keys.Add(KeyForScanCode(0x08, layout));             // 7
            row.Keys.Add(KeyForScanCode(0x09, layout));             // 8
            row.Keys.Add(KeyForScanCode(0x0A, layout));             // 9
            row.Keys.Add(KeyForScanCode(0x0B, layout));             // 0
            row.Keys.Add(KeyForScanCode(0x0C, layout));             // -
            row.Keys.Add(KeyForScanCode(0x0D, layout));             // +
            row.Keys.Add(KeyForScanCode(0x0E, layout, 20));         // backspace
            return row;
        }

        private OnScreenKeyboardRowViewModel BuildRow2(KeyboardLayout layout)
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x0F, layout, 15));         // tab
            row.Keys.Add(KeyForScanCode(0x10, layout));             // Q
            row.Keys.Add(KeyForScanCode(0x11, layout));             // W
            row.Keys.Add(KeyForScanCode(0x12, layout));             // E
            row.Keys.Add(KeyForScanCode(0x13, layout));             // R
            row.Keys.Add(KeyForScanCode(0x14, layout));             // T
            row.Keys.Add(KeyForScanCode(0x15, layout));             // Y
            row.Keys.Add(KeyForScanCode(0x16, layout));             // U
            row.Keys.Add(KeyForScanCode(0x17, layout));             // I
            row.Keys.Add(KeyForScanCode(0x18, layout));             // O
            row.Keys.Add(KeyForScanCode(0x19, layout));             // P
            row.Keys.Add(KeyForScanCode(0x1A, layout));             // [
            row.Keys.Add(KeyForScanCode(0x1B, layout));             // ]
            row.Keys.Add(KeyForScanCode(0x2B, layout, 15));         // \
            return row;
        }

        private OnScreenKeyboardRowViewModel BuildRow3(KeyboardLayout layout)
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x3A, layout, 17));         // caps lock
            row.Keys.Add(KeyForScanCode(0x1E, layout));             // A
            row.Keys.Add(KeyForScanCode(0x1F, layout));             // S
            row.Keys.Add(KeyForScanCode(0x20, layout));             // D
            row.Keys.Add(KeyForScanCode(0x21, layout));             // F
            row.Keys.Add(KeyForScanCode(0x22, layout));             // G
            row.Keys.Add(KeyForScanCode(0x23, layout));             // H
            row.Keys.Add(KeyForScanCode(0x24, layout));             // J
            row.Keys.Add(KeyForScanCode(0x25, layout));             // K
            row.Keys.Add(KeyForScanCode(0x26, layout));             // L
            row.Keys.Add(KeyForScanCode(0x27, layout));             // ;
            row.Keys.Add(KeyForScanCode(0x28, layout));             // '
            row.Keys.Add(KeyForScanCode(0x1C, layout, 21));         // enter
            return row;
        }

        private OnScreenKeyboardRowViewModel BuildRow4(KeyboardLayout layout)
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x2A, layout, 21));         // left shift
            row.Keys.Add(KeyForScanCode(0x2C, layout));             // Z
            row.Keys.Add(KeyForScanCode(0x2D, layout));             // X
            row.Keys.Add(KeyForScanCode(0x2E, layout));             // C
            row.Keys.Add(KeyForScanCode(0x2F, layout));             // V
            row.Keys.Add(KeyForScanCode(0x30, layout));             // B
            row.Keys.Add(KeyForScanCode(0x31, layout));             // N
            row.Keys.Add(KeyForScanCode(0x32, layout));             // M
            row.Keys.Add(KeyForScanCode(0x33, layout));             // ,
            row.Keys.Add(KeyForScanCode(0x34, layout));             // .
            row.Keys.Add(KeyForScanCode(0x35, layout));             // /
            row.Keys.Add(KeyForScanCode(0x36, layout, 21));         // right shift
            return row;
        }

        private OnScreenKeyboardRowViewModel BuildRow5(KeyboardLayout layout)
        {
            var row = new OnScreenKeyboardRowViewModel();
            row.Keys.Add(KeyForScanCode(0x1D, layout));             // right control
            row.Keys.Add(KeyForScanCode(0x5B, layout, isE0: true)); // left windows
            row.Keys.Add(KeyForScanCode(0x38, layout));             // left alt
            row.Keys.Add(KeyForScanCode(0x39, layout, 45));         // space bar
            row.Keys.Add(KeyForScanCode(0x38, layout, isE0: true)); // right alt
            row.Keys.Add(KeyForScanCode(0x5C, layout, isE0: true)); // right windows
            row.Keys.Add(KeyForScanCode(0x5D, layout, isE0: true)); // menu
            row.Keys.Add(KeyForScanCode(0x1D, layout, isE0: true)); // right control
            return row;
        }

        private BaseOnScreenKeyViewModel KeyForScanCode(int scanCode, KeyboardLayout layout, int widthWeight = 10, bool isE0 = false, bool isE1 = false)
        {
            var sc = layout.ScanCodes.FirstOrDefault(x => x.Code == scanCode && x.E0Set == isE0 && x.E1Set == isE1);

            if (sc == null)
                throw new Exception(string.Format("The scan code {0:X} isn't valid.", scanCode));

            var virtualKey = (VirtualKeyCode)(sc.VirtualKey & 0xFF);

            var virtualKeyInfo = layout.VirtualKeys.SingleOrDefault(x => x.Key == sc.VirtualKey);

            var scanCodeText = layout.CodeText.SingleOrDefault(x => x.ScanCode == scanCode);

            return new VirtualKey(virtualKey,
                scanCodeText != null ? scanCodeText.Text : null,
                virtualKeyInfo != null ? virtualKeyInfo.Characters.ToList() : new List<int>(),
                virtualKeyInfo != null && virtualKeyInfo.Attributes == 1)
                {
                    ButtonWidth = new GridLength(widthWeight, GridUnitType.Star)
                };
        }
    }
}
