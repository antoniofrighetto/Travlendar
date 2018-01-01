
using Android.App;
using Android.OS;

namespace Travlendar.Droid
{
    [Activity (Theme = "@style/Theme.Splash", Icon = "@drawable/icon", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            StartActivity (typeof (MainActivity));
        }
    }
}
