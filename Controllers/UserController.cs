using Microsoft.AspNetCore.Mvc;
using StajApi.Models;
using StajApi.Models.Requests;
using StajApi.Models.Responses;
using StajApi.Services.Interfaces;

namespace StajApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Tüm kullanıcıları listeler.
    /// </summary>
    /// <returns>Kullanıcı listesini döndürür.</returns>
    /// <response code="200">Kullanıcı listesi başarıyla getirildi.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        return Ok(await _userService.GetAllAsync());
    }

    /// <summary>
    /// Belirtilen ID değerine sahip kullanıcıyı getirir. Karakter sayısı ve GUID formatı kontrolü yapar.
    /// </summary>
    /// <param name="id">Getirilecek kullanıcının ID değeridir.</param>
    /// <returns>Bulunan kullanıcıyı döndürür.</returns>
    /// <response code="200">Kullanıcı başarıyla bulundu.</response>
    /// <response code="400">ID formatı geçersiz (eksik veya fazladan karakter içeriyor).</response>
    /// <response code="404">Kullanıcı bulunamadı.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        // 1. KONTROL: Karakter dizisi geçerli bir GUID formatına uyuyor mu?
        if (!Guid.TryParse(id, out Guid parsedGuid))
        {
            return BadRequest(new ErrorResponse 
            { 
                Message = $"Girdiğiniz ID değeri ('{id}') geçerli bir GUID formatında değildir. Eksik veya fazladan karakter girmiş olabilirsiniz (Girilen Karakter Sayısı: {id?.Length ?? 0})." 
            });
        }

        // 2. KONTROL: Service tarafındaki olası Hata (Exception) durumlarına karşı try-catch
        try
        {
            var user = await _userService.GetByIdAsync(parsedGuid);

            if (user == null)
            {
                return NotFound(new ErrorResponse 
                { 
                    Message = $"Girdiğiniz ID ('{parsedGuid}') geçerli bir formatadır ancak bu ID ile eşleşen bir kullanıcı veritabanında bulunamadı." 
                });
            }

            return Ok(user);
        }
        catch (Exception)
        {
            // Eğer Service katmanı kullanıcıyı bulamadığında 500 hatası (Exception) fırlatıyorsa yakalayıp 404 dönüyoruz:
            return NotFound(new ErrorResponse 
            { 
                Message = $"Girdiğiniz ID ('{parsedGuid}') geçerli bir formatadır ancak bu ID ile eşleşen bir kullanıcı veritabanında bulunamadı." 
            });
        }
    }

    /// <summary>
    /// Name, Email ve Password bilgileriyle yeni kullanıcı oluşturur.
    /// </summary>
    /// <param name="model">Request body içerisinden alınan kullanıcı bilgileridir.</param>
    /// <returns>Oluşturulan kullanıcıyı döndürür.</returns>
    /// <response code="201">Kullanıcı başarıyla oluşturuldu.</response>
    /// <response code="400">Password alanı boş veya bilgiler geçersiz.</response>
    /// <response code="409">Email adresi başka bir kullanıcı tarafından kullanılıyor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create([FromBody] UserCreateModel model)
    {
        // Password alanı boş gönderildiyse servise gitmeden 400 Bad Request fırlatıyoruz
        if (model == null || string.IsNullOrWhiteSpace(model.Password))
        {
            return BadRequest(new ErrorResponse
            {
                Message = "Password alanı zorunludur ve boş bırakılamaz."
            });
        }

        try
        {
            UserDto newUser = await _userService.CreateAsync(model);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newUser.Id },
                newUser
            );
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                Message = $"Kullanıcı oluşturulurken bir hata oluştu: {ex.Message}"
            });
        }
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
    public async Task<ActionResult<UserActionResponse>> Update(
        Guid id,
        [FromBody] UserUpdateModel model
    )
    {
        return Ok(await _userService.UpdateAsync(id, model));
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
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}