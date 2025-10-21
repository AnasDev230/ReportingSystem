using Microsoft.AspNetCore.Identity;

namespace ReportingSystem.Repositories.Interface
{
    public interface ITokenRepository
    {
        Task<string> CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
