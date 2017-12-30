using Plugin.Media.Abstractions;
using System;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class AddTicketPage : ContentPage
    {
        private TicketsViewModel _viewModel;

        private StackLayout layout;
        private Button pickPictureButton;
        private Button takePictureButton;
        private Image ticketChosen;
        private Image savedTicket;
        private Entry ticketName;
        private string entry;
        private string filePath;

        public AddTicketPage (TicketsViewModel vm)
        {
            _viewModel = vm;
            BackgroundColor = Color.White;

            ToolbarItem save = new ToolbarItem
            {
                Text = "Save"
            };
            save.Clicked += Save_ClickedAsync;
            ToolbarItems.Add (save);
            FillLayout ("Ticket Name");
        }

        private async void Save_ClickedAsync (object sender, EventArgs e)
        {
            if ( filePath == null )
            {
                await DisplayAlert ("Hey", "It looks like there is no image selected", "Pick one!");
                ticketName.IsEnabled = true;
                return;
            }

            _viewModel.SaveTicket (filePath, entry);
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

            ticketChosen = tempTicket != null ? tempTicket : new Image { VerticalOptions = LayoutOptions.FillAndExpand };

            StackLayout buttons = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Center };
            pickPictureButton = new Button
            {
                Text = "Add Ticket",
                VerticalOptions = LayoutOptions.End
            };
            pickPictureButton.Clicked += PickPictureButton_ClickedAsync;
            pickPictureButton.IsEnabled = false;

            takePictureButton = new Button
            {
                Text = "Take Picture",
                VerticalOptions = LayoutOptions.End
            };
            takePictureButton.Clicked += TakePictureButton_ClickedAsync;
            takePictureButton.IsEnabled = false;

            buttons.Children.Add (pickPictureButton);
            buttons.Children.Add (takePictureButton);

            layout.Children.Add (ticketName);
            layout.Children.Add (ticketChosen);
            layout.Children.Add (buttons);

            this.Content = layout;
        }

        private void TicketName_TextChanged (object sender, TextChangedEventArgs e)
        {
            if ( string.IsNullOrWhiteSpace (e.NewTextValue) )
            {
                pickPictureButton.IsEnabled = false;
                takePictureButton.IsEnabled = false;
            }
            else
            {
                pickPictureButton.IsEnabled = true;
                takePictureButton.IsEnabled = true;
                entry = e.NewTextValue;
            }
        }

        private void TicketName_Completed (object sender, EventArgs e)
        {
            entry = ((Entry) sender).Text;
        }

        private async void TakePictureButton_ClickedAsync (object sender, EventArgs e)
        {
            var x = await _viewModel.TakePictureAsync (entry);
            if ( x != null )
            {
                ShowImage (x);
            }
        }

        private async void PickPictureButton_ClickedAsync (object sender, EventArgs e)
        {
            var x = await _viewModel.PickPictureAsync (entry);
            if ( x != null )
            {
                ShowImage (x);
            }
        }

        private void ShowImage (MediaFile x)
        {
            filePath = x.Path;
            savedTicket = new Image { Source = ImageSource.FromFile (filePath) };
            FillLayout (entry, savedTicket);
            ticketName.IsEnabled = false;
        }
    }
}
