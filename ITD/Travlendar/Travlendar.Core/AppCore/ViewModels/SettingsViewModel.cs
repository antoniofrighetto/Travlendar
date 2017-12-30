using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Travlendar.Framework.ViewModels;
using Travlendar.Core.AppCore.Model;
using Travlendar.Core.AppCore.Pages;
using Newtonsoft.Json;

namespace Travlendar.Core.AppCore.ViewModels
{
    class SettingsViewModel : BindableBaseNotify
    {

        //private SQLiteConnection database = DependencyService.Get<ISQLite>().InitConnection();
        private SettingsPage page;
        private Settings settings;
        private INavigation navigation;


        private string age;
        public string Age
        {
            get { return this.age; }
            set { this.SetProperty(ref this.age, value, "Age"); }
        }

        private bool car;
        public bool Car
        {
            get { return this.car; }
            set { this.SetProperty(ref this.car, value); }
        }

        private bool bike;
        public bool Bike
        {
            get { return this.bike; }
            set { this.SetProperty(ref this.bike, value); }
        }

        private bool publicTransport;
        public bool PublicTransport
        {
            get { return this.publicTransport; }
            set { this.SetProperty(ref this.publicTransport, value); }
        }

        private bool sharedCar;
        public bool SharedCar
        {
            get { return this.sharedCar; }
            set { this.SetProperty(ref this.sharedCar, value); }
        }

        private bool sharedBike;
        public bool SharedBike
        {
            get { return this.sharedBike; }
            set { this.SetProperty(ref this.sharedBike, value); }
        }

        private bool minimizeCarbonFootPrint;
        public bool MinimizeCarbonFootPrint
        {
            get { return this.minimizeCarbonFootPrint; }
            set { this.SetProperty(ref this.minimizeCarbonFootPrint, value); }
        }

        private bool lunchBreak;
        public bool LunchBreak
        {
            get { return this.lunchBreak; }
            set { this.SetProperty(ref this.lunchBreak, value); }
        }

        private TimeSpan timeBreak;
        public TimeSpan TimeBreak
        {
            get { return this.timeBreak; }
            set { this.SetProperty(ref this.timeBreak, value); }
        }

        public string timeInterval;
        public string TimeInterval
        {
            get { return this.timeInterval; }
            set { this.SetProperty(ref this.timeInterval, value); }
        }



        public SettingsViewModel(SettingsPage page, INavigation navigation)
        {
            this.page = page;
            this.navigation = navigation;

            LoadSettings();

            this.age = settings.age.ToString();
            this.car = settings.car;
            this.bike = settings.bike;
            this.publicTransport = settings.publicTransport;
            this.sharedCar = settings.sharedCar;
            this.sharedBike = settings.sharedBike;
            this.minimizeCarbonFootPrint = settings.minimizeCarbonFootPrint;
            this.lunchBreak = settings.lunchBreak;
            this.timeBreak = settings.timeBreak;
            this.timeInterval = settings.timeInterval.ToString();
        }

        private async void LoadSettings()
        {
            try
            {
                CognitoSyncViewModel.GetInstance().CreateDataset("Settings");
                string settingsStringFormat = CognitoSyncViewModel.GetInstance().ReadDataset("Settings", "UserSettings");
                if (settingsStringFormat == null)
                {
                    settings = new Settings();
                }
                else
                {
                    settings = JsonConvert.DeserializeObject<Settings>(settingsStringFormat);
                    int i = 0;
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
            }

            //database.CreateTable<Settings>();
            //if (database.Table<Settings>().Any())
            //{
            //    settings = database.Get<Settings>(0);
            //}
            //else { settings = new Settings(); }

        }

        private Command save;
        public ICommand Save
        {
            get
            {
                return save ?? (save = new Command(async () =>

                {
                    if ((this.timeBreak.TotalMinutes < 690 || this.timeBreak.TotalMinutes > 840))
                    {
                        await page.DisplayAlert("Please insert a valid time from 11.30 to 14.00", "", "OK");
                        return;
                    }

                    if (int.TryParse(this.age, out var number) == false)
                    {
                        await page.DisplayAlert("Please insert a valid value", "", "Ok");
                    }
                    else
                    {
                        Settings settings = new Settings
                        {
                            age = int.Parse(this.age),
                            car = this.car,
                            bike = this.bike,
                            publicTransport = this.publicTransport,
                            sharedCar = this.sharedCar,
                            sharedBike = this.sharedBike,
                            minimizeCarbonFootPrint = this.minimizeCarbonFootPrint,
                            lunchBreak = this.lunchBreak,
                            timeBreak = this.timeBreak,
                            timeInterval = int.Parse(this.timeInterval)
                        };

                        await page.DisplayAlert("Settings correctly inserted", "", "Ok");
                        await (syncSettings(settings));
                    }
                }));
            }
        }


        private async Task syncSettings(Settings settings)
        {
            string settingJSON = JsonConvert.SerializeObject(settings);
            CognitoSyncViewModel.GetInstance().WriteDataset("Settings", "UserSettings", settingJSON);

        }
    }
}
