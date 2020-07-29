using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IGPS.Models;

namespace IGPS.Services.Auth
{
    public interface IAuthService
    {
        bool IsAuthenticated { get; }
        UserInfo AuthenticatedUser { get; }

        Task<bool> LoginAsync(string email, string password);
        bool LoginWithSNS(SNSProvider provider);
        Task<bool> UserIsAuthenticatedAndValidAsync();
        Task LogoutAsync();
    }
}
