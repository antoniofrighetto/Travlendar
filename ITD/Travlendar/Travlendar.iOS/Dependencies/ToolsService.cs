using System;
using Travlendar.iOS.Dependencies;
using Travlendar.Dependencies;
using Xamarin.Forms;
using Facebook.LoginKit;

[assembly: Dependency(typeof(ToolsService))]
namespace Travlendar.iOS.Dependencies
{
    public class ToolsService : ITools
    {
        public void LogoutFromFacebook()
        {
            var fbSession = new LoginManager();
            fbSession.LogOut();
        }
    }
}