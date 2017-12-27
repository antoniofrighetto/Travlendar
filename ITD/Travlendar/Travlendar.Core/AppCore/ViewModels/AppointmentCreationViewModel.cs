using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Travlendar.Core.AppCore.Model;
using Travlendar.Core.AppCore.Pages;
using Travlendar.Framework.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Travlendar.Core.AppCore.ViewModels
{
    public class AppointmentCreationViewModel : BindableBaseNotify
    {
        private AppointmentCreationPage page;
        private INavigation navigation;
        private ObservableCollection<Appointment> appointments;
        private Appointment appointment;
        private string message;

        private Position location;
        public Position Location
        {
            get { return Location; }
            set { this.SetProperty (ref location, value); }
        }

        private string titleAppointment;
        public string TitleAppointment
        {
            get { return titleAppointment; }
            set { this.SetProperty (ref titleAppointment, value); }
        }

        private bool isAllDayOn;
        public bool IsAllDayOn
        {
            get { return isAllDayOn; }
            set { SetProperty (ref isAllDayOn, value); }
        }

        private DateTime startDate = DateTime.Now;
        public DateTime StartDate
        {
            get { return startDate; }
            set { SetProperty (ref startDate, value); }
        }

        private TimeSpan startTime = DateTime.Now.TimeOfDay;
        public TimeSpan StartTime
        {
            get { return startTime; }
            set { SetProperty (ref startTime, value); }
        }

        private DateTime endDate = DateTime.Now;
        public DateTime EndDate
        {
            get { return endDate; }
            set { SetProperty (ref endDate, value); }
        }

        private TimeSpan endTime = DateTime.Now.TimeOfDay;
        public TimeSpan EndTime
        {
            get { return endTime; }
            set { SetProperty (ref endTime, value); }
        }

        private string detail;
        public string Detail
        {
            get { return detail; }
            set { SetProperty (ref detail, value); }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
            set { SetProperty (ref color, value); }
        }

        private bool isAlertOn;
        public bool IsAlertOn
        {
            get { return isAlertOn; }
            set { SetProperty (ref isAlertOn, value); }
        }

        private Command saveAppointmentCommand;
        public ICommand SaveAppointmentCommand
        {
            get
            {
                return saveAppointmentCommand ?? (saveAppointmentCommand = new Command (async () =>
                {
                    if ( string.IsNullOrWhiteSpace (this.TitleAppointment) )
                    {
                        await page.DisplayAlert ("Title cannot be empty.", "", "Ok");
                    }
                    else if ( StartDate > EndDate || (StartDate < EndDate && StartTime > EndTime) )
                    {
                        await page.DisplayAlert ("Cannot Save Event", "The start date must be before the end date.", "Ok");
                    }
                    else
                    {
                        Appointment ap = new Appointment
                        {
                            Title = this.TitleAppointment,
                            IsAllDay = this.IsAllDayOn,
                            StartDate = this.StartDate.AddHours (StartTime.Hours).AddMinutes (StartTime.Minutes),
                            EndDate = this.EndDate.AddHours (EndTime.Hours).AddMinutes (EndTime.Minutes),
                            Detail = this.Detail,
                            Color = this.Color == Color.Default ? Color.Yellow : this.Color
                        };
                        await CreateAppointment (ap);
                    }
                }));
            }
        }

        private Command removeAppointmentCommand;
        public ICommand RemoveAppointmentCommand
        {
            get
            {
                return removeAppointmentCommand ?? (removeAppointmentCommand = new Command (async () =>
                 {
                     appointments.Remove (appointment);
                     await navigation.PopAsync (true);
                 }));
            }
        }

        public AppointmentCreationViewModel (AppointmentCreationPage page, INavigation navigation, ObservableCollection<Appointment> aps, string msg, Appointment a)
        {
            this.page = page;
            this.navigation = navigation;
            this.appointments = aps;
            this.message = msg;
            this.appointment = a;

            if ( message == "Update" )
            {
                this.TitleAppointment = a.Title;
                this.StartDate = a.StartDate;
                this.EndDate = a.EndDate;
                this.Detail = a.Detail;
                this.IsAllDayOn = a.IsAllDay;
                this.Color = a.Color;
            }

            MessagingCenter.Subscribe<CalendarTypePage, Color> (this, "ColorEvent", (sender, color) =>
             {
                 this.Color = color;
             });
        }

        private async Task CreateAppointment (Appointment newAppointment)
        {
            var overlappedEvent = appointments.FirstOrDefault (item => item.StartDate == newAppointment.StartDate || (item.StartDate <= newAppointment.EndDate && item.EndDate >= newAppointment.StartDate));
            if ( overlappedEvent != null )
            {
                bool response = await page.DisplayAlert ("Overlapped Event", String.Format ("There's another event ({0}) scheduled for this time interval, are you sure to continue?", overlappedEvent.Title), "Continue", "Cancel");
                if ( !response )
                {
                    return;
                }
            }

            if ( message == "Update" )
            {
                appointments.Remove (appointment);
            }

            MessagingCenter.Send<AppointmentCreationPage, Appointment> (this.page, "CreationAppointments", newAppointment);
            await navigation.PopAsync (true);
        }
    }
}
