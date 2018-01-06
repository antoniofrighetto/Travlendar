using Travlendar.Droid.Dependencies;
using Travlendar.Framework.Dependencies;
using Xamarin.Facebook.Login;
using Xamarin.Forms;

[assembly: Dependency (typeof (ToolsService))]
namespace Travlendar.Droid.Dependencies
{
    public class ToolsService : ITools
    {
        //Platform Implementation for the native logout from Facebook
        public void LogoutFromFacebook ()
        {
            LoginManager.Instance.LogOut ();
        }
    }
}
