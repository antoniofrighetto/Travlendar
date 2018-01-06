using Facebook.LoginKit;
using Travlendar.Framework.Dependencies;
using Travlendar.iOS.Dependencies;
using Xamarin.Forms;

[assembly: Dependency (typeof (ToolsService))]
namespace Travlendar.iOS.Dependencies
{
    public class ToolsService : ITools
    {
        /// <summary>
        /// Implementing the native logout from Facebook.
        /// </summary>
        public void LogoutFromFacebook ()
        {
            var fbSession = new LoginManager ();
            fbSession.LogOut ();
        }
    }
}
