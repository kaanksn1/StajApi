namespace StajApi.Models;

public class Stajyer
{
    public int Id { get; set; }

    public string Ad { get; set; } = string.Empty;

    public string Soyad { get; set; } = string.Empty;

    public string Departman { get; set; } = string.Empty;

    public DateOnly BaslangicTarihi { get; set; }

    public bool AktifMi { get; set; }
}