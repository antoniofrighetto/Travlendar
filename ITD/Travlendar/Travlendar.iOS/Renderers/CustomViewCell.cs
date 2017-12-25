using System;
using Xamarin.Forms;
using UIKit;
using Foundation;
using Travlendar.iOS.Renderers;

using Travlendar.Core.AppCore.Pages;

[assembly: ExportRenderer(typeof(ViewCell), typeof(CustomViewCell))]
namespace Travlendar.iOS.Renderers
{
    public class CustomViewCell : global::Xamarin.Forms.Platform.iOS.ViewCellRenderer
    {

        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);

            item.Tapped += (sender, e) =>
            {
                Cell currentCell = (Cell)sender;
                TableView table = currentCell.Parent as TableView;
                if (table.Parent is CalendarTypePage)
                {
                    Cell[] z = new Cell[4];
                    for (int i = 0; i < 4; i++)
                    {
                        z[i] = table.Root[0][i] as Cell;
                        if (currentCell.ClassId != z[i].ClassId && z[i].StyleId == "checkmark")
                        {
                            z[i].StyleId = "none";
                            UITableViewCell cellz = null;
                            for (int j = 0; j < tv.NumberOfSections(); j++)
                            { //should be just 1 section
                                cellz = tv.CellAt(NSIndexPath.FromRowSection(i, j));
                            }
                            changeStyleId(z[i], cellz);
                        }
                    }
                    changeStyleId(currentCell, cell);
                }
            };

            changeStyleId(item, cell);
            return cell;
        }

        private void changeStyleId(Cell item, UITableViewCell cell)
        {
            switch (item.StyleId)
            {
                case "none":
                    cell.Accessory = UITableViewCellAccessory.None;
                    break;
                case "disclosure":
                    cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                    break;
                case "checkmark":
                    cell.Accessory = UITableViewCellAccessory.Checkmark;
                    break;
            }
        }
    }
}