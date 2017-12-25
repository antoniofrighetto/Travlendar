using Android.Content;
using Travlendar.Droid.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer (typeof (TimePicker), typeof (CustomTimePicker))]
namespace Travlendar.Droid.Renderers
{
    public class CustomTimePicker : Xamarin.Forms.Platform.Android.TimePickerRenderer
    {
        public CustomTimePicker (Context context) : base (context)
        {
        }

        protected override void OnElementChanged (Xamarin.Forms.Platform.Android.ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged (e);
            if ( Control != null )
            {
                Control.SetBackgroundColor (Android.Graphics.Color.Transparent);
                Control.SetPadding (0, 0, 0, 0);
            }
        }

    }
}
