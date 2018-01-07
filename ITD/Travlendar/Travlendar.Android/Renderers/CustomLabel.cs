using Android.Content;
using Android.Views;
using Travlendar.Droid.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Label), typeof(CustomLabel))]

namespace Travlendar.Droid.Renderers
{
    public class CustomLabel : Xamarin.Forms.Platform.Android.LabelRenderer
    {
        public CustomLabel(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {

                Control.Gravity = GravityFlags.CenterVertical;
                Control.SetTextColor(Android.Graphics.Color.Black);
            }

        }
    }
}
