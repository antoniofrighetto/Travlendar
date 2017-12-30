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

        public void RemoveTicket (string name)
        {
            name = Tickets.FirstOrDefault (x => x.Value == name).Key;
            Tickets.Remove (name);

            CognitoSyncViewModel.GetInstance ().RemoveFromDataset (DATASET_NAME, name);
            if ( PropertyChanged != null )
                PropertyChanged (this, null);
        }

        private IDictionary<string, string> LoadTickets ()
        {
            CognitoSyncViewModel.GetInstance ().CreateDataset ("Tickets");
            return CognitoSyncViewModel.GetInstance ().ReadWholeDataset (DATASET_NAME);
        }
    }
}
