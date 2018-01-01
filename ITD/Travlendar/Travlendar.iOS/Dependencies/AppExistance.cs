using Foundation;
using Travlendar.Core.AppCore.Helpers;
using UIKit;

namespace Travlendar.iOS.Dependencies
{
    public class AppExistance : IExistance
    {
        public bool ApplicationExistance (string app)
        {
            return UIApplication.SharedApplication.CanOpenUrl (new NSUrl (app));
        }
    }
}
