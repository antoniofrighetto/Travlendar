using Xamarin.Forms;

namespace Travlendar
{
    public class Constants
    {
        //AWS Federal Identities Identity Pool ID
        public const string AWS_IDENTITY_POOL_ID = "eu-west-1:45c8b421-8dca-403d-9a82-3aea7c9a7879";

        //Facebook Login App ID
        public const string FB_APP_ID = "1779284975475344";

        //Facebook provide name
        public const string FB_PROVIDER = "graph.facebook.com";

        //Appointments Dataset
        public const string APPOINTMENTS_DATASET_NAME = "AppointmentDataset";

        //App Color
        public static Color TravlendarAquaGreen = Color.FromRgb (24, 149, 132);

        //Font Color
        public static Color TravlendarWhite = Color.White;

        //Google Maps Android
        public static string NavigationURL_Android = "google.navigation:q=";

        //Google Maps iOS
        public static string NavigationURL_iOS = "https://www.google.com/maps/dir/?api=1&";

        //Google Maps API KEY
        public static string MapsKey = "AIzaSyCpVbZOXvTgri28nC6KnFbdO79HmoVCUKw";
    }
}
