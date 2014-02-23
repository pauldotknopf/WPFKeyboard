using System.Collections.Specialized;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows;
using WindowsInput.Native;
using WPFKeyboard.Models;

namespace Sample.ViewModels
{
    public class SampleKeyboardViewModel : OnScreenKeyboardViewModel
    {
        public SampleKeyboardViewModel()
        {
            //var mainSection = new OnScreenKeyboardSectionViewModel();
            //mainSection.Rows.Add(new OnScreenKeyboardRowViewModel());
            //mainSection.Rows.Add(new OnScreenKeyboardRowViewModel());
            //mainSection.Rows.Add(new OnScreenKeyboardRowViewModel());
            //mainSection.Rows.Add(new OnScreenKeyboardRowViewModel());
            //mainSection.Rows.Add(new OnScreenKeyboardRowViewModel());

            //mainSection.Rows[0].Keys.Add(new VirtualKey(VirtualKeyCode.ESCAPE, "Escape"));
            //mainSection.Rows[0].Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.OEM_3, "`", "~"));
            //mainSection.Rows[0].Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.OEM_3, "1", "!"));
            //mainSection.Rows[0].Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.OEM_3, "2", "@"));
            //mainSection.Rows[0].Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.OEM_3, "3", "#"));
            //mainSection.Rows[0].Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.OEM_3, "4", "$"));
            //mainSection.Rows[0].Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.OEM_3, "5", "%"));
            //mainSection.Rows[0].Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.OEM_3, "6", "^"));
            //mainSection.Rows[0].Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.OEM_3, "7", "&"));
            //mainSection.Rows[0].Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.OEM_3, "8", "*"));
            //mainSection.Rows[0].Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.OEM_3, "9", "("));
            //mainSection.Rows[0].Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.OEM_3, "0", ")"));
            //mainSection.Rows[0].Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.OEM_3, "-", "_"));
            //mainSection.Rows[0].Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.OEM_3, "=", "+"));
            //mainSection.Rows[0].Keys.Add(new VirtualKey(VirtualKeyCode.BACK, "Backspace"){ButtonWidth = new GridLength(2, GridUnitType.Star)});
            ////new OnScreenKey { GridRow = 0, GridColumn = 0, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_3, new List<string> { "`", "~" })},
            ////                       new OnScreenKey { GridRow = 0, GridColumn = 1, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_1, new List<string> { "1", "!" })},
            ////                       new OnScreenKey { GridRow = 0, GridColumn = 2, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_2, new List<string> { "2", "@" })},
            ////                       new OnScreenKey { GridRow = 0, GridColumn = 3, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_3, new List<string> { "3", "#" })},
            ////                       new OnScreenKey { GridRow = 0, GridColumn = 4, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_4, new List<string> { "4", "$" })},
            ////                       new OnScreenKey { GridRow = 0, GridColumn = 5, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_5, new List<string> { "5", "%" })},
            ////                       new OnScreenKey { GridRow = 0, GridColumn = 6, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_6, new List<string> { "6", "^" })},
            ////                       new OnScreenKey { GridRow = 0, GridColumn = 7, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_7, new List<string> { "7", "&" })},
            ////                       new OnScreenKey { GridRow = 0, GridColumn = 8, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_8, new List<string> { "8", "*" })},
            ////                       new OnScreenKey { GridRow = 0, GridColumn = 9, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_9, new List<string> { "9", "(" })},
            ////                       new OnScreenKey { GridRow = 0, GridColumn = 10, Key =  new ShiftSensitiveKey(VirtualKeyCode.VK_0, new List<string> { "0", ")" })},
            ////                       new OnScreenKey { GridRow = 0, GridColumn = 11, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_MINUS, new List<string> { "-", "_" })},
            ////                       new OnScreenKey { GridRow = 0, GridColumn = 12, Key =  new ShiftSensitiveKey(VirtualKeyCode.OEM_PLUS, new List<string> { "=", "+" })},
            ////                       new OnScreenKey { GridRow = 0, GridColumn = 13, Key =  new VirtualKey(VirtualKeyCode.BACK, "Bksp"), GridWidth = new GridLength(2, GridUnitType.Star)},


            //Sections.Add(mainSection);

            //AddKeys(testSection1, 15);

            //var testSection2 = new OnScreenKeyboardSectionViewModel
            //{
            //    SectionWidth = new GridLength(1, GridUnitType.Star)
            //};
            //testSection2.Rows.Add(new OnScreenKeyboardRowViewModel());
            //testSection2.Rows.Add(new OnScreenKeyboardRowViewModel());
            //testSection2.Rows.Add(new OnScreenKeyboardRowViewModel());

            //Sections.Add(testSection1);
            //Sections.Add(testSection2);

            //AddKeys(testSection2, 3);
        }

        private void AddKeys(OnScreenKeyboardSectionViewModel section, int keysPerRow)
        {
            foreach (var row in section.Rows)
            {
                for (var x = 1; x <= keysPerRow; x++)
                {
                    //row.Keys.Add(new ModifierKey(VirtualKeyCode.SHIFT, VirtualKeyCode.LSHIFT, VirtualKeyCode.RSHIFT));
                    //row.Keys.Add(new ShiftSensitiveKey(VirtualKeyCode.VK_A, "a", "A"));
                }
                row.Keys[1].ButtonWidth = new GridLength(5, GridUnitType.Star);
            }
        }
    }
}
