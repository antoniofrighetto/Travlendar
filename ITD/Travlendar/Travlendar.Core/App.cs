using Travlendar.Core.AppCore.Pages;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar
{
    public class App : Application
    {
        public App ()
        {
            var vm = LoginViewModel.GetInstance ();
            vm.PropertyChanged += Vm_PropertyChanged;

            var page = new LandingPage (LoginViewModel.GetInstance ());
            NavigationPage.SetHasNavigationBar (page, false);
            // The root page of your application
            MainPage = new NavigationPage (page);
        }

        private void Vm_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = sender as LoginViewModel;
            switch ( e.PropertyName )
            {
                case "Success":
                    {
                        if ( vm.Success )
                        {
                            NavigationPage navigationPage;
                            navigationPage = new NavigationPage (new CalendarPage ())
                            {
                                BarBackgroundColor = Constants.TravlendarAquaGreen,
                                BarTextColor = Color.White
                            };

                            MainPage = navigationPage;
                        }
                        else
                        {
                            if ( !(MainPage is LoginPage) )
                            {
                                MainPage = new LoginPage (vm);
                            }
                        }
                        break;
                    }
            }
        }
    }
}
