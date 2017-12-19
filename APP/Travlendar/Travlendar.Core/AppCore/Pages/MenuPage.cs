
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class MenuPage : ContentPage
    {
        public MenuPage ()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "That's the menu" }
                }
            };
        }
    }
}
