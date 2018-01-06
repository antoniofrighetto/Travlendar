using Travlendar.Core.AppCore.Pages;
using Travlendar.Core.AppCore.ViewModels;
using Travlendar.Core.Framework.Dependencies;
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

            /* Application's entrypoint. navigationPage handles a stack of pages so that we are able to navigate through the various views when interacting
             * with the application. Whenever we a new view is displayed, a page is pushed into the handled stack, when we are done, it is popped out. First 
             * page we push in is LandingPage(). */
            MainPage = navigationPage;
        }

        private async void Vm_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = sender as LoginViewModel;
            if ( e.PropertyName == "Success" )
            {
                /* If we successfully log in to Facebook, a CalendarPage() is shown. */
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
                    DependencyService.Get<IMessage>().ShortAlert("Could not login to Facebook.");
                    /* Login page not yet implemented */
                    /*if ( !(MainPage is LoginPage) )
                    {
                        MainPage = new LoginPage (vm);
                    }*/
                }
            }
        }
    }
}
