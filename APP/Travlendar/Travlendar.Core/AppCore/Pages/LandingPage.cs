
using Travlendar.Core.AppCore.ViewModels;
using Travlendar.Pages;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class LandingPage : ContentPage
    {
        private RelativeLayout relativeLayout;
        private StackLayout layout;
        private Image backgroundImage;
        private Label title;
        private Button loginButton;
        private Button registerButton;

        //No VM needed for the moment

        public LandingPage ()
        {
            relativeLayout = new RelativeLayout
            {
                VerticalOptions = LayoutOptions.Fill
            };

            layout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.White
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

            registerButton = new Button
            {
                Text = "Create Account",
                TextColor = Constants.TravlendarBlue,
                BackgroundColor = Color.White,
                BorderColor = Constants.TravlendarBlue,
                BorderRadius = 20,
                BorderWidth = 2
            };

            loginButton = new Button
            {
                Text = "Login",
                TextColor = Constants.TravlendarBlue,
                BackgroundColor = Color.White,
                BorderColor = Constants.TravlendarBlue,
                BorderRadius = 20,
                BorderWidth = 2
            };

            RegisterEvents ();

            relativeLayout.Children.Add (backgroundImage, Constraint.Constant (0), Constraint.Constant (0));
            relativeLayout.Children.Add (title, Constraint.RelativeToParent (parent => (parent.Width / 2) - 75), Constraint.RelativeToParent (parent => parent.Height / 2));

            layout.Children.Add (relativeLayout);
            layout.Children.Add (registerButton);
            layout.Children.Add (loginButton);

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
            registerButton.Clicked -= RegisterButton_Clicked;
            registerButton.Clicked += RegisterButton_Clicked;

            loginButton.Clicked -= LoginButton_ClickedAsync;
            loginButton.Clicked += LoginButton_ClickedAsync;
        }
    }
}
