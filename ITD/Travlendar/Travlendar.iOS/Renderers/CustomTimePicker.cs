using System;
using Xamarin.Forms;
using UIKit;

using Travlendar.iOS.Renderers;

[assembly: ExportRenderer(typeof(TimePicker), typeof(CustomTimePicker))]
namespace Travlendar.iOS.Renderers
{
    public class CustomTimePicker : global::Xamarin.Forms.Platform.iOS.TimePickerRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);
            Control.BorderStyle = UITextBorderStyle.None;

        }
    }
}
