using Xamarin.Facebook.Login;
using Xamarin.Forms;

using Travlendar.Droid.Dependencies;
using Travlendar.Framework.Dependencies;

[assembly: Dependency (typeof (ToolsService))]
namespace Travlendar.Droid.Dependencies
{
    public class ToolsService : ITools
    {
        public void LogoutFromFacebook ()
        {
            LoginManager.Instance.LogOut ();
        }
    }
}
