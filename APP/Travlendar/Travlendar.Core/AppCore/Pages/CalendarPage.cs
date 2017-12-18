
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class CalendarPage : ContentPage
    {
        public CalendarPage ()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "CalendarPage!" }
                }
            };
        }
    }
}
