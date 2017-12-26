using System;
using Xamarin.Forms;
using CoreGraphics;
using UIKit;
using Foundation;

using Travlendar.iOS.Renderers;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntry))]
namespace Travlendar.iOS.Renderers
{
    public class CustomEntry : global::Xamarin.Forms.Platform.iOS.EntryRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                this.Control.BorderStyle = UITextBorderStyle.None;
            }
        }
    }
}