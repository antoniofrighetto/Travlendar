using Android.Content;
using Android.Views;
using Travlendar.Droid.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer (typeof (Entry), typeof (CustomEntryCell))]
namespace Travlendar.Droid.Renderers
{
    public class CustomEntryCell : Xamarin.Forms.Platform.Android.EntryRenderer
    {
        public CustomEntryCell (Context context) : base (context)
        {
        }

        protected override void OnElementChanged (Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged (e);
            if ( Control != null )
            {
                Control.SetBackgroundColor (Android.Graphics.Color.Transparent);
                Control.Gravity = GravityFlags.CenterVertical; 
            }
        }
    }
}
