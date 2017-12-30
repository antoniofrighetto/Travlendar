using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ListView listView;
        ObservableCollection<TicketModel> tickets { get; set; }

        public TicketsPage (TicketsViewModel vm)
        {
            _viewModel = vm;
            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
            Title = "Tickets";
            BackgroundColor = Color.White;

            DependencyService.Get<IStatusBar> ().ShowStatusBar ();

            tickets = new ObservableCollection<TicketModel> ();
            listView = new ListView ();
            listView.ItemsSource = tickets;

            listView.ItemTemplate = new DataTemplate (typeof (CustomImageCell));
            listView.ItemTemplate.SetBinding (ImageCell.TextProperty, "Name");
            listView.ItemTemplate.SetBinding (ImageCell.DetailProperty, "Detail");
            listView.ItemTemplate.SetBinding (ImageCell.ImageSourceProperty, "Image");
            listView.ItemTapped += ListView_ItemTapped;
            listView.ItemSelected += (sender, e) =>
            {
                ((ListView) sender).SelectedItem = null;
            };

            ToolbarItem addTicket = new ToolbarItem
            {
                Text = "Add Ticket",
                Icon = Device.RuntimePlatform == Device.Android ? "add_ic" : ""
            };
            addTicket.Clicked += AddTicket_ClickedAsync;

            ToolbarItems.Add (addTicket);
            Content = listView;
            RefreshList ();
        }

        private void ListView_ItemTapped (object sender, ItemTappedEventArgs e)
        {
            var page = new FullscreenImagePage (e.Item.ToString ());
            NavigationPage.SetHasNavigationBar (page, false);
            Application.Current.MainPage.Navigation.PushAsync (page);
        }

        private void _viewModel_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RefreshList ();
        }

        private void RefreshList ()
        {
            tickets = new ObservableCollection<TicketModel> ();
            foreach ( KeyValuePair<string, string> element in _viewModel.Tickets )
            {
                tickets.Add (new TicketModel () { Name = "Ticket", Detail = element.Value, Image = element.Key });
            }
            listView.ItemsSource = tickets;
        }

        private async void AddTicket_ClickedAsync (object sender, System.EventArgs e)
        {
            var page = new AddTicketPage (TicketsViewModel.GetInstance ());
            await Navigation.PushAsync (page);
        }
    }
}
