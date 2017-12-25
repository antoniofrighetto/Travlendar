using System;
using UIKit;
using Xamarin.Forms;
using Travlendar.iOS.Renderers;

[assembly: ExportRenderer(typeof(DatePicker), typeof(CustomDatePicker))]
namespace Travlendar.iOS.Renderers
{
    public class CustomDatePicker : global::Xamarin.Forms.Platform.iOS.DatePickerRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            Control.BorderStyle = UITextBorderStyle.None;
        }
    }
}