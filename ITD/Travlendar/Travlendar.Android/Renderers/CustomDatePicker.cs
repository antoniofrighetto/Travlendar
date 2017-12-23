using System;
using Android;
using Xamarin.Forms;
using Travlendar.Renderers;

[assembly: ExportRenderer(typeof(DatePicker), typeof(CustomDatePicker))]
namespace Travlendar.Renderers
{
    public class CustomDatePicker : Xamarin.Forms.Platform.Android.DatePickerRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            Control.SetPadding(0, 0, 0, 0);

        }
    }
}