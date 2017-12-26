using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public partial class CalendarTypePage : ContentPage
    {
        public CalendarTypePage()
        {
            InitializeComponent();
        }

        void ViewCellTapped(object sender, EventArgs e)
        {
            Color color = Color.Default;
            ViewCell cell = (ViewCell)sender;
            cell.StyleId = "checkmark";
            switch (cell.ClassId)
            {
                case "Home":
                    color = Color.Red; break;
                case "Work":
                    color = Color.Yellow; break;
                case "Birthday":
                    color = Color.Green; break;
                case "Festivity":
                    color = Color.Blue; break;
            }
            MessagingCenter.Send(this, "ColorEvent", color);
        }
    }
}
