using System;
using Travlendar.Core.AppCore.Helpers;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class LoginPage : ContentPage
    {
        LoginViewModel _viewModel;
        private StackLayout layoutContainer;
        public StackLayout layout;

        public Label title;
        public Label userLabel;
        public Label passwordLabel;

        public Entry username;
        public Entry password;

        //Future implementation of the AWS Login with Email/Password
        public LoginPage (LoginViewModel vm)
        {
            DependencyService.Get<IStatusBar> ().HideStatusBar ();
            _viewModel = vm;

            layoutContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White
            };

            layout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.White
            };

            userLabel = new Label
            {
                Text = "Username/Email address",
                TextColor = Constants.TravlendarAquaGreen,
                HorizontalOptions = LayoutOptions.Center
            };

            passwordLabel = new Label
            {
                Text = "Password",
                TextColor = Constants.TravlendarAquaGreen,
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

            RegisterEvents ();
            layout.Children.Add (userLabel);
            layout.Children.Add (username);
            layout.Children.Add (passwordLabel);
            layout.Children.Add (password);

            layoutContainer.Children.Add (layout);

            this.Content = layoutContainer;
        }

        private void Username_Completed (object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException ();
        }

        private void Password_Completed (object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException ();
        }

        private void RegisterEvents ()
        {
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
