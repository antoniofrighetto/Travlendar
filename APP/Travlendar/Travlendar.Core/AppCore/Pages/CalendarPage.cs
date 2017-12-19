
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class CalendarPage : ContentPage
    {
        CalendarViewModel _viewModel;

        public CalendarPage (CalendarViewModel vm)
        {
            _viewModel = vm;

            Button map = new Button
            {
                Text = "Map",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            map.Clicked += Map_ClickedAsync;
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "CalendarPage!" },
                    new Label { Text = "Qui mettiamo il componente di telerik!" },
                    map
                }
            };
        }

        private async void Map_ClickedAsync (object sender, System.EventArgs e)
        {
            var page = new MapPage ();
            //NavigationPage.SetHasNavigationBar (page, false);
            await Navigation.PushAsync (page);
        }
    }
}
