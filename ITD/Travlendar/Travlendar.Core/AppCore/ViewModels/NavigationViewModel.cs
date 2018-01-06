using System;
using System.Net;
using Travlendar.Core.Framework.Dependencies;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.ViewModels
{
    /// <summary>
    /// NavigationViewModel singleton class handling the Navigation to a place
    /// </summary>
    sealed public class NavigationViewModel
    {
        private static NavigationViewModel _instance = new NavigationViewModel ();
        private NavigationViewModel ()
        { }

        static internal NavigationViewModel GetInstance ()
        {
            return _instance;
        }

        /// <summary>
        /// Navigate logic which optimize the mean of transport options
        /// </summary>
        public void Navigate (string location, bool car, bool bike, bool publicTransport, bool minimizeCarbonFootPrint)
        {
            string mean = "d";

            //to be added && distance to location > 15 min
            if ( minimizeCarbonFootPrint )
            {
                mean = "w";
                if ( bike )
                    mean = "b";
                if ( publicTransport )
                    mean = "transit";
                if ( !bike && !publicTransport )
                    mean = "w";
            }
            else
            {
                if ( bike )
                    mean = "b";
                if ( publicTransport )
                    mean = "transit";
                if ( car )
                    mean = "d";
            }

            //Advising the user of which mean of transport is going to use
            switch ( mean )
            {
                case "b":
                    DependencyService.Get<IMessage> ().ShortAlert ("Navigate Biking");
                    break;
                case "transit":
                    DependencyService.Get<IMessage> ().ShortAlert ("Navigate with Public Transport");
                    break;
                case "d":
                    DependencyService.Get<IMessage> ().ShortAlert ("Navigate Driving");
                    break;
                case "w":
                    DependencyService.Get<IMessage> ().ShortAlert ("Navigate Walking");
                    break;
            }

            //Each platform uses a different url for opening the Navigation app
            switch ( Device.RuntimePlatform )
            {
                case Device.iOS:
                    Device.OpenUri (new Uri (string.Format ("{0}destination={1}&mode={2}", Constants.NavigationURL_iOS, WebUtility.UrlEncode (location), WebUtility.UrlEncode (mean))));
                    break;
                case Device.Android:
                    Device.OpenUri (new Uri (string.Format ("{0}{1}&mode={2}", Constants.NavigationURL_Android, WebUtility.UrlEncode (location), WebUtility.UrlEncode (mean))));
                    break;
            }
            return;
        }
    }
}
