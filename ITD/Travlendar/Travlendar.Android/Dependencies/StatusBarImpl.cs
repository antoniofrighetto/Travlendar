using Android.App;
using Android.Views;
using Travlendar.Core.AppCore.Helpers;
using Travlendar.Droid.Dependencies;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency (typeof (StatusBarImplementation))]
namespace Travlendar.Droid.Dependencies
{
    public class StatusBarImplementation : IStatusBar
    {
        WindowManagerFlags _originalFlags;

        #region IStatusBar implementation

        public void HideStatusBar ()
        {
            var activity = (Activity) Forms.Context;
            var attrs = activity.Window.Attributes;
            _originalFlags = attrs.Flags;
            attrs.Flags |= Android.Views.WindowManagerFlags.Fullscreen;
            activity.Window.Attributes = attrs;
        }

        public void ShowStatusBar ()
        {
            var activity = (Activity) Forms.Context;
            var attrs = activity.Window.Attributes;
            attrs.Flags = _originalFlags;
            activity.Window.Attributes = attrs;
        }

        #endregion
    }
}
