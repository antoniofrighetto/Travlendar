﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
                Position = await GetPositionsForAddressSyncAsync (textPosition);
            }
            catch ( Exception e )
            {
                Debug.WriteLine ("TRAVLENDAR || GetPositionFromString error: " + e);
                DependencyService.Get<IMessage> ().ShortAlert (textPosition + "not found");
            }
        }

        public async System.Threading.Tasks.Task<Position> GetPositionsForAddressSyncAsync (string textPosition)
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
