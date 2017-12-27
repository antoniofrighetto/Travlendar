using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Travlendar.Core.AppCore.Pages
{
    public class MapPage : ContentPage
    {
        MapViewModel _viewModel;

        StackLayout stack;
        ToolbarItem save;
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

            map = new Map (
                            MapSpan.FromCenterAndRadius (new Position (0, 0), Distance.FromMiles (0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            save = new ToolbarItem
            {
                Text = "Save"
            };
            save.Clicked += Save_Clicked;

            stack.Children.Add (position);
            stack.Children.Add (map);

            ToolbarItems.Add (save);
            Content = stack;
        }

        private async void Save_Clicked (object sender, System.EventArgs e)
        {
            //Salva sul viewmodel di appointment creation
            //la position _viewModel.Position;
            await Navigation.PopAsync ();
        }

        private void Position_Completed (object sender, System.EventArgs e)
        {
            var entry = ((Entry) sender).Text;
            _viewModel.GetPositionFromString (entry);
        }

        private void _viewModel_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            map.MoveToRegion (MapSpan.FromCenterAndRadius (_viewModel.Position, Distance.FromKilometers (0.5)));
            map.Pins.Add (new Pin
            {
                Type = PinType.SearchResult,
                Position = _viewModel.Position,
                Label = "Position"
            });
        }
    }
}
