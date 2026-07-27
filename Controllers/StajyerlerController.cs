using Microsoft.AspNetCore.Mvc;
using StajApi.Models;

namespace StajApi.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    [HttpGet]
    public ActionResult<List<Stajyer>> TumunuGetir()
    {
        return Ok(Stajyerler);
    }
    [HttpGet("{id}")]
    public ActionResult<Stajyer> IdIleGetir(int id)
    {
        Stajyer? stajyer = Stajyerler.FirstOrDefault(x => x.Id == id);

        if (stajyer is null)
        {
            return NotFound(new
            {
                message = $"ID'si {id} olan stajyer bulunamadı."
            });
        }

        return Ok(stajyer);
    }
    [HttpPost]
    public ActionResult<Stajyer> StajyerEkle(Stajyer stajyer)
    {
        stajyer.Id = Stajyerler.Count == 0
            ? 1
            : Stajyerler.Max(x => x.Id) + 1;

        Stajyerler.Add(stajyer);

        return CreatedAtAction(
            nameof(IdIleGetir),
            new { id = stajyer.Id },
            stajyer
        );
    }
    [HttpPut("{id}")]
    public ActionResult<Stajyer> StajyerGuncelle(
    int id,
    Stajyer guncelStajyer
)
    {
        int index = Stajyerler.FindIndex(x => x.Id == id);

        if (index == -1)
        {
            return NotFound(new
            {
                message = $"ID'si {id} olan stajyer güncellenemedi, kayıt bulunamadı."
            });
        }

        guncelStajyer.Id = id;
        Stajyerler[index] = guncelStajyer;

        return Ok(guncelStajyer);
    }
    [HttpDelete("{id}")]
    public IActionResult StajyerSil(int id)
    {
        Stajyer? stajyer = Stajyerler.FirstOrDefault(x => x.Id == id);

        if (stajyer is null)
        {
            return NotFound(new
            {
                message = $"ID'si {id} olan stajyer silinemedi."
            });

        }

        Stajyerler.Remove(stajyer);

        return NoContent();
    }
}