using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Travlendar.Core.AppCore.Model;
using Travlendar.Core.AppCore.Renderers;
using Travlendar.Core.Framework.Dependencies;
using Travlendar.Framework.Dependencies;
using Travlendar.Framework.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.ViewModels
{
    /// <summary>
    /// LoginViewModel singleton class that handle the login functionality.
    /// </summary>
    sealed public class LoginViewModel : AViewModel<LoginModel>
    {
        private static LoginViewModel _instance = new LoginViewModel ();

        public CognitoSyncViewModel cognitoSyncViewModel;

        public event EventHandler displayAlert;
        public override event PropertyChangedEventHandler PropertyChanged;

        bool success;
        public bool Success
        {
            get
            {
                return success;
            }
            set
            {
                success = value;
                RaisePropertyChanged ();
            }
        }

        private LoginViewModel ()
        {
            cognitoSyncViewModel = CognitoSyncViewModel.GetInstance ();
        }

        static internal LoginViewModel GetInstance ()
        {
            return _instance;
        }

        //Validating the custom event info and logging in AWS Cognito
        public void LoginWithFacebook (object sender, FacebookEventArgs e)
        {
            Success = (!string.IsNullOrEmpty (e.UserId) &&
                !string.IsNullOrEmpty (e.AccessToken) &&
                e.TokenExpiration != null);

            if ( Success )
            {
                //Logging in AWS Cognito Federal Entities Pool
                cognitoSyncViewModel.AWSLogin (Constants.FB_PROVIDER, e.AccessToken);
            }
            else
            {
                DependencyService.Get<IMessage> ().LongAlert ("Unsuccesful login");
            }
            DependencyService.Get<ITools> ().LogoutFromFacebook ();
        }

        private void RaisePropertyChanged ([CallerMemberName] string property = null)
        {
            var propChanged = PropertyChanged;
            if ( propChanged != null )
            {
                propChanged (this, new PropertyChangedEventArgs (property));
            }
        }
    }
}
