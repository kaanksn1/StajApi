namespace StajApi.Models.Responses;

/// <summary>
/// Kullanıcı işleminin sonucunu içeren mesaj cevabını temsil eder.
/// </summary>
public class UserActionResponse
{
    /// <summary>
    /// Yapılan işlemin sonucunu açıklayan mesajdır.
    /// </summary>
    /// <example>User "11111111-1111-1111-1111-111111111111" updated</example>
    public string Message { get; set; } = string.Empty;
}
