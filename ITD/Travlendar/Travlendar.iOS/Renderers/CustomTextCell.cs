using System;
using Xamarin.Forms;
using UIKit;

using Travlendar.iOS.Renderers;

[assembly: ExportRenderer(typeof(TextCell), typeof(CustomTextCell))]
namespace Travlendar.iOS.Renderers
{
    public class CustomTextCell : global::Xamarin.Forms.Platform.iOS.TextCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            cell.TextLabel.TextAlignment = UITextAlignment.Right;
            return cell;
        }
    }
}
