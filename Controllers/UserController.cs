using Microsoft.AspNetCore.Mvc;
using StajApi.Models;

namespace StajApi.Controllers;

// [ApiController]: Bu sınıfın bir Web API Controller olduğunu framework'e bildirir.
// Otomatik model doğrulama (Validation) ve HTTP istek yönlendirmelerini sağlar.
[ApiController]

// [Route]: API'nin hangi URL adresi üzerinden erişileceğini belirler.
// "api/[controller]" ifadesi, Controller kelimesi atılarak "api/user" adresini oluşturur.
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    // Henüz veritabanı (Database) bağlamadığımız için verileri bellekte (In-Memory) tutuyoruz.
    // 'static' yapmamızın sebebi: Her HTTP isteğinde controller yeniden örneklendiğinde verilerin kaybolmamasıdır.
    private static List<UserDto> _users = new List<UserDto>
    {
        // Eğitmenin örnek şablonunda belirttiği başlangıç verileri:
        new UserDto { Id = Guid.NewGuid(), Name = "Fatih", Email = "fatih.ulus@pointr.tech" },
        new UserDto { Id = Guid.NewGuid(), Name = "Rüstem", Email = "rustem.akkaya@pointr.tech" }
    };

    /// <summary>
    /// 1. HTTP GET - Tüm Kullanıcıları Listeleme
    /// Adres: GET /api/user
    /// </summary>
    [HttpGet]
    public IActionResult GetAll()
    {
        // Ok(): HTTP 200 Başarılı durum kodu ile birlikte JSON listesini döner.
        return Ok(_users);
    }

    /// <summary>
    /// 2. HTTP GET - Tek Bir Kullanıcıyı Id ile Getirme
    /// Adres: GET /api/user/{id} (Örn: /api/user/d23f...
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        // FirstOrDefault: LINQ sorgusu ile listede verilen Id'ye sahip ilk elemanı arar.
        var user = _users.FirstOrDefault(u => u.Id == id);

        // Kullanıcı bulunamazsa HTTP 404 Not Found yanıtı dönülür.
        if (user == null)
            return NotFound("Aranan kullanıcı bulunamadı.");

        // Bulunursa HTTP 200 OK ve kullanıcı verisi dönülür.
        return Ok(user);
    }

    /// <summary>
    /// 3. HTTP POST - Yeni Kullanıcı Oluşturma
    /// Adres: POST /api/user
    /// </summary>
    [HttpPost]
    public IActionResult Create([FromBody] UserCreateModel model)
    {
        // [FromBody]: Gelen verinin HTTP Request Body (JSON) içinden okunacağını belirtir.
        
        // Yeni bir DTO oluşturup benzersiz GUID kimliğini burada atıyoruz.
        var newUser = new UserDto
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Email = model.Email
        };

        // Bellekteki listemize ekliyoruz.
        _users.Add(newUser);

        // Eklenen yeni kullanıcıyı geri dönüyoruz.
        return Ok(newUser);
    }

    /// <summary>
    /// 4. HTTP PUT - Kullanıcı Adı Güncelleme
    /// Adres: PUT /api/user/{id}
    /// KURAL: Id ve Email DEĞİŞTİRİLEMEZ. Sadece Name değiştirilebilir.
    /// </summary>
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] UserUpdateModel model)
    {
        // Güncellenecek kullanıcıyı listede buluyoruz.
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user == null)
            return NotFound("Güncellenmek istenen kullanıcı bulunamadı.");

        // Kurala uygun şekilde SADECE Name alanını güncelliyoruz. Id ve Email dokunulmadan kalıyor.
        user.Name = model.Name;

        // Eğitmenin istediği özel yanıt formatı: { "User \"Id\" updated" }
        return Ok($"User \"{id}\" updated");
    }

    /// <summary>
    /// 5. HTTP DELETE - Kullanıcı Silme
    /// Adres: DELETE /api/user/{id}
    /// </summary>
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        // Silinecek kullanıcıyı buluyoruz.
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user == null)
            return NotFound("Silinmek istenen kullanıcı bulunamadı.");

        // Kullanıcıyı listeden siliyoruz.
        _users.Remove(user);

        return Ok($"User \"{id}\" deleted");
    }
}