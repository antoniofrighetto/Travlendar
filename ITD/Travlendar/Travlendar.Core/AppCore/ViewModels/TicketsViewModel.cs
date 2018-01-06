using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Travlendar.Core.AppCore.Model;
using Travlendar.Core.Framework.Dependencies;
using Travlendar.Framework.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.ViewModels
{
    /// <summary>
    /// TicketsViewModel singleton class that handle the tickets
    /// </summary>
    public class TicketsViewModel : AViewModel<TicketModel>
    {
        private const string DATASET_NAME = "Tickets";
        public override event PropertyChangedEventHandler PropertyChanged;
        private static TicketsViewModel _instance = new TicketsViewModel ();

        static internal TicketsViewModel GetInstance ()
        {
            return _instance;
        }

        IDictionary<string, string> tickets;

        public IDictionary<string, string> Tickets
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
            Tickets = LoadTickets ();

        }

        /// <summary>
        /// Pick picture method that will handle the choice of the ticket from the User's media gallery
        /// </summary>
        public async Task<MediaFile> PickPictureAsync (string name)
        {
            if ( !CrossMedia.Current.IsPickPhotoSupported )
            {
                DependencyService.Get<IMessage> ().ShortAlert ("Photos Not Supported");
                return null;
            }
            var file = await CrossMedia.Current.PickPhotoAsync (new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium
            });

            if ( file == null )
                return null;

            return file;
        }

        /// <summary>
        /// Take picture method that will handle the process of taking a picture of a ticket
        /// </summary>
        public async Task<MediaFile> TakePictureAsync (string name)
        {
            if ( !CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported )
            {
                DependencyService.Get<IMessage> ().ShortAlert ("No camera avaialble");
                return null;
            }

            var file = await CrossMedia.Current.TakePhotoAsync (new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Tickets",
                Name = Regex.Replace (name, @"\s+", "") + ".jpg"
            });

            if ( file == null )
                return null;

            return file;
        }

        /// <summary>
        /// Saving the ticket path in AWS and locally
        /// </summary>
        public void SaveTicket (string name, string path)
        {
            try
            {
                name = Regex.Replace (name, @"\s+", "");
                Tickets.Add (name, path);
                if ( PropertyChanged != null )
                    PropertyChanged (this, null);
            }
            catch ( Exception e )
            {
                DependencyService.Get<IMessage> ().ShortAlert ("Ticket already added");
            }
            CognitoSyncViewModel.GetInstance ().WriteDataset (DATASET_NAME, name, path);
        }

        /// <summary>
        /// Removing the ticket.
        /// </summary>
        public void RemoveTicket (string name)
        {
            name = Tickets.FirstOrDefault (x => x.Value == name).Key;
            if ( name != null )
            {
                Tickets.Remove (name);
            }

            CognitoSyncViewModel.GetInstance ().RemoveFromDataset (DATASET_NAME, name);
            if ( PropertyChanged != null )
                PropertyChanged (this, new PropertyChangedEventArgs (name));
        }

        /// <summary>
        /// Loading the saved tickets.
        /// </summary>
        private IDictionary<string, string> LoadTickets ()
        {
            CognitoSyncViewModel.GetInstance ().CreateDataset ("Tickets");
            return CognitoSyncViewModel.GetInstance ().ReadWholeDataset (DATASET_NAME);
        }
    }
}
