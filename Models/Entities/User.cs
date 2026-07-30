namespace StajApi.Models.Entities;

/// <summary>
/// Veritabanındaki 'Users' tablosuna denk gelen Entity sınıfımızdır.
/// </summary>
public class User
{
    /// <summary>
    /// Kullanıcının API tarafından üretilen benzersiz Guid kimliğidir.
    /// </summary>
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcının düz şifresi değil, güvenli şekilde üretilmiş hash değeridir.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcının JWT içine de yazılan tip/rol bilgisidir.
    /// </summary>
    public string UserType { get; set; } = "User";
}
