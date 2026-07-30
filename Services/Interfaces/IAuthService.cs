using System.Security.Claims;
using StajApi.Models.Requests;
using StajApi.Models.Responses;

namespace StajApi.Services.Interfaces;

/// <summary>
/// Login, JWT üretimi ve doğrulanmış token claimlerini okuma sözleşmesidir.
/// </summary>
public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);

    TokenValidationResponse GetTokenInfo(ClaimsPrincipal principal);
}
