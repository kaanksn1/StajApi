namespace StajApi.Models.Responses;

/// <summary>
/// Başarılı login işlemi sonucunda istemciye verilen JWT bilgisidir.
/// </summary>
public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public string TokenType { get; set; } = "Bearer";

    public DateTime ExpiresAtUtc { get; set; }
}
