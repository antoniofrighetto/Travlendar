using System;

using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public partial class CalendarTypePage : ContentPage
    {
        private static bool flag = true;
        private static string checkedViewCell = null;
        public CalendarTypePage()
        {
            InitializeComponent();
            if (flag || checkedViewCell == null)
            {
                Work.StyleId = "checkmark";
                flag = false;
            }
            else
            {
                switch (checkedViewCell)
                {
                    case "Work":
                        Work.StyleId = "checkmark"; break;
                    case "Home":
                        Home.StyleId = "checkmark"; break;
                    case "Birthday":
                        Birthday.StyleId = "checkmark"; break;
                    case "Festivity":
                        Festivity.StyleId = "checkmark"; break;
                }
            }
        }

        void ViewCellTapped(object sender, EventArgs e)
        {
            Color color = Color.Default;
            ViewCell cell = (ViewCell)sender;
            checkedViewCell = cell.ClassId;
            cell.StyleId = "checkmark";
            switch (cell.ClassId)
            {
                case "Work":
                    color = Color.FromRgb(28, 109, 107); break;
                case "Home":
                    color = Color.FromRgb(24, 164, 247); break;
                case "Birthday":
                    color = Color.FromRgb(24, 89, 127); break;
                case "Festivity":
                    color = Color.FromRgb(3, 109, 81); break;
            }
            MessagingCenter.Send(this, "ColorEvent", color);
        }
    }
}
