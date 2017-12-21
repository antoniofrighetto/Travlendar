
using System.IO;
using Travlendar.Core.AppCore.Tools;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class TicketsPage : ContentPage
    {
        StackLayout layout;
        Button pickPictureButton;
        TicketsViewModel _viewModel;

        public TicketsPage (TicketsViewModel vm)
        {
            _viewModel = vm;
            layout = new StackLayout
            {
                Children = {
                    new Label { Text = "Welcome to Xamarin.Forms!" }
                },
                BackgroundColor = Color.White
            };

            pickPictureButton = new Button
            {
                Text = "Pick Photo",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            pickPictureButton.Clicked += PickPictureButton_ClickedAsync;
            layout.Children.Add (pickPictureButton);
        }

        private async void PickPictureButton_ClickedAsync (object sender, System.EventArgs e)
        {
            pickPictureButton.IsEnabled = false;
            Stream stream = await DependencyService.Get<IPicturePicker> ().GetImageStreamAsync ();

            if ( stream != null )
            {
                Image image = new Image
                {
                    Source = ImageSource.FromStream (() => stream),
                    BackgroundColor = Color.Gray
                };

                //
                //https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/dependency-service/photo-picker/#Creating_the_Interface
                //

                TapGestureRecognizer recognizer = new TapGestureRecognizer ();
                recognizer.Tapped += (sender2, args) =>
                {
                    (sender2 as ContentPage).Content = layout;
                    pickPictureButton.IsEnabled = true;
                };
                image.GestureRecognizers.Add (recognizer);

                //(as ContentPage).Content = image;
            }
            else
            {
                pickPictureButton.IsEnabled = true;
            }
        }
    }
}
