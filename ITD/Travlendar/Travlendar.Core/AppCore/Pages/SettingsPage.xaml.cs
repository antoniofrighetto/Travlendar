using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Travlendar.Core.AppCore.Pages
{

    public partial class SettingsPage : ContentPage
    {


        List<int> timeIntervalOption = new List<int>();
        //Label errorLabel = new Label
        //{
        //    Text = string.Concat("Sorry, select time from 11.30 to 14.00!"),
        //    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
        //    HorizontalOptions = LayoutOptions.CenterAndExpand,
        //    TextColor = Color.Red

        //};

        public SettingsPage(INavigation navigation)
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel(this, navigation);

        }



        void Handle_OnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            if (e.Value)
            {
                timer.IsVisible = true;
                picker.IsVisible = true;
                minutes.IsVisible = true;
            }
            else
            {
                picker.IsVisible = false;
                minutes.IsVisible = false;
                timer.IsVisible = false;
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