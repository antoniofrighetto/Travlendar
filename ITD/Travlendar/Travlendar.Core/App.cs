using Travlendar.Core.AppCore.Pages;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar
{
    public class App : Application
    {
        private NavigationPage navigationPage;
        public App ()
        {
            var vm = LoginViewModel.GetInstance ();
            vm.PropertyChanged += Vm_PropertyChanged;

            navigationPage = new NavigationPage (new LandingPage (LoginViewModel.GetInstance ()));
            NavigationPage.SetHasNavigationBar (navigationPage, false);

            // The root page of application
            MainPage = navigationPage;
        }

        private async void Vm_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = sender as LoginViewModel;
            if ( e.PropertyName == "Success" )
            {
                if ( vm.Success )
                {
                    var page = new CalendarPage ();
                    navigationPage.BarBackgroundColor = Constants.TravlendarAquaGreen;
                    navigationPage.BarTextColor = Color.White;
                    NavigationPage.SetHasBackButton (page, false);
                    await navigationPage.PushAsync (page);
                }
                else
                {
                    if ( !(MainPage is LoginPage) )
                    {
                        MainPage = new LoginPage (vm);
                    }
                }
            }
        }
    }
}
