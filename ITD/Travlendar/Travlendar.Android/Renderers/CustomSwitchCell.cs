using Android.Content;
using Android.Support.V4.Content.Res;
using Android.Views;
using Android.Widget;
using Travlendar.Droid.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(SwitchCell), typeof(CustomSwitchCell))]
namespace Travlendar.Droid.Renderers
{
    public class CustomSwitchCell : Xamarin.Forms.Platform.Android.SwitchCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {

            var cell = base.GetCellCore(item, convertView, parent, context);
            var child1 = ((LinearLayout)cell).GetChildAt(1);

            var label = (TextView)((LinearLayout)child1).GetChildAt(0);
            label.SetTextColor(Android.Graphics.Color.Black);

            return cell;

        }

    }
}
