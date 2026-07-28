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
}
