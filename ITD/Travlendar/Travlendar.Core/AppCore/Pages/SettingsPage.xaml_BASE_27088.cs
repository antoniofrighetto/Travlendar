using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{

    public partial class SettingsPage : ContentPage
    {

        public SettingsPage (INavigation navigation)
        {
            InitializeComponent ();
            BindingContext = new SettingsViewModel (this, navigation);
            BackgroundColor = Color.White;
        }

        void Handle_OnChanged (object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            if ( e.Value )
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