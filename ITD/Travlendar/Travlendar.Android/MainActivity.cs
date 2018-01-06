using Amazon;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Facebook;

//Permissions needed for Facebook SDK on Android
[assembly: Permission (Name = Android.Manifest.Permission.Internet)]
[assembly: Permission (Name = Android.Manifest.Permission.WriteExternalStorage)]
//Metadata to initialize the Facebook SDK on Android
[assembly: MetaData ("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
[assembly: MetaData ("com.facebook.sdk.ApplicationName", Value = "@string/app_name")]
namespace Travlendar.Droid
{
    [Activity (Label = "Travlendar", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static ICallbackManager CallbackManager;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            global::Xamarin.Forms.Forms.Init (this, bundle);

            //FacebookSDK Config
            FacebookSdk.SdkInitialize (this.ApplicationContext);
            CallbackManager = CallbackManagerFactory.Create ();

            //AWS SDK Config
            var loggingConfig = AWSConfigs.LoggingConfig;
            loggingConfig.LogMetrics = true;
            loggingConfig.LogResponses = ResponseLoggingOption.Always;
            loggingConfig.LogMetricsFormat = LogMetricsFormatOption.JSON;
            loggingConfig.LogTo = LoggingOptions.SystemDiagnostics;
            AWSConfigs.AWSRegion = "eu-west-1";

            //Google Maps Config
            Xamarin.FormsMaps.Init (this, bundle);

            ActionBar.SetIcon (Android.Resource.Color.Transparent);
            LoadApplication (new App ());
        }

        /* 
            Override OnActivityResult to catch Facebook result Intent data.
            Then pass the values to the CallBackManager instance and this will trigger the
            FacebookCallbackManager<LoginResult> We have define on the FacebookButtonRenderer.
        */
        protected override void OnActivityResult (int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult (requestCode, resultCode, intent);
            CallbackManager.OnActivityResult (requestCode, (int) resultCode, intent);
        }

        public override void OnRequestPermissionsResult (int requestCode, string [] permissions, Android.Content.PM.Permission [] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult (requestCode, permissions, grantResults);
        }
    }
}
