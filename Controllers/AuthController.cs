using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajApi.Models;
using StajApi.Models.Requests;
using StajApi.Models.Responses;
using StajApi.Services.Interfaces;

namespace StajApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Email ve şifre doğruysa JWT token üretir.
    /// </summary>
    /// <response code="200">JWT başarıyla üretildi.</response>
    /// <response code="400">Request validation başarısız.</response>
    /// <response code="401">Email veya şifre hatalı.</response>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest request
    )
    {
        LoginResponse response = await _authService.LoginAsync(request);
        return Ok(response);
    }

    /// <summary>
    /// Authorization header ile gelen JWT'yi doğrular ve geçerliyse claimleri döndürür.
    /// </summary>
    /// <response code="200">Token geçerli; kullanıcı ve claim bilgileri döndürüldü.</response>
    /// <response code="401">Token eksik, süresi geçmiş veya geçersiz.</response>
    [Authorize]
    [HttpGet("validate")]
    [ProducesResponseType(typeof(TokenValidationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<TokenValidationResponse> ValidateToken()
    {
        return Ok(_authService.GetTokenInfo(User));
    }
}
