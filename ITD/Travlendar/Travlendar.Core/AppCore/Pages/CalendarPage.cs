using Travlendar.Core.AppCore.Helpers;
using Travlendar.Core.AppCore.ViewModels;
using Travlendar.Framework.Dependencies;
using Xamarin.Forms;

using Telerik.XamarinForms.Input;
using Telerik.XamarinForms.Common;

namespace Travlendar.Core.AppCore.Pages
{
    public partial class CalendarPage : ContentPage
    {
        private double width = 0;
        private double height = 0;
        private RadCalendar calendar;

        /* After pushing CalendarPage() into the NavigationPage stack (App.cs:33), constructor is invoked. Here, an instance of a calendar is created, some of its properties and event are
         * set up. All the buttons shown in CalendarPage() are instances of ToolbarItems. The business logic of this page can be found in CalendarViewModel(). */
        public CalendarPage()
        {
            DependencyService.Get<IStatusBar>().ShowStatusBar();

            calendar = new RadCalendar
            {
                WeekNumbersDisplayMode = DisplayMode.Hide,
                DayNamesDisplayMode = DisplayMode.Show,
                GridLinesWidth = 0,
            };

            this.BindingContext = new CalendarViewModel(this, this.Navigation, calendar);
            calendar.ViewChanged += ((CalendarViewModel)this.BindingContext).calendarViewChanged;
            calendar.DisplayDateChanged += ((CalendarViewModel)this.BindingContext).dateChanged;

            if (Device.RuntimePlatform == Device.iOS)
            {
                calendar.DayNameCellStyle = new CalendarCellStyle { FontSize = 12 };
                var layout = new RelativeLayout();
                this.changeLayout(calendar, layout);
                layout.SizeChanged += (sender, e) =>
                {
                    this.changeLayout(calendar, layout);
                };
            }
            else
            {
                Content = calendar;
            }

            var addAppointmentButton = new ToolbarItem
            {
                Icon = Device.RuntimePlatform == Device.Android ? "add_ic" : "",
                Text = "Add",
                Order = ToolbarItemOrder.Primary,
                Priority = 1
            };

            addAppointmentButton.SetBinding(MenuItem.CommandProperty, new Binding("AddAppointmentCommand"));

            var changeViewButton = new ToolbarItem
            {
                Icon = Device.RuntimePlatform == Device.Android ? "day_ic" : "",
                Text = "BarButton",
                Order = ToolbarItemOrder.Primary,
                Priority = 0,
                Command = new Command(() =>
                {
                    var selectedDate = calendar.SelectedDate;
                    if (calendar.ViewMode != CalendarViewMode.Day)
                    {
                        calendar.TrySetViewMode(CalendarViewMode.Day, true);
                        if (selectedDate.HasValue) {
                            calendar.DisplayDate = selectedDate.Value;
                            calendar.SelectedDate = selectedDate;    
                        }
                    }
                    else
                    {
                        calendar.TrySetViewMode(CalendarViewMode.Month, true);
                    }
                })
            };

            ToolbarItems.Add(changeViewButton);
            ToolbarItems.Add(addAppointmentButton);

            string date = string.Empty;
            if (calendar.ViewMode == CalendarViewMode.Month)
            {
                if (Device.RuntimePlatform == Device.iOS)
                    date = calendar.DisplayDate.Date.ToString("MMMM");
                else
                    date = calendar.DisplayDate.Year.ToString();
            }
            else if (calendar.ViewMode == CalendarViewMode.MonthNames)
                date = calendar.DisplayDate.Year.ToString();

            ((CalendarViewModel)this.BindingContext).ChangeViewButton = new ToolbarItem
            {
                Text = date == string.Empty ? string.Empty : string.Concat("Go to ", date),
                Command = new Command(() =>
                {
                    switch (calendar.ViewMode)
                    {
                        case CalendarViewMode.Month:
                            {
                                if (Device.RuntimePlatform == Device.iOS)
                                    calendar.TrySetViewMode(CalendarViewMode.MonthNames);
                                else
                                    calendar.TrySetViewMode(CalendarViewMode.Year);
                                break;
                            }
                        case CalendarViewMode.MonthNames:
                            {
                                if (Height > Width)
                                    calendar.TrySetViewMode(CalendarViewMode.Year);
                                else
                                    calendar.TrySetViewMode(CalendarViewMode.YearNumbers);
                                break;
                            }
                        case CalendarViewMode.YearNumbers:
                            break;
                    }
                }),
                Icon = Device.RuntimePlatform == Device.Android ? "goto_ic" : "",
                Order = ToolbarItemOrder.Primary,
                Priority = 1
            };
            ToolbarItems.Add(((CalendarViewModel)this.BindingContext).ChangeViewButton);

            string[] secondaryButtonTitles = new string[] { "Today", "Settings", "Tickets", "Logout" };
            ToolbarItem[] secondaryToolbar = new ToolbarItem[secondaryButtonTitles.Length];
            for (int i = 0; i < secondaryButtonTitles.Length; i++)
            {
                secondaryToolbar[i] = new ToolbarItem
                {
                    Text = secondaryButtonTitles[i],
                    Order = ToolbarItemOrder.Secondary
                };
                secondaryToolbar[i].SetBinding(MenuItem.CommandProperty, new Binding(string.Concat(secondaryButtonTitles[i], "Command")));
                ToolbarItems.Add(secondaryToolbar[i]);
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            /* There is a bug on the calendar for iOS in landscape orientation */
            if (Device.RuntimePlatform == Device.iOS && (this.width != width || this.height != height))
            {
                this.width = width;
                this.height = height;
                if (width > height && calendar.ViewMode == CalendarViewMode.Year)
                    calendar.TrySetViewMode(CalendarViewMode.YearNumbers);
            }
        }

        private void changeLayout(View calendarz, RelativeLayout layout)
        {
            layout.Children.Add(calendarz, Constraint.RelativeToParent((parent) =>
            {
                return parent.X;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Y;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width;
            }), Constraint.RelativeToParent((parent) =>
            {
                if (Height > Width)
                {
                    return parent.Height - 44;
                }
                return parent.Height - 33;
            }));
            Content = layout;
        }

        protected override bool OnBackButtonPressed ()
        {
            return true;
        }
    }
}
