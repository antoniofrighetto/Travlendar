using Travlendar.Core.AppCore.Tools;
using Travlendar.iOS.Dependencies;
using UIKit;


[assembly: Xamarin.Forms.Dependency (typeof (StatusBarImplementation))]
namespace Travlendar.iOS.Dependencies
{
    public class StatusBarImplementation : IStatusBar
    {
        public StatusBarImplementation ()
        {
            
        }

        #region IStatusBar implementation

        public void HideStatusBar ()
        {
            UIApplication.SharedApplication.StatusBarHidden = true;
        }

        public void ShowStatusBar ()
        {
            UIApplication.SharedApplication.StatusBarHidden = false;
        }

        #endregion
    }
}
