namespace StajApi.Models.Responses;

/// <summary>
/// JWT geçerliyse token içerisindeki kullanıcı ve claim bilgilerini döndürür.
/// </summary>
public class TokenValidationResponse
{
    public bool IsValid { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string UserType { get; set; } = string.Empty;

    public List<TokenClaimDto> Claims { get; set; } = [];
}
