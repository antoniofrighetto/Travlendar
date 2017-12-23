using System;
using Android;
using Xamarin.Forms;
using Travlendar.Renderers;

[assembly: ExportRenderer(typeof(TimePicker), typeof(CustomTimePicker))]
namespace Travlendar.Renderers
{
    public class CustomTimePicker : Xamarin.Forms.Platform.Android.TimePickerRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);
            Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            Control.SetPadding(0, 0, 0, 0);
        }

    }
}