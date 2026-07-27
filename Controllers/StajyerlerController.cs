using Microsoft.AspNetCore.Mvc;
using StajApi.Models;

namespace StajApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")] //endpointlerin json cevap urettigini swaggera bildirir, text plain yerine application/json gorunmesini saglar
public class StajyerlerController : ControllerBase
{
    private static readonly List<Stajyer> Stajyerler =
    [
        new Stajyer
        {
            Id = 1,
            Ad = "Kaan",
            Soyad = "Kesen",
            Departman = "Yazılım",
            BaslangicTarihi = new DateOnly(2026, 7, 23),
            AktifMi = true
        }
    ];

    /// <summary>
    /// Tüm stajyer kayıtlarını getirir.
    /// </summary>
    /// <returns>Stajyer kayıtlarının listesini döndürür.</returns>
    /// <response code="200">Stajyer listesi başarıyla getirildi.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<Stajyer>), StatusCodes.Status200OK)]
    public ActionResult<List<Stajyer>> TumunuGetir()
    {
        return Ok(Stajyerler);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip stajyer kaydını getirir.
    /// </summary>
    /// <param name="id">Getirilecek stajyerin ID değeridir.</param>
    /// <returns>Bulunan stajyer kaydını döndürür.</returns>
    /// <response code="200">Stajyer başarıyla bulundu.</response>
    /// <response code="404">Belirtilen ID'ye sahip stajyer bulunamadı.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Stajyer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public ActionResult<Stajyer> IdIleGetir(int id)
    {
        Stajyer? stajyer = Stajyerler.FirstOrDefault(x => x.Id == id);

        if (stajyer is null)
        {
            return NotFound(new ErrorResponse
            {
                Message = $"ID'si {id} olan stajyer bulunamadı."
            });
        }

        return Ok(stajyer);
    }

    /// <summary>
    /// Yeni bir stajyer kaydı oluşturur.
    /// </summary>
    /// <param name="istek">Request body içerisinden alınan yeni stajyer bilgileridir.</param>
    /// <returns>Oluşturulan stajyer kaydını döndürür.</returns>
    /// <response code="201">Stajyer başarıyla oluşturuldu.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Stajyer), StatusCodes.Status201Created)]
    public ActionResult<Stajyer> StajyerEkle(StajyerRequest istek)
    {
        int yeniId = Stajyerler.Count == 0
            ? 1
            : Stajyerler.Max(x => x.Id) + 1;

        Stajyer stajyer = new Stajyer
        {
            Id = yeniId,
            Ad = istek.Ad,
            Soyad = istek.Soyad,
            Departman = istek.Departman,
            BaslangicTarihi = istek.BaslangicTarihi,
            AktifMi = istek.AktifMi
        };

        Stajyerler.Add(stajyer);

        return CreatedAtAction(
            nameof(IdIleGetir),
            new { id = stajyer.Id },
            stajyer
        );
    }

    /// <summary>
    /// Belirtilen ID'ye sahip stajyer kaydını günceller.
    /// </summary>
    /// <param name="id">Güncellenecek stajyerin URL'den alınan ID değeridir.</param>
    /// <param name="istek">Request body içerisinden alınan güncel stajyer bilgileridir.</param>
    /// <returns>Güncellenmiş stajyer kaydını döndürür.</returns>
    /// <response code="200">Stajyer başarıyla güncellendi.</response>
    /// <response code="404">Güncellenecek stajyer bulunamadı.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Stajyer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public ActionResult<Stajyer> StajyerGuncelle(
        int id,
        StajyerRequest istek
    )
    {
        int index = Stajyerler.FindIndex(x => x.Id == id);

        if (index == -1)
        {
            return NotFound(new ErrorResponse
            {
                Message = $"ID'si {id} olan stajyer güncellenemedi, kayıt bulunamadı."
            });
        }

        Stajyer guncellenmisStajyer = new Stajyer
        {
            Id = id,
            Ad = istek.Ad,
            Soyad = istek.Soyad,
            Departman = istek.Departman,
            BaslangicTarihi = istek.BaslangicTarihi,
            AktifMi = istek.AktifMi
        };

        Stajyerler[index] = guncellenmisStajyer;

        return Ok(guncellenmisStajyer);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip stajyer kaydını siler.
    /// </summary>
    /// <param name="id">Silinecek stajyerin ID değeridir.</param>
    /// <returns>Silme işlemi başarılı olduğunda boş cevap döndürür.</returns>
    /// <response code="204">Stajyer başarıyla silindi.</response>
    /// <response code="404">Silinecek stajyer bulunamadı.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public IActionResult StajyerSil(int id)
    {
        Stajyer? stajyer = Stajyerler.FirstOrDefault(x => x.Id == id);

        if (stajyer is null)
        {
            return NotFound(new ErrorResponse
            {
                Message = $"ID'si {id} olan stajyer silinemedi, kayıt bulunamadı."
            });
        }

        Stajyerler.Remove(stajyer);

        return NoContent();
    }
}
