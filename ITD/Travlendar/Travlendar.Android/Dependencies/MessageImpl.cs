
using Android.App;
using Android.Widget;
using Travlendar.Core.Framework.Dependencies;
using Travlendar.Droid.Dependencies;

[assembly: Xamarin.Forms.Dependency (typeof (MessageImpl))]
namespace Travlendar.Droid.Dependencies
{
    public class MessageImpl : IMessage
    {
        public void LongAlert (string message)
        {
            Toast.MakeText (Application.Context, message, ToastLength.Long).Show ();
        }

        public void ShortAlert (string message)
        {
            Toast.MakeText (Application.Context, message, ToastLength.Short).Show ();
        }
    }
}
