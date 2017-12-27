using Travlendar.Core.AppCore.Helpers;
using Travlendar.Core.AppCore.ViewModels;
using Travlendar.Framework.Dependencies;
using Xamarin.Forms;

using Telerik.XamarinForms.Input;
using Telerik.XamarinForms.Common;
using FormsPlugin.Iconize;

namespace Travlendar.Core.AppCore.Pages
{
    public partial class CalendarPage : ContentPage
    {
        private double width = 0;
        private double height = 0;
        private RadCalendar calendar;

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


            var addAppointmentButton = new IconToolbarItem
            {
                Icon = Device.RuntimePlatform == Device.Android ? "ion-plus-round" : "",
                IconColor = Color.White,
                Text = "Add",
                Order = ToolbarItemOrder.Primary,
                Priority = 1
            };

            addAppointmentButton.SetBinding(MenuItem.CommandProperty, new Binding("AddAppointmentCommand"));

            var changeViewButton = new IconToolbarItem
            {
                Icon = Device.RuntimePlatform == Device.Android ? "ion-navicon-round" : "",
                IconColor = Color.White,
                Text = "BarButton",
                Order = ToolbarItemOrder.Primary,
                Priority = 0,
                Command = new Command(() =>
                {
                    if (calendar.ViewMode != CalendarViewMode.Day)
                    {
                        calendar.TrySetViewMode(CalendarViewMode.Day, true);
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
            //CHIUDIAMO L'APP 
            return true;
        }

        /*public class CalendarPage : ContentPage
        {
            CalendarViewModel _viewModel;
            private ToolbarItem addAppointment;
            private ToolbarItem tickets;
            private ToolbarItem settings;
            private ToolbarItem logout;

            public CalendarPage (CalendarViewModel vm)
            {
                _viewModel = vm;
                FillMenu ();

                Button map = new Button
                {
                    Text = "Map -> WIP Toolbar",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                map.Clicked += Map_ClickedAsync;
                Content = new StackLayout
                {
                    Children = {
                        new Label { Text = "CalendarPage!" },
                        new Label { Text = "Qui mettiamo il componente di telerik!" },
                        map
                    }
                };
                BackgroundColor = Color.White;
            }


            private async void Map_ClickedAsync (object sender, System.EventArgs e)
            {
                var page = new MapPage (new MapViewModel (this.Navigation));
                //NavigationPage.SetHasNavigationBar (page, false);
                await Navigation.PushAsync (page);
            }

            private void FillMenu ()
            {
                addAppointment = new ToolbarItem
                {
                    Text = "Add",
                    Order = ToolbarItemOrder.Primary,
                    Priority = 0
                };

                tickets = new ToolbarItem
                {
                    Text = "Tickets",
                    Order = ToolbarItemOrder.Secondary,
                    Priority = 0
                };

                settings = new ToolbarItem
                {
                    Text = "Settings",
                    Order = ToolbarItemOrder.Secondary,
                    Priority = 0
                };

                logout = new ToolbarItem
                {
                    Text = "Logout",
                    Order = ToolbarItemOrder.Secondary,
                    Priority = 0
                };

                RegisterEvents ();

                ToolbarItems.Add (addAppointment);
                ToolbarItems.Add (tickets);
                ToolbarItems.Add (settings);
                ToolbarItems.Add (logout);
            }
        }*/
    }
}
