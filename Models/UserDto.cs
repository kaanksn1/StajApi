namespace StajApi.Models;

/// <summary>
/// DTO (Data Transfer Object): İstemciye (ön yüze veya Swagger'a) 
/// geri döneceğimiz kullanıcı verisinin formatıdır.
/// System.Guid tipinde eşsiz bir kimlik (Id) barındırır.
/// </summary>
public class UserDto
{
    // Guid: Benzersiz (unique) 128-bitlik alfanümerik kimlik değeridir.
    public Guid Id { get; set; }

    // Kullanıcının adı
    public string Name { get; set; } = string.Empty;

    // Kullanıcının e-posta adresi
    public string Email { get; set; } = string.Empty;
}