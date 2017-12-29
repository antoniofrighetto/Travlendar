using Android.Content;
using Android.OS;
using Travlendar.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof (SearchBar), typeof (CustomSearchBarRenderer))]
namespace Travlendar.Droid.Renderers
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        public CustomSearchBarRenderer (Context context) : base (context)
        {
        }

        protected override void OnElementChanged (ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged (e);

            if ( e.OldElement != null || Element == null )
                return;

            if ( Build.VERSION.SdkInt >= BuildVersionCodes.N )
                Element.HeightRequest = 40;
            Element.BackgroundColor = Color.FromRgb (202, 201, 207);
        }
    }
}
