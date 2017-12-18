using Amazon;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Xamarin.Facebook;

//Permissions needed for Facebook SDK on Android
[assembly: Permission (Name = Android.Manifest.Permission.Internet)]
[assembly: Permission (Name = Android.Manifest.Permission.WriteExternalStorage)]
//You need to add this metadata to initialize FacebookActivity, you can add it on your strings.xml file
[assembly: MetaData ("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
[assembly: MetaData ("com.facebook.sdk.ApplicationName", Value = "@string/app_name")]
namespace Travlendar.Droid
{
    [Activity (Label = "Travlendar", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

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

            //Start

            LoadApplication (new App ());
        }

        /* 
            Override OnActivityResult to catch Facebook result Intent data.
            Then pass the values to the CallBackManager instance and this will trigger the
            FacebookCallbackManager<LoginResult> We have define on the Renderer.

            If you use other intents that pass through this method, you should identify them by yourself
        */
        protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult (requestCode, resultCode, data);
            CallbackManager.OnActivityResult (requestCode, (int) resultCode, data);
        }
    }
}
