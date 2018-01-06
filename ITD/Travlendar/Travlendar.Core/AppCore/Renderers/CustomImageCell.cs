using Travlendar.Core.AppCore.Pages;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Renderers
{
    public class CustomImageCell : ImageCell
    {
        //Custom cell for displaying tickets
        public CustomImageCell ()
        {
            var moreAction = new MenuItem { Text = "View Ticket" };
            moreAction.SetBinding (MenuItem.CommandParameterProperty, new Binding ("."));
            moreAction.Clicked += MoreAction_Clicked;

            var deleteAction = new MenuItem { Text = "Delete Ticket", IsDestructive = true }; // red background
            deleteAction.SetBinding (MenuItem.CommandParameterProperty, new Binding ("."));
            deleteAction.Clicked += DeleteAction_Clicked;

            ContextActions.Add (moreAction);
            ContextActions.Add (deleteAction);
        }

        private void MoreAction_Clicked (object sender, System.EventArgs e)
        {
            var mi = ((MenuItem) sender);
            var page = new FullscreenImagePage (mi.CommandParameter.ToString ());
            if ( Device.RuntimePlatform == Device.Android )
                NavigationPage.SetHasNavigationBar (page, false);
            Application.Current.MainPage.Navigation.PushAsync (page);
        }

        private void DeleteAction_Clicked (object sender, System.EventArgs e)
        {
            var mi = ((MenuItem) sender);
            TicketsViewModel.GetInstance ().RemoveTicket (mi.CommandParameter.ToString ());
        }
    }
}
