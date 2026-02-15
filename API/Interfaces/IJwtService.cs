using System.Security.Claims;

namespace API.Interfaces
{
    /// <summary>
    /// Interface for JWT token generation and validation
    /// </summary>
    public interface IJwtService
    {

        string GenerateAccessToken(Guid userId, string username, string role);
            string GenerateRefreshToken();
        ClaimsPrincipal? ValidateToken(string token);
        int GetAccessTokenExpirationSeconds();
    }
}
