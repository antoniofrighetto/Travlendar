using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Travlendar.Core.AppCore.Model;
using Travlendar.Core.AppCore.ViewModels;

using Plugin.LocalNotifications;

namespace Travlendar.Core.AppCore.Pages
{
    public partial class AppointmentCreationPage : ContentPage
    {
        private static int flag = 0;
        private const int ID = 1;
        private Appointment appointment;

        public AppointmentCreationPage(ObservableCollection<Appointment> appointments, string message, Appointment appointment)
        {
            InitializeComponent();
            Title = message == "Creation" ? "New Event" : "Modify Event";
            if (message == "Creation")
            {
                Table.RemoveAt(3);
            } else {
                if (LocationLabel.Text != null && LocationLabel.Text != "Location") {
                    LocationLabel.TextColor = Color.Black;
                    LocationLabel.WidthRequest = 250;
                    LocationLabel.FontSize = 10;
                }
            }

            if (Device.RuntimePlatform == Device.iOS)
                NavigationPage.SetHasBackButton(this, false);

            this.appointment = appointment;
            BindingContext = new AppointmentCreationViewModel(this, this.Navigation, appointments, message, appointment);

            var saveAppointmentButton = new ToolbarItem
            {
                Text = "Save",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            saveAppointmentButton.SetBinding(MenuItem.CommandProperty, new Binding("SaveAppointmentCommand"));
            ToolbarItems.Add(saveAppointmentButton);

            if (Device.RuntimePlatform == Device.iOS)
            {
                var cancelButton = new ToolbarItem
                {
                    Text = "Cancel",
                    Order = ToolbarItemOrder.Primary,
                    Command = new Command(async () => await Navigation.PopAsync()),
                    Priority = 1
                };
                ToolbarItems.Add(cancelButton);
            }

            CalendarTypeViewCell.Tapped += async (sender, e) =>
            {
                await Navigation.PushAsync(new CalendarTypePage());
            };

            LocationViewCell.Tapped += async (sender, e) =>
            {
                await Navigation.PushAsync(new MapPage());
            };

            StartDatePicker.DateSelected += (sender, e) => {
                EndDatePicker.Date = StartDatePicker.Date;
            };

            MessagingCenter.Subscribe<MapPage, string>(this, "LocationNameEvent", (sender, location) => {
                LocationLabel.TextColor = Color.Black;
                LocationLabel.FontSize = 10;
                location = location.Replace("\n", " ");
                LocationLabel.Text = location;
                MessagingCenter.Send(this, "LocationNameSaved", LocationLabel.Text);
            });
        }

        private void IsAllDayOnChanged(object sender, ToggledEventArgs e)
        {
            switch (((SwitchCell)sender).On)
            {
                case true:
                    StartTimePicker.IsVisible = false; EndTimePicker.IsVisible = false; EndDatePicker.Date = StartDatePicker.Date; break;
                case false:
                    StartTimePicker.IsVisible = true; EndTimePicker.IsVisible = true; break;
            }
        }


        private async void IsAlertOnChanged(object sender, ToggledEventArgs e)
        {
            var switcher = (SwitchCell)sender;
            if (TitleApp.Text == null)
            {
                if (switcher.On)
                {
                    await DisplayAlert("Set title before activating an alert.", "", "Ok");
                    switcher.On = false;
                }
                return;
            }

            switch (switcher.On)
            {
                case true:
                    {
                        if (flag == 0)
                        {
                            await DisplayAlert("Alert enabled", "You will be notified 10 minutes ahead of schedule.", "Ok");
                            flag = 1;
                        }
                        {
                            CrossLocalNotifications.Current.Show("Hey!",
                                                                 "Event " + TitleApp.Text + " is going to start within 10 minutes.",
                                                                 ID,
                                                                 StartDatePicker.Date.AddMinutes(StartTimePicker.Time.Minutes - 10).AddSeconds(StartTimePicker.Time.Seconds));
                        }
                        break;
                    }
                case false:
                    CrossLocalNotifications.Current.Cancel(ID);
                    break;
            }
        }
    }
}
