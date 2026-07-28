using System.ComponentModel.DataAnnotations;

namespace StajApi.Models.Requests;

/// <summary>
/// Yeni kullanıcı oluşturma isteğinin request body modelidir.
/// ID istemciden alınmaz; API tarafından otomatik üretilir.
/// </summary>
public class UserCreateModel
{
    /// <summary>
    /// Yeni kullanıcının adıdır.
    /// </summary>
    /// <example>Fatih</example>
    [Required(ErrorMessage = "Name alanı zorunludur.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name 2 ile 100 karakter arasında olmalıdır.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Yeni kullanıcının email adresidir.
    /// </summary>
    /// <example>fatih.ulus@pointr.tech</example>
    [Required(ErrorMessage = "Email alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi girilmelidir.")]
    [StringLength(254, ErrorMessage = "Email en fazla 254 karakter olabilir.")]
    public string Email { get; set; } = string.Empty;
}
