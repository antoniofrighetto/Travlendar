using System.ComponentModel;
using Travlendar.Core.AppCore.Model;
using Travlendar.Framework.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.ViewModels
{
    public class TicketsViewModel : AViewModel<TicketModel>
    {
        public TicketsViewModel (INavigation navigator)
        {
            this.Navigation = navigator;
        }

        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
