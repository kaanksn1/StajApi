namespace StajApi.Models;

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
    public string Name { get; set; } = string.Empty;
}
