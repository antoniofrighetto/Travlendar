using System.ComponentModel;
using Travlendar.Core.AppCore.Model;
using Travlendar.Framework.ViewModels;

namespace Travlendar.Core.AppCore.ViewModels
{
    public class CalendarViewModel : AViewModel<CalendarModel>
    {
        public CalendarViewModel ()
        {

        }

        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
