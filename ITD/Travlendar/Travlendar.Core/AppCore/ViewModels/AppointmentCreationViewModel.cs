using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Travlendar.Core.AppCore.Model;
using Travlendar.Core.AppCore.Pages;
using Travlendar.Framework.ViewModels;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;

namespace Travlendar.Core.AppCore.ViewModels
{
    public class AppointmentCreationViewModel : BindableBaseNotify
    {
        private AppointmentCreationPage page;
        private INavigation navigation;
        private ObservableCollection<Appointment> appointments;
        private Appointment appointment;
        private string message;
        private static object[] settingsValue = null;
        private static bool flag = true;

        private string titleAppointment;
        public string TitleAppointment
        {
            get { return titleAppointment; }
            set { this.SetProperty(ref titleAppointment, value); }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set { this.SetProperty(ref location, value); }
        }

        private bool isAllDayOn;
        public bool IsAllDayOn
        {
            get { return isAllDayOn; }
            set { SetProperty(ref isAllDayOn, value); }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { SetProperty(ref startDate, value); }
        }

        private TimeSpan startTime;
        public TimeSpan StartTime
        {
            get { return startTime; }
            set { SetProperty(ref startTime, value); }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set { SetProperty(ref endDate, value); }
        }

        private TimeSpan endTime;
        public TimeSpan EndTime
        {
            get { return endTime; }
            set { SetProperty(ref endTime, value); }
        }

        private string detail;
        public string Detail
        {
            get { return detail; }
            set { SetProperty(ref detail, value); }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
            set { SetProperty(ref color, value); }
        }

        private bool isAlertOn;
        public bool IsAlertOn
        {
            get { return isAlertOn; }
            set { SetProperty(ref isAlertOn, value); }
        }

        private Command saveAppointmentCommand;
        public ICommand SaveAppointmentCommand
        {
            get
            {
                return saveAppointmentCommand ?? (saveAppointmentCommand = new Command(async () =>
                {
                    if (string.IsNullOrWhiteSpace(this.TitleAppointment))
                    {
                        await page.DisplayAlert("Title cannot be empty.", "", "Ok");
                    }
                    else if (StartDate > EndDate || (StartDate < EndDate && StartTime > EndTime))
                    {
                        await page.DisplayAlert("Cannot Save Event", "The start date must be before the end date.", "Ok");
                    }
                    else
                    {
                        Appointment ap = new Appointment
                        {
                            Title = this.TitleAppointment,
                            IsAllDay = this.IsAllDayOn,
                            StartDate = this.StartDate.AddHours(StartTime.Hours).AddMinutes(StartTime.Minutes),
                            EndDate = this.EndDate.AddHours(EndTime.Hours).AddMinutes(EndTime.Minutes),
                            Detail = this.Detail,
                            Color = this.Color == Color.Default ? Color.FromRgb(28, 109, 107) : this.Color,
                            Location = this.Location
                        };

                        ap.Key = ap.GetHashCode().ToString();
                        await CreateAppointment(ap);
                    }
                }));
            }
        }

        private Command removeAppointmentCommand;
        public ICommand RemoveAppointmentCommand
        {
            get
            {
                return removeAppointmentCommand ?? (removeAppointmentCommand = new Command(async () =>
                {
                    CognitoSyncViewModel.GetInstance().RemoveFromDataset(Constants.APPOINTMENTS_DATASET_NAME, appointment.Key);
                    appointments.Remove(appointment);
                    await navigation.PopAsync(true);
                }));
            }
        }

        private Command navigateAppointmentCommand;
        public ICommand NavigateAppointmentCommand
        {
            get
            {
                SettingsViewModel settings = new SettingsViewModel(null, null);

                return navigateAppointmentCommand ?? (navigateAppointmentCommand = new Command(() =>
                {
                    NavigationViewModel.GetInstance().Navigate(Location, settings.Car, settings.Bike, settings.PublicTransport, settings.MinimizeCarbonFootPrint);
                }));
            }
        }

        public AppointmentCreationViewModel(AppointmentCreationPage page, INavigation navigation, ObservableCollection<Appointment> aps, string msg, Appointment a)
        {
            this.page = page;
            this.navigation = navigation;
            this.appointments = aps;
            this.message = msg;
            this.appointment = a;

            if (message == "Update")
            {
                this.TitleAppointment = a.Title;
                this.StartDate = a.StartDate;
                this.EndDate = a.EndDate;
                this.StartTime = a.StartDate.TimeOfDay;
                this.EndTime = a.EndDate.TimeOfDay;
                this.Detail = a.Detail;
                this.IsAllDayOn = a.IsAllDay;
                this.Color = a.Color;
                this.Location = a.Location;
            }
            else
            {
                this.StartDate = DateTime.Now;
                this.EndDate = DateTime.Now;
                this.StartTime = DateTime.Now.TimeOfDay;
                this.EndTime = DateTime.Now.TimeOfDay;
                this.Location = "Location";
            }

            /* It doesn't make sense reading every time the settings, we just read them the first time the constructor is invoked... */
            if (flag)
            {
                ReadSettingsData();
                flag = false;
            }

            /* ... and whenever settings are changed. */
            MessagingCenter.Subscribe<SettingsPage>(this, "SettingsChangedEvent", (obj) => {
                ReadSettingsData();
            });

            MessagingCenter.Subscribe<CalendarTypePage, Color>(this, "ColorEvent", (sender, color) =>
            {
                this.Color = color;
            });

            MessagingCenter.Subscribe<AppointmentCreationPage, string>(this.page, "LocationNameSaved", (sender, loc) => {
                this.Location = loc;
            });

        }

        private void ReadSettingsData()
        {
            try
            {
                CognitoSyncViewModel.GetInstance().CreateDataset("Settings");
                string json = CognitoSyncViewModel.GetInstance().ReadDataset("Settings", "UserSettings");
                JObject rootObject = JObject.Parse(json);
                settingsValue = new object[3];
                settingsValue[0] = rootObject["lunchBreak"].ToObject<bool>();
                settingsValue[1] = rootObject["timeBreak"].ToObject<TimeSpan>();
                settingsValue[2] = rootObject["timeInterval"].ToObject<TimeSpan>();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Dataset does not exist. Exception: " + e.Message);
            }
        }

        private async Task CreateAppointment(Appointment newAppointment)
        {
            var overlappedEvent = appointments.FirstOrDefault(item => item.StartDate == newAppointment.StartDate
                                                              || (item.StartDate <= newAppointment.EndDate && item.EndDate >= newAppointment.StartDate)
                                                              || (item.StartDate.Date == newAppointment.StartDate.Date && item.IsAllDay && newAppointment.IsAllDay)
                                                             );
            if (overlappedEvent != null && message != "Update")
            {
                if (overlappedEvent.Title == newAppointment.Title)
                {
                    await page.DisplayAlert("Event already added.", "", "Ok");
                    return;
                }

                bool response = await page.DisplayAlert("Overlapped Event", String.Format("There's another event ({0}) scheduled for this time interval, are you sure to continue?", overlappedEvent.Title), "Continue", "Cancel");
                if (!response)
                {
                    return;
                }
            }

            if (settingsValue != null && (bool)settingsValue[0])
            {
                TimeSpan timeBreak = (TimeSpan)settingsValue[1];
                TimeSpan timeInterval = (TimeSpan)settingsValue[2];
                if (newAppointment.StartDate.TimeOfDay == timeBreak || (timeBreak <= newAppointment.EndDate.TimeOfDay && timeBreak.Add(timeInterval) >= newAppointment.StartDate.TimeOfDay))
                {
                    if (!newAppointment.Title.ToLower().Contains("lunch"))
                    {
                        await page.DisplayAlert("Overlapped Event", "LunchBreak mode is active and there's no space left for it. Disable settings if you want to save appointment anyway.", "Ok");
                        return;
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine("we out here " + newAppointment.Title);
            object[] values = new object[] { message, appointment, newAppointment };
            MessagingCenter.Send<AppointmentCreationPage, object[]>(this.page, "CreationAppointments", values);
            await navigation.PopAsync(true);
        }
    }
}