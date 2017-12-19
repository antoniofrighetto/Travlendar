using Travlendar.Core.AppCore.Pages;
using Travlendar.Core.AppCore.ViewModels;
using Travlendar.Pages;
using Xamarin.Forms;

namespace Travlendar
{
    public class App : Application
    {
        public App ()
        {
            var vm = LoginViewModel.GetInstance ();
            vm.PropertyChanged += Vm_PropertyChanged;

            var page = new LandingPage ();
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
                            MainPage = new CalendarMasterDetailPage ();
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

        protected override void OnStart ()
        {
            // Handle when your app starts
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }
    }
}
