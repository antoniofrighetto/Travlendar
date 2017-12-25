using System;
using System.Threading.Tasks;

namespace Travlendar.Framework.Dependencies
{
    interface IParse
    {
        Task<bool> LoginWithFacebook (string userId, string accessToken, DateTime tokenExpiration);
    }
}
