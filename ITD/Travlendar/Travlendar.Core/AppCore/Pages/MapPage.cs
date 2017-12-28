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
        SearchBar searchBar;

        public MapPage ()
        {
            BindingContext = new MapViewModel(this.Navigation, this);
            _viewModel = ((MapViewModel)BindingContext);

            stack = new StackLayout { Spacing = 0 };

            searchBar = new SearchBar
            {
                Placeholder = "Enter destination",
                SearchCommand = new Command(async () => await _viewModel.GetPositionFromString(searchBar.Text))
            };

            map = new Map ()
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

            save.SetBinding(MenuItem.CommandProperty, new Binding("SaveLocationCommand"));

            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
            _viewModel.CurrentPositionEvent += (sender, e) =>
            {
                map.MoveToRegion (MapSpan.FromCenterAndRadius (_viewModel.CurrentPosition, Distance.FromMiles (0.5)));
            };

            Device.BeginInvokeOnMainThread(async () => await _viewModel.GetCurrentLocation());

            stack.Children.Add (searchBar);
            stack.Children.Add (map);
            ToolbarItems.Add (save);
            Content = stack;
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
