namespace StajApi.Models;

/// <summary>
/// Kullanıcı kayıt/oluşturma (POST) isteği atılırken istemciden (Swagger/Postman)
/// bize gelecek olan veri kalıbıdır.
/// ID istemci tarafından verilmez, sistem tarafından otomatik üretilir.
/// </summary>
public class UserCreateModel
{
    // Yeni kullanıcının adı ve soyadı
    public string Name { get; set; } = string.Empty;

    // Yeni kullanıcının e-posta adresi
    public string Email { get; set; } = string.Empty;
}