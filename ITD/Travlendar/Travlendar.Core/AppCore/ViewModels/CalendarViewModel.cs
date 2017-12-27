using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Telerik.XamarinForms.Common;
using Telerik.XamarinForms.Input;
using Travlendar.Core.AppCore.Model;
using Travlendar.Core.AppCore.Pages;
using Travlendar.Framework.Dependencies;
using Travlendar.Framework.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.ViewModels
{
    public class CalendarViewModel : BindableBaseNotify
    {
        private CalendarPage page;
        private INavigation navigation;
        private RadCalendar calendar;

        private static ObservableCollection<Appointment> appointmentsList = new ObservableCollection<Appointment> ();
        public ObservableCollection<Appointment> AppointmentList
        {
            get { return appointmentsList; }
        }

        private Command addAppointmentCommand;
        public ICommand AddAppointmentCommand
        {
            get
            {
                return addAppointmentCommand ?? (addAppointmentCommand = new Command (
                    async () => await navigation.PushAsync (new AppointmentCreationPage (appointmentsList, "Creation", null), true)
                ));
            }
        }

        private Command todayCommand;
        public ICommand TodayCommand
        {
            get
            {
                return todayCommand ?? (todayCommand = new Command (() =>
                 {
                     if ( calendar.ViewMode != CalendarViewMode.Month )
                     {
                         calendar.TrySetViewMode (CalendarViewMode.Month);
                     }
                     calendar.DisplayDate = DateTime.Now;
                     calendar.SelectedDate = DateTime.Now;
                 }));
            }
        }

        private Command settingsCommand;
        public ICommand SettingsCommand
        {
            get
            {
                return settingsCommand ?? (settingsCommand = new Command (async () =>
                {
                    await navigation.PushAsync (new SettingsPage (navigation));
                }));
            }
        }

        private Command logoutCommand;
        public ICommand LogoutCommand
        {
            get
            {
                return logoutCommand ?? (logoutCommand = new Command (() =>
                 {
                     DependencyService.Get<ITools> ().LogoutFromFacebook ();
                     Device.BeginInvokeOnMainThread (async () => await navigation.PopToRootAsync ());
                 }));
            }
        }

        private Command ticketsCommand;
        public ICommand TicketsCommand
        {
            get
            {
                return ticketsCommand ?? (ticketsCommand = new Command (async () =>
                {
                    await navigation.PushAsync (new TicketsPage (TicketsViewModel.GetInstance ()));
                }));
            }
        }

        private ToolbarItem changeViewButton;
        public ToolbarItem ChangeViewButton
        {
            get { return changeViewButton; }
            set { this.SetProperty (ref changeViewButton, value, "ChangeViewButton"); }
        }

        public CalendarViewModel (CalendarPage page, INavigation navigation, RadCalendar calendar)
        {
            this.page = page;
            this.navigation = navigation;
            this.calendar = calendar;

            calendar.AppointmentsSource = AppointmentList;

            MessagingCenter.Subscribe<AppointmentCreationPage, Appointment> (this, "CreationAppointments", (sender, appointment) =>
            {
                appointmentsList.Add (appointment);
                calendar.AppointmentsSource = AppointmentList;
            });

            calendar.AppointmentTapped += async (sender, e) =>
            {
                await navigation.PushAsync (new AppointmentCreationPage (appointmentsList, "Update", (e.Appointment as Appointment)));
            };
        }

        public void calendarViewChanged (object sender, ValueChangedEventArgs<CalendarViewMode> e)
        {
            var viewMode = ((RadCalendar) sender).ViewMode;
            switch ( viewMode )
            {
                case CalendarViewMode.Day:
                    ChangeViewButton.Text = string.Empty; break;
                case CalendarViewMode.YearNumbers:
                case CalendarViewMode.Year:
                    ChangeViewButton.Text = string.Empty; break;
                case CalendarViewMode.MonthNames:
                    ChangeViewButton.Text = string.Concat ("Go to ", ((RadCalendar) sender).DisplayDate.Year.ToString ()); break;
                case CalendarViewMode.Month:
                    {
                        if ( Device.RuntimePlatform != Device.iOS )
                            ChangeViewButton.Text = string.Concat ("Go to ", ((RadCalendar) sender).DisplayDate.Year.ToString ());
                        else
                            ChangeViewButton.Text = string.Concat ("Go to ", ((RadCalendar) sender).DisplayDate.Date.ToString ("MMMM"));
                        break;
                    }
            }
        }

        public void dateChanged (object sender, ValueChangedEventArgs<object> e)
        {
            var viewMode = ((RadCalendar) sender).ViewMode;
            var date = ((RadCalendar) sender).DisplayDate;
            switch ( viewMode )
            {
                case CalendarViewMode.Month:
                    {
                        if ( Device.RuntimePlatform != Device.iOS )
                            ChangeViewButton.Text = string.Concat ("Go to ", ((RadCalendar) sender).DisplayDate.Year.ToString ());
                        else
                            ChangeViewButton.Text = string.Concat ("Go to ", date.Date.ToString ("MMMM"));
                        break;
                    }
                case CalendarViewMode.MonthNames:
                    ChangeViewButton.Text = string.Concat ("Go to ", date.Year.ToString ()); break;
            }
        }
    }
}
