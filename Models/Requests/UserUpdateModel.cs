using System.ComponentModel.DataAnnotations;

namespace StajApi.Models.Requests;

/// <summary>
/// Kullanıcı güncelleme isteğinin request body modelidir.
/// ID ve Email değiştirilemez; bu nedenle yalnızca Name alanını içerir.
/// </summary>
public class UserUpdateModel
{
    /// <summary>
    /// Kullanıcının yeni adıdır.
    /// </summary>
    /// <example>Fatih Ulus</example>
    [Required(ErrorMessage = "Name alanı zorunludur.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name 2 ile 100 karakter arasında olmalıdır.")]
    public string Name { get; set; } = string.Empty;
}
