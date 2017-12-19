
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class CalendarMasterDetailPage : MasterDetailPage
    {
        public CalendarMasterDetailPage ()
        {
            Master = new MenuPage
            {
                Title = "Prova"
            };

            Detail = new NavigationPage (new CalendarPage (new CalendarViewModel (this.Navigation)));
        }

        private async void Map_ClickedAsync (object sender, System.EventArgs e)
        {
            var page = new MapPage ();
            //NavigationPage.SetHasNavigationBar (page, false);
            await Navigation.PushAsync (page);
        }
    }
}
