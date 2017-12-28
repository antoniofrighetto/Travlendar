using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Travlendar.Core.AppCore.Model;
using Travlendar.Core.Framework.Dependencies;
using Travlendar.Framework.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Travlendar.Core.AppCore.ViewModels
{
    public class MapViewModel : AViewModel<MapModel>
    {

        Geocoder geoCoder;
        public override event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CurrentPositionEvent;

        private string _positionName;
        public string PositionName { get => _positionName; set => _positionName = value; }

        private Position _currentPosition;
        public Position CurrentPosition
        {
            get
            {
                return _currentPosition;
            }
            set
            {
                _currentPosition = value;
                CurrentPositionEvent?.Invoke (this, null);
            }
        }

        private Position _position;
        public Position Position
        {
            get
            {
                return _position;
            }
            set
            {
                if ( _position != value )
                {
                    _position = value;
                    RaisePropertyChanged ();
                }
            }
        }

        public MapViewModel (INavigation navigation)
        {
            this.Navigation = navigation;
            geoCoder = new Geocoder ();
        }

        public async void GetCurrentLocation ()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                var loc = await locator.GetLastKnownLocationAsync ();
                CurrentPosition = new Position (loc.Latitude, loc.Longitude);

                if ( CurrentPosition != null )
                {
                    return;
                }

                if ( !locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled )
                {
                    DependencyService.Get<IMessage> ().ShortAlert ("Location not available");
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

        public async void GetPositionFromString (string textPosition)
        {
            try
            {
                Position = await GetPositionsForAddressSyncAsync (textPosition);
                PositionName = textPosition;
            }
            catch ( Exception e )
            {
                Debug.WriteLine ("TRAVLENDAR || GetPositionFromString error: " + e);
                DependencyService.Get<IMessage> ().ShortAlert (textPosition + "not found");
            }
        }

        public async Task<Position> GetPositionsForAddressSyncAsync (string textPosition)
        {
            List<Position> positions = new List<Position> ();
            var approximateLocations = await geoCoder.GetPositionsForAddressAsync (textPosition);

            return approximateLocations.FirstOrDefault ();
        }

        private void RaisePropertyChanged ([CallerMemberName] string property = null)
        {
            var propChanged = PropertyChanged;
            if ( propChanged != null )
            {
                propChanged (this, new PropertyChangedEventArgs (property));
            }
        }
    }
}
