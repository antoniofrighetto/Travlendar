using System;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Renderers
{
    //Custom Facebook Event
    public class FacebookEventArgs : EventArgs
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public DateTime TokenExpiration { get; set; }
    }

    //Custom Facebook Button
    public class FacebookButton : Button
    {
        public Action<object, FacebookEventArgs> OnLogin;

        public void Login (object sender, FacebookEventArgs args)
        {
            if ( OnLogin != null )
                OnLogin (sender, args);
        }
    }
}
