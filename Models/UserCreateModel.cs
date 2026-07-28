namespace StajApi.Models;

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
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Yeni kullanıcının email adresidir.
    /// </summary>
    /// <example>fatih.ulus@pointr.tech</example>
    public string Email { get; set; } = string.Empty;
}
