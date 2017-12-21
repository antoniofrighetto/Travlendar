using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Travlendar.Core.AppCore.Model;
using Travlendar.Framework.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Travlendar.Core.AppCore.ViewModels
{
    public class MapViewModel : AViewModel<MapModel>
    {

        Geocoder geoCoder;

        private Position _position;

        public override event PropertyChangedEventHandler PropertyChanged;

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
            //TODO for now is a test
            geoCoder = new Geocoder ();
        }

        public async void GetPositionFromString (string textPosition)
        {
            try
            {
                var pos = await GetPositionsForAddressSyncAsync (textPosition);
                Position = pos [0];
            }
            catch ( Exception e )
            {
                // TODO ADD POPUP LOCATION NOT FOUND OR HANDLE ERROR
            }
        }

        public async System.Threading.Tasks.Task<List<Position>> GetPositionsForAddressSyncAsync (string textPosition)
        {
            List<Position> positions = new List<Position> ();

            var approximateLocations = await geoCoder.GetPositionsForAddressAsync (textPosition);
            foreach ( var position in approximateLocations )
            {
                positions.Add (position);
            }

            return positions;
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
