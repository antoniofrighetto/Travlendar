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
            }
            else
            {
                timer.IsVisible = false;
            }
        }
    }
}