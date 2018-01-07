using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;
using Travlendar.Core.AppCore.Model;
using System.Collections.ObjectModel;

namespace Travlendar.Core.AppCore.Pages
{
    public partial class SettingsPage : ContentPage
    {
        List<int> timeIntervalOption = new List<int>();

        public SettingsPage(INavigation navigation)
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel(this, navigation);
            BackgroundColor = Color.White;
        }

        void Handle_OnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            if (e.Value)
            {
                timer.IsEnabled = true;
                picker.IsEnabled = true;
                minutes.IsEnabled = true;
               
            }
            else
            {
                picker.IsEnabled = false;
                minutes.IsEnabled = false;
                timer.IsEnabled = false;
            }
        }

        private void timer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                if ((int)timer.Time.TotalMinutes > 840)
                {
                    picker.Title = "-";
                    picker.Items.Clear();
                    picker.IsEnabled = false;
                    return;
                }

                if ((int)timer.Time.TotalMinutes <= 840)
                {
                    timeIntervalOption.Clear();
                    picker.Items.Clear();
                    picker.IsEnabled = true;
                    int difference = 870 - (int)timer.Time.TotalMinutes;
                    difference = (difference > 180) ? 120 : difference;
                    for (int index = difference; index > 29; index--)
                    {
                        timeIntervalOption.Add(index);

                    }
                }

                timeIntervalOption.Reverse();
                foreach (int value in timeIntervalOption)
                {
                    picker.Items.Add(value.ToString());
                    picker.SelectedItem = "30";
                }
            }
        }
    }
}