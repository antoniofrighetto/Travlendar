using Foundation;
using Travlendar.Core.AppCore.Helpers;
using UIKit;

namespace Travlendar.iOS.Dependencies
{
    public class AppExistance : IExistance
    {
        /// <summary>
        /// Check the existance of an app in iOS
        /// </summary>
        public bool ApplicationExistance (string app)
        {
            return UIApplication.SharedApplication.CanOpenUrl (new NSUrl (app));
        }
    }
}
