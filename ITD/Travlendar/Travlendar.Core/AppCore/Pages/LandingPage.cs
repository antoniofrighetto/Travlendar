using Travlendar.Core.AppCore.Helpers;
using Travlendar.Core.AppCore.Renderers;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class LandingPage : ContentPage
    {
        LoginViewModel _viewModel;

        private StackLayout buttons;
        private StackLayout layout;
        private Image backgroundImage;
        private Label title;
        private Button loginButton;
        private Button registerButton;
        public FacebookButton fbButton;

        public LandingPage (LoginViewModel vm)
        {
            DependencyService.Get<IStatusBar> ().HideStatusBar ();
            _viewModel = vm;

            buttons = new StackLayout
            {
                Padding = new Thickness (0, 20, 0, 20),
            };

            layout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.White
            };

            backgroundImage = new Image
            {
                Source = "login_background.jpg",
            };

            title = new Label ()
            {
                Text = "Travlendar+",
                FontSize = 32,
                TextColor = Color.White
            };

            fbButton = new FacebookButton ();
            fbButton.HorizontalOptions = LayoutOptions.Center;
            fbButton.HeightRequest = 50;
            fbButton.VerticalOptions = LayoutOptions.Center;

            //Add your event handler for the OnLogin to operate with the Facebook credentials comming from SDK

            registerButton = new Button
            {
                Text = "Create Account",
                TextColor = Constants.TravlendarAquaGreen,
                BackgroundColor = Color.White,
                BorderColor = Constants.TravlendarAquaGreen,
                BorderRadius = 4,
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 155
            };

            loginButton = new Button
            {
                Text = "Login",
                TextColor = Constants.TravlendarAquaGreen,
                BackgroundColor = Color.White,
                BorderColor = Constants.TravlendarAquaGreen,
                BorderRadius = 4,
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 155
            };

            RegisterEvents ();

            //relativeLayout.Children.Add (backgroundImage, Constraint.Constant (0), Constraint.Constant (0));
            //relativeLayout.Children.Add (title, Constraint.RelativeToParent (parent => (parent.Width / 2) - 75), Constraint.RelativeToParent (parent => parent.Height / 2));

            layout.Children.Add (backgroundImage);
            buttons.Children.Add (registerButton);
            buttons.Children.Add (loginButton);

            buttons.Children.Add (fbButton);
            layout.Children.Add (buttons);

            this.Content = layout;
        }

        private async void LoginButton_ClickedAsync (object sender, System.EventArgs e)
        {
            var vm = LoginViewModel.GetInstance ();
            var page = new LoginPage (vm);
            NavigationPage.SetHasNavigationBar (page, false);
            await Navigation.PushAsync (page);
        }

        private void RegisterButton_Clicked (object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException ();
            //Handle manual register
        }

        private void RegisterEvents ()
        {
            fbButton.OnLogin -= _viewModel.LoginWithFacebook;
            fbButton.OnLogin += _viewModel.LoginWithFacebook;

            registerButton.Clicked -= RegisterButton_Clicked;
            registerButton.Clicked += RegisterButton_Clicked;

            loginButton.Clicked -= LoginButton_ClickedAsync;
            loginButton.Clicked += LoginButton_ClickedAsync;
        }
    }
}
