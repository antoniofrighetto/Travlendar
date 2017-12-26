using Travlendar.Core.AppCore.Pages;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;
using FormsPlugin.Iconize;

namespace Travlendar
{
    public class App : Application
    {
        private NavigationPage navigationPage;
        public App ()
        {
            var vm = LoginViewModel.GetInstance();
            vm.PropertyChanged += Vm_PropertyChanged;

            /* Toolbar icons by Iconize can be used only on Android */
            if (Device.RuntimePlatform == Device.Android) {
                navigationPage = new IconNavigationPage(new LandingPage(LoginViewModel.GetInstance()));    
            } else {
                navigationPage = new NavigationPage(new LandingPage(LoginViewModel.GetInstance()));
            }
            NavigationPage.SetHasNavigationBar(navigationPage, false);

            // The root page of application
            MainPage = navigationPage;
        }

        private async void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = sender as LoginViewModel;
            if (e.PropertyName == "Success")
            {
                if (vm.Success)
                {
                    navigationPage.BarBackgroundColor = Constants.TravlendarAquaGreen;
                    navigationPage.BarTextColor = Color.White;
                    await navigationPage.PushAsync(new CalendarPage());
                }
                else
                {
                    if (!(MainPage is LoginPage))
                    {
                        MainPage = new LoginPage(vm);
                    }
                }
            }
        }
    }
}
