using System.Collections.Generic;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class TicketsPage : ContentPage
    {
        ToolbarItem addTicket;
        TicketsViewModel _viewModel;
        TableView tableView;
        TableSection tableSection;

        StackLayout testLayout;

        public TicketsPage (TicketsViewModel vm)
        {
            _viewModel = vm;

            testLayout = new StackLayout { };

            foreach ( KeyValuePair<string, ImageSource> element in _viewModel.Tickets )
            {
                var y = new Label
                {
                    Text = element.Key
                };
                var x = new Image
                {
                    IsVisible = true,
                    Source = element.Value,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    BackgroundColor = Color.Gray
                };
                testLayout.Children.Add (x);
                testLayout.Children.Add (y);
            }

            addTicket = new ToolbarItem
            {
                Text = "Add Ticket"
            };
            addTicket.Clicked += AddTicket_ClickedAsync;
            ToolbarItems.Add (addTicket);

            this.Content = testLayout;
        }

        private async void AddTicket_ClickedAsync (object sender, System.EventArgs e)
        {
            var page = new AddTicketPage (TicketsViewModel.GetInstance ());
            await Navigation.PushAsync (page);
        }
    }
}
