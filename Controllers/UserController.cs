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
    /// Belirtilen Guid ID değerine sahip kullanıcıyı getirir.
    /// </summary>
    /// <param name="id">Getirilecek kullanıcının Guid ID değeridir.</param>
    /// <returns>Bulunan kullanıcıyı döndürür.</returns>
    /// <response code="200">Kullanıcı başarıyla bulundu.</response>
    /// <response code="404">Kullanıcı bulunamadı.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        return Ok(await _userService.GetByIdAsync(id));
    }

    /// <summary>
    /// Name ve Email bilgileriyle yeni kullanıcı oluşturur.
    /// </summary>
    /// <param name="model">Request body içerisinden alınan kullanıcı bilgileridir.</param>
    /// <returns>Oluşturulan kullanıcıyı döndürür.</returns>
    /// <response code="201">Kullanıcı başarıyla oluşturuldu.</response>
    /// <response code="409">Email adresi başka bir kullanıcı tarafından kullanılıyor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserDto>> Create([FromBody] UserCreateModel model)
    {
        UserDto newUser = await _userService.CreateAsync(model);

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
