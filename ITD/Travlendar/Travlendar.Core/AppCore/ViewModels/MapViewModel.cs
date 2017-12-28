using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Travlendar.Core.Framework.Dependencies;
using Travlendar.Framework.ViewModels;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

using Travlendar.Core.AppCore.Pages;

namespace Travlendar.Core.AppCore.ViewModels
{
    public class MapViewModel : BindableBaseNotify
    {

        Geocoder geoCoder;
        private INavigation navigation;
        private MapPage page;
        public event EventHandler CurrentPositionEvent;

        private string _positionName;
        public string PositionName { get => _positionName; set => _positionName = value; }

        private Position _currentPosition;
        public Position CurrentPosition
        {
            get { return _currentPosition; }
            set { 
                this.SetProperty(ref _currentPosition, value); 
                CurrentPositionEvent?.Invoke(this, null); 
            }
        }

        private Position _position;
        public Position Position
        {
            get { return _position; }
            set { this.SetProperty(ref _position, value); }
        }

        private Command saveLocationCommand;
        public ICommand SaveLocationCommand 
        {
            get
            {
                return saveLocationCommand ?? (saveLocationCommand = new Command(async () =>
                {
                    if (PositionName == null) {
                        await page.DisplayAlert("Select a valid location before", "", "Ok");
                        return;
                    }

                    MessagingCenter.Send<MapPage, string>(this.page, "LocationNameEvent", PositionName);
                    await navigation.PopAsync();
                }));
            }
        }

        public MapViewModel (INavigation navigation, MapPage page)
        {
            this.navigation = navigation;
            this.page = page;
            geoCoder = new Geocoder ();
        }

        public async Task GetCurrentLocation ()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                var loc = await locator.GetLastKnownLocationAsync ();

                if ( loc != null )
                {
                    CurrentPosition = new Position(loc.Latitude, loc.Longitude);
                    return;
                }

                if ( !locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled )
                {
                    DependencyService.Get<IMessage>().ShortAlert("Location not available");    
                    return;
                }

                var def = await locator.GetPositionAsync (TimeSpan.FromSeconds (10), null, true);
                CurrentPosition = new Position (def.Latitude, def.Longitude);
            }
            catch ( Exception ex )
            {
                Debug.WriteLine ("TRAVLENDAR || GetCurrentLocation error: " + ex);
            }
        }

        public async Task GetPositionFromString (string textPosition)
        {
            try
            {
                var positions = await geoCoder.GetPositionsForAddressAsync(textPosition);
                if (positions.Any()) {
                    Position = positions.ToList()[0];
                    PositionName = (await geoCoder.GetAddressesForPositionAsync(Position)).ToList()[0];
                }
            }
            catch ( Exception e )
            {
                Debug.WriteLine ("TRAVLENDAR || GetPositionFromString error: " + e);
                DependencyService.Get<IMessage> ().ShortAlert (textPosition + "not found");
            }
        }
    }
}
