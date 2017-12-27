
using System;
using System.IO;
using Travlendar.Core.AppCore.Helpers;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class AddTicketPage : ContentPage
    {
        private TicketsViewModel _viewModel;

        private StackLayout layout;
        private ToolbarItem save;
        private Button pickPictureButton;
        private Image ticketChosen;
        private ImageSource savedTicket;
        private Entry ticketName;
        private string entry;

        public AddTicketPage (TicketsViewModel vm)
        {
            _viewModel = vm;
            save = new ToolbarItem
            {
                Text = "Save"
            };
            save.Clicked += Save_ClickedAsync;
            ToolbarItems.Add (save);
            FillLayout ("Ticket Name");
        }

        private async void Save_ClickedAsync (object sender, EventArgs e)
        {
            if ( savedTicket == null )
            {
                await DisplayAlert ("Hey", "It looks like there is no image selected", "Pick one!");
                ticketName.IsEnabled = true;
                return;
            }

            _viewModel.Tickets.Add (entry, savedTicket);
            await Navigation.PushAsync (new TicketsPage (TicketsViewModel.GetInstance ()));
        }

        private void FillLayout (string entryName, Image tempTicket = null)
        {
            layout = new StackLayout
            {
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            ticketName = new Entry
            {
                Placeholder = entryName,
                BackgroundColor = Color.LightGray,
                HorizontalTextAlignment = TextAlignment.Center
            };
            ticketName.Completed += TicketName_Completed;
            ticketName.TextChanged += TicketName_TextChanged;

            ticketChosen = tempTicket != null ? tempTicket : new Image { };

            pickPictureButton = new Button
            {
                Text = "Add Ticket",
                VerticalOptions = LayoutOptions.EndAndExpand
            };
            pickPictureButton.Clicked += PickPictureButton_ClickedAsync;
            pickPictureButton.IsEnabled = false;

            layout.Children.Add (ticketName);
            layout.Children.Add (ticketChosen);
            layout.Children.Add (pickPictureButton);
            this.Content = layout;
        }

        private void TicketName_TextChanged (object sender, TextChangedEventArgs e)
        {
            if ( string.IsNullOrWhiteSpace (e.NewTextValue) )
                pickPictureButton.IsEnabled = false;
            else
            {
                pickPictureButton.IsEnabled = true;
                entry = e.NewTextValue;
            }

        }

        private void TicketName_Completed (object sender, EventArgs e)
        {
            entry = ((Entry) sender).Text;
        }

        private async void PickPictureButton_ClickedAsync (object sender, EventArgs e)
        {
            Stream stream = await DependencyService.Get<IPicturePicker> ().GetImageStreamAsync ();

            if ( stream != null )
            {
                Image image = new Image
                {
                    Source = ImageSource.FromStream (() => stream),
                    BackgroundColor = Color.Gray
                };

                TapGestureRecognizer recognizer = new TapGestureRecognizer ();
                recognizer.Tapped += (sender2, args) =>
                {
                    this.Content = layout;
                    pickPictureButton.IsEnabled = true;
                };
                image.GestureRecognizers.Add (recognizer);
                savedTicket = image.Source;
                FillLayout (entry, image);
                ticketName.IsEnabled = false;
            }
            else
            {
                pickPictureButton.IsEnabled = true;
            }
        }
    }
}
