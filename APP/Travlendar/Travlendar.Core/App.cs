using Travlendar.Pages;
using Xamarin.Forms;

namespace Travlendar
{
    public class App : Application
    {
        public App ()
        {
            // The root page of your application
            var page = new LoginPage ();
            NavigationPage.SetHasNavigationBar (page, false);
            MainPage = new NavigationPage (page);
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
