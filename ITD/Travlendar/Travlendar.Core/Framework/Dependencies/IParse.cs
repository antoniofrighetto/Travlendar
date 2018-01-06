using System;
using System.Threading.Tasks;

namespace Travlendar.Framework.Dependencies
{
    /// <summary>
    /// Interface for passing the user's login data
    /// </summary>
    interface IParse
    {
        Task<bool> LoginWithFacebook (string userId, string accessToken, DateTime tokenExpiration);
    }
}
