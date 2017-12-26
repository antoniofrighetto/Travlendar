using System;
using Travlendar.iOS.Renderers;
using Xamarin.Forms;
using TelerikUI;
using Telerik.XamarinForms.InputRenderer.iOS;
using Telerik.XamarinForms.Input;
using Telerik.XamarinForms.Common;
using UIKit;
using Foundation;
using CoreGraphics;

[assembly: ExportRenderer(typeof(RadCalendar), typeof(CustomCalendarRenderer))]
namespace Travlendar.iOS.Renderers
{
    public class CustomCalendarRenderer : CalendarRenderer
    {
        protected override CalendarDelegate CreateCalendarDelegateOverride()
        {
            return new CustomCalendarDelegate();
        }
    }

    public class CustomCalendarDelegate : CalendarDelegate
    {
        public override void UpdateVisualsForCell(TKCalendar calendar, TKCalendarCell cell)
        {
            calendar.Theme = new TKCalendarIPadTheme();
            calendar.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            if (calendar.Presenter is TKCalendarMonthPresenter)
            {
                TKCalendarMonthPresenter presenter = calendar.Presenter as TKCalendarMonthPresenter;
                presenter.Style.DayNameTextEffect = TKCalendarTextEffect.None;
            }

            if (cell is TKCalendarCell)
            {
                TKCalendarDayCell dayCell = cell as TKCalendarDayCell;
                if (dayCell != null)
                {
                    TKCalendarDayState selectedState = TKCalendarDayState.Selected;
                    if ((dayCell.State & TKCalendarDayState.Today) != 0)
                    {
                        cell.Style.TextColor = UIColor.FromRGB(24, 149, 132);
                    }
                    if ((dayCell.State & selectedState) == selectedState)
                    {
                        cell.Style.BackgroundColor = UIColor.FromRGB(184, 184, 184);
                    }
                }
            }
            base.UpdateVisualsForCell(calendar, cell);
        }
    }
}
