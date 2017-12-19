using System.ComponentModel;
using Travlendar.Core.AppCore.Model;
using Travlendar.Framework.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.ViewModels
{
    public class CalendarViewModel : AViewModel<CalendarModel>
    {
        public CalendarViewModel (INavigation navigation)
        {
            this.Navigation = navigation;
        }

        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
