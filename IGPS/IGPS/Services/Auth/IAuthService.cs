using IGPS.Models;

using System.Threading.Tasks;

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
