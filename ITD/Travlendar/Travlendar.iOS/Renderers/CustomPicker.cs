using System;
using Xamarin.Forms;
using CoreGraphics;
using UIKit;
using Foundation;

using Travlendar.iOS.Renderers;

[assembly: ExportRenderer(typeof(Picker), typeof(CustomPicker))]
namespace Travlendar.iOS.Renderers
{
    public class CustomPicker : global::Xamarin.Forms.Platform.iOS.PickerRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                this.Control.BorderStyle = UITextBorderStyle.None;
            }
        }
    }
}