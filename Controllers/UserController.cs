using Microsoft.AspNetCore.Mvc;
using StajApi.Models;

namespace StajApi.Controllers;

[ApiController] // Bu sınıfın bir Web API controller'ı olduğunu belirtir.
[Route("api/[controller]")] // Controller adından /api/User adresini oluşturur.
[Produces("application/json")] // Endpointlerin JSON cevap ürettiğini Swagger'a bildirir.
public class UserController : ControllerBase
{
    // Veritabanı olmadığı için kullanıcılar uygulama belleğinde tutulur.
    private static readonly List<UserDto> Users =
    [
        new UserDto
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Fatih",
            Email = "fatih.ulus@pointr.tech"
        },
        new UserDto
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "Rüstem",
            Email = "rustem.akkaya@pointr.tech"
        }
    ];

    /// <summary>
    /// Tüm kullanıcıları listeler.
    /// </summary>
    /// <returns>Kullanıcı listesini döndürür.</returns>
    /// <response code="200">Kullanıcı listesi başarıyla getirildi.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    public ActionResult<List<UserDto>> GetAll()
    {
        return Ok(Users);
    }

    /// <summary>
    /// Belirtilen Guid ID değerine sahip kullanıcıyı getirir.
    /// </summary>
    /// <param name="id">Getirilecek kullanıcının Guid ID değeridir.</param>
    /// <returns>Bulunan kullanıcıyı döndürür.</returns>
    /// <response code="200">Kullanıcı başarıyla bulundu.</response>
    /// <response code="404">Kullanıcı bulunamadı.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public ActionResult<UserDto> GetById(Guid id)
    {
        UserDto? user = Users.FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return NotFound(new ErrorResponse
            {
                Message = "Aranan kullanıcı bulunamadı."
            });
        }

        return Ok(user);
    }

    /// <summary>
    /// Name ve Email bilgileriyle yeni kullanıcı oluşturur.
    /// </summary>
    /// <param name="model">Request body içerisinden alınan kullanıcı bilgileridir.</param>
    /// <returns>Oluşturulan kullanıcıyı döndürür.</returns>
    /// <response code="201">Kullanıcı başarıyla oluşturuldu.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    public ActionResult<UserDto> Create([FromBody] UserCreateModel model)
    {
        UserDto newUser = new UserDto
        {
            Id = Guid.NewGuid(), // ID istemciden alınmaz, API tarafından üretilir.
            Name = model.Name,
            Email = model.Email
        };

        Users.Add(newUser);

        return CreatedAtAction(
            nameof(GetById),
            new { id = newUser.Id },
            newUser
        );
    }

    /// <summary>
    /// Kullanıcının yalnızca Name alanını günceller.
    /// </summary>
    /// <param name="id">Güncellenecek kullanıcının Guid ID değeridir.</param>
    /// <param name="model">Yalnızca yeni Name bilgisini içeren request body'dir.</param>
    /// <returns>Güncelleme sonucunu içeren mesajı döndürür.</returns>
    /// <response code="200">Kullanıcı başarıyla güncellendi.</response>
    /// <response code="404">Kullanıcı bulunamadı.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserActionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public ActionResult<UserActionResponse> Update(Guid id, [FromBody] UserUpdateModel model)
    {
        UserDto? user = Users.FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return NotFound(new ErrorResponse
            {
                Message = "Güncellenmek istenen kullanıcı bulunamadı."
            });
        }

        // UserUpdateModel yalnız Name içerdiği için Id ve Email değiştirilemez.
        user.Name = model.Name;

        return Ok(new UserActionResponse
        {
            Message = $"User \"{id}\" updated"
        });
    }

    /// <summary>
    /// Belirtilen Guid ID değerine sahip kullanıcıyı siler.
    /// </summary>
    /// <param name="id">Silinecek kullanıcının Guid ID değeridir.</param>
    /// <response code="204">Kullanıcı başarıyla silindi.</response>
    /// <response code="404">Kullanıcı bulunamadı.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        UserDto? user = Users.FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return NotFound(new ErrorResponse
            {
                Message = "Silinmek istenen kullanıcı bulunamadı."
            });
        }

        Users.Remove(user);

        return NoContent();
    }
}
