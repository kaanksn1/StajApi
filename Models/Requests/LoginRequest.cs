using System.ComponentModel.DataAnnotations;

namespace StajApi.Models.Requests;

/// <summary>
/// Kullanıcının email ve şifreyle giriş yapması için gönderilen request body'dir.
/// </summary>
public class LoginRequest
{
    /// <example>kaan.kesen@pointr.tech</example>
    [Required(ErrorMessage = "Email alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi girilmelidir.")]
    public string Email { get; set; } = string.Empty;

    /// <example>Password123!</example>
    [Required(ErrorMessage = "Password alanı zorunludur.")]
    public string Password { get; set; } = string.Empty;
}
