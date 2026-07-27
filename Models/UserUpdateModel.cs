namespace StajApi.Models;

/// <summary>
/// Kullanıcı güncelleme (PUT) isteğinde kullanılacak modeldir.
/// Eğitmen kuralı: "Id ve Mail değiştirilemez, sadece Name değiştirilebilir."
/// Bu yüzden bu model içinde güvenlik ve kural gereği Id ve Email alanları YER ALMAZ.
/// </summary>
public class UserUpdateModel
{
    // Sadece adı güncelleyebildiğimiz için tek parametre alıyoruz.
    public string Name { get; set; } = string.Empty;
}