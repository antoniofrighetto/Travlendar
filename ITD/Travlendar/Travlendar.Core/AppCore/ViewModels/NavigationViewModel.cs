using System;
using System.Net;
using Travlendar.Core.Framework.Dependencies;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.ViewModels
{
    sealed public class NavigationViewModel
    {
        private static NavigationViewModel _instance = new NavigationViewModel ();
        private NavigationViewModel ()
        { }

        static internal NavigationViewModel GetInstance ()
        {
            return _instance;
        }

        public void Navigate (string location, bool car, bool bike, bool publicTransport, bool minimizeCarbonFootPrint)
        {
            string mean = "w";

            //to be added && distance to location > 15 min
            if ( minimizeCarbonFootPrint )
            {
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
