using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StajApi.Data;
using StajApi.Exceptions;
using StajApi.Models.Configuration;
using StajApi.Models.Entities;
using StajApi.Models.Requests;
using StajApi.Models.Responses;
using StajApi.Services.Interfaces;

namespace StajApi.Services;

/// <summary>
/// Kullanıcı girişini, şifre doğrulamasını ve JWT üretimini gerçekleştirir.
/// </summary>
public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        AppDbContext context,
        IPasswordHasher<User> passwordHasher,
        IOptions<JwtSettings> jwtOptions
    )
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        string normalizedEmail = request.Email.Trim().ToLowerInvariant();

        User? user = await _context.Users
            .FirstOrDefaultAsync(user => user.Email == normalizedEmail);

        if (user is null)
        {
            throw new UnauthorizedException("Email veya şifre hatalı.");
        }

        PasswordVerificationResult verificationResult =
            _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
            );

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException("Email veya şifre hatalı.");
        }

        if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            await _context.SaveChangesAsync();
        }

        return CreateToken(user);
    }

    public TokenValidationResponse GetTokenInfo(ClaimsPrincipal principal)
    {
        List<TokenClaimDto> claims = principal.Claims
            .Select(claim => new TokenClaimDto
            {
                Type = claim.Type,
                Value = claim.Value
            })
            .ToList();

        return new TokenValidationResponse
        {
            IsValid = principal.Identity?.IsAuthenticated == true,
            UserId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? principal.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? string.Empty,
            Name = principal.FindFirstValue(JwtRegisteredClaimNames.Name)
                ?? principal.Identity?.Name
                ?? string.Empty,
            Email = principal.FindFirstValue(JwtRegisteredClaimNames.Email)
                ?? principal.FindFirstValue(ClaimTypes.Email)
                ?? string.Empty,
            UserType = principal.FindFirstValue("type")
                ?? principal.FindFirstValue("role")
                ?? string.Empty,
            Claims = claims
        };
    }

    private LoginResponse CreateToken(User user)
    {
        DateTime expiresAtUtc = DateTime.UtcNow
            .AddMinutes(_jwtSettings.ExpirationMinutes);

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Name, user.Name),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("type", user.UserType),
            new("role", user.UserType)
        ];

        SymmetricSecurityKey securityKey = new(
            Encoding.UTF8.GetBytes(_jwtSettings.Key)
        );

        SigningCredentials credentials = new(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        JwtSecurityToken token = new(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAtUtc,
            signingCredentials: credentials
        );

        return new LoginResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAtUtc = expiresAtUtc
        };
    }
}
