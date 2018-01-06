using Travlendar.Core.AppCore.Helpers;

namespace Travlendar.Droid.Dependencies
{
    public class AppExistance : IExistance
    {
        public bool ApplicationExistance (string app)
        {
            //Actually on the majority of the Android device there is Google Maps installed
            return true;
        }
    }
}
