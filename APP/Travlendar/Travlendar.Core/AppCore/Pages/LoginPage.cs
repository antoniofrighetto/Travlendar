using Travlendar.AppCore.ViewModels;
using Travlendar.Core.AppCore.Pages;
using Travlendar.Dependencies;
using Travlendar.Renderers;
using Xamarin.Forms;

namespace Travlendar.Pages
{
    public class LoginPage : ContentPage
    {
        public CognitoSyncViewModel cognitoSyncViewModel;
        public Image backgroundImage;
        public RelativeLayout relativeLayout;
        public StackLayout layout;
        public Label title;
        public FacebookButton fbButton;

        public LoginPage ()
        {
            cognitoSyncViewModel = new CognitoSyncViewModel (this.Navigation);

            relativeLayout = new RelativeLayout
            {
                VerticalOptions = LayoutOptions.Fill
            };

            backgroundImage = new Image
            {
                Source = "login_background.jpg"
            };

            title = new Label ()
            {
                Text = "Travlendar+",
                FontSize = 32,
                TextColor = Color.White
            };

            layout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.White
            };

            fbButton = new FacebookButton ();
            fbButton.HorizontalOptions = LayoutOptions.Center;
            fbButton.HeightRequest = 50;
            fbButton.VerticalOptions = LayoutOptions.Center;
            //Add your event handler for the OnLogin to operate with the Facebook credentials comming from SDK
            fbButton.OnLogin -= LoginWithFacebook;
            fbButton.OnLogin += LoginWithFacebook;

            relativeLayout.Children.Add (backgroundImage, Constraint.Constant (0), Constraint.Constant (0));
            relativeLayout.Children.Add (title, Constraint.RelativeToParent (parent => (parent.Width / 2) - 75), Constraint.RelativeToParent (parent => parent.Height / 2));

            layout.Children.Add (relativeLayout);
            layout.Children.Add (fbButton);
            this.Content = layout;
        }

        private async void LoginWithFacebook (object sender, FacebookEventArgs e)
        {
            /*
                If you successfully login to facebook, you must got the three parameters that login return to the app.
                Handle whatever credetianls storage here with the data you have recovered. If you are using Parse Login with Facebook
                you may use DependencyService to access your service and pass the parameters you have recovered.
                Example:
            */
            //var success = await DependencyService.Get<IParse>().LoginWithFacebook(e.UserId, e.AccessToken, e.TokenExpiration);

            bool success = (!string.IsNullOrEmpty (e.UserId) &&
                            !string.IsNullOrEmpty (e.AccessToken) &&
                            e.TokenExpiration != null);

            if ( success )
            {
                //Logging in AWS Cognito Federal Entities Pool
                cognitoSyncViewModel.AWSLogin (Constants.FB_PROVIDER, e.AccessToken);

                var page = new CalendarPage ();
                NavigationPage.SetHasNavigationBar (page, false);
                await Navigation.PushAsync (page);
                //var message = string.Format ("You have succesfully access to your facebook account. Data returned:\n\nUserId: {0}\n\nAccess Token: {1}\n\nExpiration Date: {2}",
                //    e.UserId, e.AccessToken, e.TokenExpiration);
                //await DisplayAlert ("Success", message, "Ok");
            }
            else
            {
                await DisplayAlert (
                    "Error",
                    "There was an error trying to login to facebook",
                    "Ok"
                );
            }
            DependencyService.Get<ITools> ().LogoutFromFacebook ();
        }
    }
}
