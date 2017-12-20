using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Travlendar.Core.AppCore.Pages
{
    public class MapPage : ContentPage
    {
        MapViewModel _viewModel;

        StackLayout stack;
        Map map;
        Entry position;

        public MapPage (MapViewModel vm)
        {
            _viewModel = vm;

            _viewModel.PropertyChanged += _viewModel_PropertyChanged;

            stack = new StackLayout { Spacing = 0 };

            position = new Entry
            {
                Placeholder = "Position",
                PlaceholderColor = Color.LightGray,
                HorizontalOptions = LayoutOptions.Center
            };
            position.Completed += Position_Completed;
            stack.Children.Add (position);
            Content = stack;
        }

        private void Position_Completed (object sender, System.EventArgs e)
        {
            _viewModel.Position = _viewModel.GetPositionFromString (((Entry) sender).Text).Result;
        }

        private void _viewModel_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            map = new Map (
                            MapSpan.FromCenterAndRadius (_viewModel.Position, Distance.FromMiles (0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            stack.Children.Add (map);
        }
    }
}
