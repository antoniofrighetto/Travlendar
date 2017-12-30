using Android.Content;
using Android.Views;
using Travlendar.Droid.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer (typeof (DatePicker), typeof (CustomDatePicker))]
namespace Travlendar.Droid.Renderers
{
    public class CustomDatePicker : Xamarin.Forms.Platform.Android.DatePickerRenderer
    {
        public CustomDatePicker (Context context) : base (context)
        {
        }

        protected override void OnElementChanged (Xamarin.Forms.Platform.Android.ElementChangedEventArgs<DatePicker> e)
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
