using System;
using Xamarin.Forms;
using Travlendar.Droid;
using Android.Content;
using Android.Graphics;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryCell))]

namespace Travlendar.Renderers
{
    public class CustomEntryCell : Xamarin.Forms.Platform.Android.EntryRenderer

    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            Control.SetPadding(0, 0, 0, 0);
        }



    }
}