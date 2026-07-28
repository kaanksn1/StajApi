namespace StajApi.Models.Responses;

/// <summary>
/// API'nin istemciye döndürdüğü kullanıcı verisini temsil eder.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Kullanıcının benzersiz Guid kimlik değeridir.
    /// </summary>
    /// <example>11111111-1111-1111-1111-111111111111</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Kullanıcının adıdır.
    /// </summary>
    /// <example>Fatih</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcının email adresidir.
    /// </summary>
    /// <example>fatih.ulus@pointr.tech</example>
    public string Email { get; set; } = string.Empty;
}
