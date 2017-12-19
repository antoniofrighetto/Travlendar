using System;
using Travlendar.Core.AppCore.ViewModels;
using Travlendar.Renderers;
using Xamarin.Forms;

namespace Travlendar.Pages
{
    public class LoginPage : ContentPage
    {
        LoginViewModel _viewModel;

        public StackLayout layout;
        public FacebookButton fbButton;

        public Label title;
        public Label userLabel;
        public Label passwordLabel;

        public Entry username;
        public Entry password;



        public LoginPage (LoginViewModel vm)
        {
            _viewModel = vm;

            layout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.White
            };

            userLabel = new Label
            {
                Text = "Username/Email address",
                TextColor = Constants.TravlendarBlue,
                HorizontalOptions = LayoutOptions.Center
            };

            passwordLabel = new Label
            {
                Text = "Password",
                TextColor = Constants.TravlendarBlue,
                HorizontalOptions = LayoutOptions.Center
            };

            username = new Entry
            {
                Placeholder = "User",
                PlaceholderColor = Color.LightGray,
                Keyboard = Keyboard.Email,
                HorizontalOptions = LayoutOptions.Center
            };
            username.Completed += Username_Completed;

            password = new Entry
            {
                Placeholder = "Password",
                PlaceholderColor = Color.LightGray,
                IsPassword = true,
                HorizontalOptions = LayoutOptions.Center
            };
            password.Completed += Password_Completed;

            fbButton = new FacebookButton ();
            fbButton.HorizontalOptions = LayoutOptions.Center;
            fbButton.HeightRequest = 50;
            fbButton.VerticalOptions = LayoutOptions.Center;
            //Add your event handler for the OnLogin to operate with the Facebook credentials comming from SDK

            RegisterEvents ();
            layout.Children.Add (userLabel);
            layout.Children.Add (username);
            layout.Children.Add (passwordLabel);
            layout.Children.Add (password);
            layout.Children.Add (fbButton);

            this.Content = layout;
        }

        private void Username_Completed (object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException ();
        }

        private void Password_Completed (object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException ();
            //Need to implement the AWS Cognito standard login
        }

        private void RegisterEvents ()
        {
            fbButton.OnLogin -= _viewModel.LoginWithFacebook;
            fbButton.OnLogin += _viewModel.LoginWithFacebook;

            _viewModel.displayAlert -= _viewModel_displayAlertAsync;
            _viewModel.displayAlert += _viewModel_displayAlertAsync;
        }

        private async void _viewModel_displayAlertAsync (object sender, EventArgs e)
        {
            await DisplayAlert (
                    "Error",
                    "There was an error trying to login to facebook",
                    "Ok"
                );
        }
    }
}
