using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class CalendarPage : ContentPage
    {
        CalendarViewModel _viewModel;
        private ToolbarItem addAppointment;
        private ToolbarItem tickets;
        private ToolbarItem settings;
        private ToolbarItem logout;

        public CalendarPage (CalendarViewModel vm)
        {
            _viewModel = vm;

            FillMenu ();

            Button map = new Button
            {
                Text = "Map -> WIP Toolbar",
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
            var page = new MapPage (new MapViewModel (this.Navigation));
            //NavigationPage.SetHasNavigationBar (page, false);
            await Navigation.PushAsync (page);
        }

        private void FillMenu ()
        {
            addAppointment = new ToolbarItem
            {
                Text = "Add",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };

            tickets = new ToolbarItem
            {
                Text = "Tickets",
                Order = ToolbarItemOrder.Secondary,
                Priority = 0
            };

            settings = new ToolbarItem
            {
                Text = "Settings",
                Order = ToolbarItemOrder.Secondary,
                Priority = 0
            };

            logout = new ToolbarItem
            {
                Text = "Logout",
                Order = ToolbarItemOrder.Secondary,
                Priority = 0
            };

            RegisterEvents ();

            ToolbarItems.Add (addAppointment);
            ToolbarItems.Add (tickets);
            ToolbarItems.Add (settings);
            ToolbarItems.Add (logout);
        }

        private void RegisterEvents ()
        {
            tickets.Clicked -= Tickets_ClickedAsync;
            tickets.Clicked += Tickets_ClickedAsync;

            settings.Clicked -= Settings_Clicked;
            settings.Clicked += Settings_Clicked;

            logout.Clicked -= Logout_Clicked;
            logout.Clicked += Logout_Clicked;
        }

        private void Logout_Clicked (object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException ();
        }

        private async void Settings_Clicked (object sender, System.EventArgs e)
        {
            var page = new SettingsPage(this.Navigation);
            await Navigation.PushAsync(page);
        }

        private async void Tickets_ClickedAsync (object sender, System.EventArgs e)
        {
            var page = new TicketsPage (new TicketsViewModel (this.Navigation));
            await Navigation.PushAsync (page);
        }
    }
}
