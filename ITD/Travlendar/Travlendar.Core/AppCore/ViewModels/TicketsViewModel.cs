using System.Collections.Generic;
using System.ComponentModel;
using Travlendar.Core.AppCore.Model;
using Travlendar.Framework.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.ViewModels
{
    public class TicketsViewModel : AViewModel<TicketModel>
    {
        public override event PropertyChangedEventHandler PropertyChanged;
        private static TicketsViewModel _instance = new TicketsViewModel ();

        static internal TicketsViewModel GetInstance ()
        {
            return _instance;
        }

        Dictionary<string, ImageSource> tickets;

        public Dictionary<string, ImageSource> Tickets
        {
            get
            {
                return tickets;
            }
            set
            {
                tickets = value;
            }
        }

        private TicketsViewModel ()
        {
            Tickets = new Dictionary<string, ImageSource> ();
        }
    }
}
