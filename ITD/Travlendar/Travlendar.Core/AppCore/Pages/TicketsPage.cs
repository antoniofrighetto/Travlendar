using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Travlendar.Core.AppCore.Helpers;
using Travlendar.Core.AppCore.Model;
using Travlendar.Core.AppCore.Renderers;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class TicketsPage : ContentPage
    {
        public TicketsViewModel _viewModel;
        private INavigation navigation;
        ListView listView;
        ObservableCollection<TicketModel> tickets { get; set; }

        //Building the Tickets page
        public TicketsPage (INavigation navigation, TicketsViewModel vm)
        {
            _viewModel = vm;
            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
            this.navigation = navigation;

            Title = "Tickets";
            BackgroundColor = Color.White;

            DependencyService.Get<IStatusBar> ().ShowStatusBar ();

            //Listview displaying the available tickets
            listView = new ListView ();
            listView.ItemTemplate = new DataTemplate (typeof (CustomImageCell));
            listView.ItemTemplate.SetBinding (ImageCell.TextProperty, "Name");
            listView.ItemTemplate.SetBinding (ImageCell.DetailProperty, "Detail");
            listView.ItemTemplate.SetBinding (ImageCell.ImageSourceProperty, "Image");
            listView.ItemTapped += ListView_ItemTapped;
            listView.ItemSelected += (sender, e) =>
            {
                ((ListView) sender).SelectedItem = null;
            };

            //Controller for adding tickets
            ToolbarItem addTicket = new ToolbarItem
            {
                Text = "Add Ticket",
                Icon = Device.RuntimePlatform == Device.Android ? "add_ic" : ""
            };
            addTicket.Clicked += AddTicket_ClickedAsync;

            ToolbarItems.Add (addTicket);
            Content = listView;
            RefreshList (null);
        }

        //Preview a ticket when tapped
        private async void ListView_ItemTapped (object sender, ItemTappedEventArgs e)
        {
            var page = new FullscreenImagePage ((e.Item as TicketModel).Image);
            if ( Device.RuntimePlatform == Device.Android )
                NavigationPage.SetHasNavigationBar (page, false);
            await navigation.PushAsync (page);
        }

        //Allows refreshing the list when a ticket is added or removed
        private void _viewModel_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RefreshList (e);
        }

        private void RefreshList (System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ( tickets == null )
            {
                tickets = new ObservableCollection<TicketModel> ();
                foreach ( KeyValuePair<string, string> element in _viewModel.Tickets )
                {
                    tickets.Add (new TicketModel { Name = "Ticket", Detail = element.Key, Image = element.Value });
                }
            }
            else if ( e != null )
            {
                var item = tickets.FirstOrDefault (element => element.Detail == e.PropertyName);
                if ( item != null )
                    tickets.Remove (item);
            }
            else
            {
                var item = _viewModel.Tickets.LastOrDefault ();
                var newTicket = new TicketModel { Name = "Ticket", Detail = item.Key, Image = item.Value };
                if ( !object.Equals (item, default (KeyValuePair<string, string>)) && !tickets.Contains (newTicket) )
                    tickets.Add (newTicket);
            }

            listView.ItemsSource = tickets;
        }

        private async void AddTicket_ClickedAsync (object sender, System.EventArgs e)
        {
            var page = new AddTicketPage (TicketsViewModel.GetInstance ());
            await navigation.PushAsync (page);
        }
    }
}
