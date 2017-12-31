using Travlendar.Core.AppCore.Helpers;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    public class FullscreenImagePage : ContentPage
    {
        public FullscreenImagePage (string img)
        {
            if (Device.RuntimePlatform == Device.Android)
                DependencyService.Get<IStatusBar> ().HideStatusBar ();

            Content = new StackLayout
            {
                Padding = 0,
                Children = {
                    new Image
                    {
                        Source = ImageSource.FromFile(img),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Aspect = Aspect.AspectFit
                    }
                }
            };
        }

        protected override bool OnBackButtonPressed ()
        {
            DependencyService.Get<IStatusBar> ().ShowStatusBar ();
            return true;
        }
    }
}
