using Travlendar.Dependencies.Droid;
using Xamarin.Facebook.Login;
using Xamarin.Forms;

[assembly: Dependency (typeof (ToolsService))]
namespace Travlendar.Dependencies.Droid
{
    public class ToolsService : ITools
    {
        public void LogoutFromFacebook ()
        {
            LoginManager.Instance.LogOut ();
        }
    }
}
