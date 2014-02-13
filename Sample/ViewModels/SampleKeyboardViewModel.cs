using System.Collections.Specialized;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows;
using WPFKeyboard.Models;

namespace Sample.ViewModels
{
    public class SampleKeyboardViewModel : OnScreenKeyboardViewModel
    {
        public SampleKeyboardViewModel()
        {
            var testSection1 = new OnScreenKeyboardSectionViewModel
            {
                SectionWidth = new GridLength(3, GridUnitType.Star)
            };
            testSection1.Rows.Add(new OnScreenKeyboardRowViewModel());
            testSection1.Rows.Add(new OnScreenKeyboardRowViewModel());
            testSection1.Rows.Add(new OnScreenKeyboardRowViewModel());

            AddKeys(testSection1, 15);

            var testSection2 = new OnScreenKeyboardSectionViewModel
            {
                SectionWidth = new GridLength(1, GridUnitType.Star)
            };
            testSection2.Rows.Add(new OnScreenKeyboardRowViewModel());
            testSection2.Rows.Add(new OnScreenKeyboardRowViewModel());
            testSection2.Rows.Add(new OnScreenKeyboardRowViewModel());

            Sections.Add(testSection1);
            Sections.Add(testSection2);

            AddKeys(testSection2, 3);
        }

        private void AddKeys(OnScreenKeyboardSectionViewModel section, int keysPerRow)
        {
            foreach (var row in section.Rows)
            {
                for (var x = 1; x <= keysPerRow; x++)
                {
                    row.Keys.Add(new TempKeyBase());
                }
                row.Keys[1].ButtonWidth = new GridLength(5, GridUnitType.Star);
            }
        }
    }
}
