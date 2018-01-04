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
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Travlendar.Core.AppCore.ViewModels
{
    public class CalendarViewModel : BindableBaseNotify
    {
        private CalendarPage page;
        private INavigation navigation;
        private RadCalendar calendar;

        private static ObservableCollection<Appointment> appointmentsList;
        public ObservableCollection<Appointment> AppointmentList
        {
            get { return appointmentsList; }
        }

        private Command addAppointmentCommand;
        public ICommand AddAppointmentCommand
        {
            get
            {
                return addAppointmentCommand ?? (addAppointmentCommand = new Command(
                    async () => await navigation.PushAsync(new AppointmentCreationPage(appointmentsList, "Creation", null), true)
                ));
            }
        }

        private Command todayCommand;
        public ICommand TodayCommand
        {
            get
            {
                return todayCommand ?? (todayCommand = new Command(() =>
                {
                    if (calendar.ViewMode != CalendarViewMode.Month)
                    {
                        calendar.TrySetViewMode(CalendarViewMode.Month);
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
                return settingsCommand ?? (settingsCommand = new Command(async () =>
                {
                    await navigation.PushAsync(new SettingsPage(navigation,appointmentsList));
                }));
            }
        }

        private Command logoutCommand;
        public ICommand LogoutCommand
        {
            get
            {
                return logoutCommand ?? (logoutCommand = new Command(() =>
                {
                    DependencyService.Get<ITools>().LogoutFromFacebook();
                    Device.BeginInvokeOnMainThread(async () => await navigation.PopToRootAsync());
                }));
            }
        }

        private Command ticketsCommand;
        public ICommand TicketsCommand
        {
            get
            {
                return ticketsCommand ?? (ticketsCommand = new Command(async () =>
                {
                    await navigation.PushAsync(new TicketsPage(navigation, TicketsViewModel.GetInstance()));
                }));
            }
        }

        private ToolbarItem changeViewButton;
        public ToolbarItem ChangeViewButton
        {
            get { return changeViewButton; }
            set { this.SetProperty(ref changeViewButton, value, "ChangeViewButton"); }
        }

        public CalendarViewModel(CalendarPage page, INavigation navigation, RadCalendar calendar)
        {
            this.page = page;
            this.navigation = navigation;
            this.calendar = calendar;

            LoadAppointments();

            MessagingCenter.Subscribe<AppointmentCreationPage, object[]>(this, "CreationAppointments", (sender, values) =>
            {
                if (values[0].ToString() == "Update")
                {
                    Appointment oldAppointment = values[1] as Appointment;
                    appointmentsList.Remove(oldAppointment);
                    CognitoSyncViewModel.GetInstance().RemoveFromDataset(Constants.APPOINTMENTS_DATASET_NAME, oldAppointment.Key);
                }

                Appointment newAppointment = values[2] as Appointment;
                appointmentsList.Add(newAppointment);

                string json = JsonConvert.SerializeObject(newAppointment);
                CognitoSyncViewModel.GetInstance().WriteDataset(Constants.APPOINTMENTS_DATASET_NAME, newAppointment.Key, json);

                calendar.AppointmentsSource = AppointmentList;
            });

            calendar.AppointmentTapped += async (sender, e) =>
            {
                Appointment a = (e.Appointment as Appointment);
                var az = appointmentsList.FirstOrDefault(item => item.Title == a.Title && item.StartDate == a.StartDate && item.EndDate == a.EndDate && item.IsAllDay == a.IsAllDay);
                /* We can safely assume we got the key */
                a.Key = az.Key;
                await navigation.PushAsync(new AppointmentCreationPage(appointmentsList, "Update", a));
            };
        }

        public void calendarViewChanged(object sender, ValueChangedEventArgs<CalendarViewMode> e)
        {
            var viewMode = ((RadCalendar)sender).ViewMode;
            switch (viewMode)
            {
                case CalendarViewMode.Day:
                    ChangeViewButton.Text = string.Empty; break;
                case CalendarViewMode.YearNumbers:
                case CalendarViewMode.Year:
                    ChangeViewButton.Text = string.Empty; break;
                case CalendarViewMode.MonthNames:
                    ChangeViewButton.Text = string.Concat("Go to ", ((RadCalendar)sender).DisplayDate.Year.ToString()); break;
                case CalendarViewMode.Month:
                    {
                        if (Device.RuntimePlatform != Device.iOS)
                            ChangeViewButton.Text = string.Concat("Go to ", ((RadCalendar)sender).DisplayDate.Year.ToString());
                        else
                            ChangeViewButton.Text = string.Concat("Go to ", ((RadCalendar)sender).DisplayDate.Date.ToString("MMMM"));
                        break;
                    }
            }
        }

        public void dateChanged(object sender, ValueChangedEventArgs<object> e)
        {
            var viewMode = ((RadCalendar)sender).ViewMode;
            var date = ((RadCalendar)sender).DisplayDate;
            switch (viewMode)
            {
                case CalendarViewMode.Month:
                    {
                        if (Device.RuntimePlatform != Device.iOS)
                            ChangeViewButton.Text = string.Concat("Go to ", ((RadCalendar)sender).DisplayDate.Year.ToString());
                        else
                            ChangeViewButton.Text = string.Concat("Go to ", date.Date.ToString("MMMM"));
                        break;
                    }
                case CalendarViewMode.MonthNames:
                    ChangeViewButton.Text = string.Concat("Go to ", date.Year.ToString()); break;
            }
        }

        private void LoadAppointments()
        {
            appointmentsList = new ObservableCollection<Appointment>();
            CognitoSyncViewModel.GetInstance().CreateDataset(Constants.APPOINTMENTS_DATASET_NAME);
            IDictionary<string, string> appointments = CognitoSyncViewModel.GetInstance().ReadWholeDataset(Constants.APPOINTMENTS_DATASET_NAME);
            foreach (KeyValuePair<string, string> item in appointments)
            {
                Appointment a = JsonConvert.DeserializeObject<Appointment>(item.Value);
                a.Color = parseColor(item.Value);
                appointmentsList.Add(a);
            }
            calendar.AppointmentsSource = AppointmentList;
        }

        private static Color parseColor(string json) {
            JObject rootObject = JObject.Parse(json);
            JObject color = (JObject)rootObject["Color"];
            return new Color(color.Property("R").Value.ToObject<double>(),
                             color.Property("G").Value.ToObject<double>(),
                             color.Property("B").Value.ToObject<double>(),
                             color.Property("A").Value.ToObject<double>());
        }
    }
}