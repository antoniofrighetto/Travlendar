using Android.Content;
using Android.Views;
using Travlendar.Droid.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Picker), typeof(CustomPicker))]
namespace Travlendar.Droid.Renderers
{
    public class CustomPicker : Xamarin.Forms.Platform.Android.PickerRenderer
    {
        public CustomPicker(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                Control.SetTextColor(Android.Graphics.Color.Black);

            }
        }

    }
}
