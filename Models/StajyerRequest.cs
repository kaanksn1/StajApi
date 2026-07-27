namespace StajApi.Models;

/// <summary>
/// Stajyer oluşturma ve güncelleme isteklerinde gönderilecek bilgileri temsil eder.
/// </summary>
public class StajyerRequest
{
    /// <summary>
    /// Stajyerin adıdır.
    /// </summary>
    /// <example>Kaan</example>
    public string Ad { get; set; } = string.Empty;

    /// <summary>
    /// Stajyerin soyadıdır.
    /// </summary>
    /// <example>Kesen</example>
    public string Soyad { get; set; } = string.Empty;

    /// <summary>
    /// Stajyerin çalışacağı departmandır.
    /// </summary>
    /// <example>Yazılım</example>
    public string Departman { get; set; } = string.Empty;

    /// <summary>
    /// Stajyerin işe başlangıç tarihidir.
    /// </summary>
    /// <example>2026-07-21</example>
    public DateOnly BaslangicTarihi { get; set; }

    /// <summary>
    /// Stajyerin aktif olup olmadığını belirtir.
    /// </summary>
    /// <example>true</example>
    public bool AktifMi { get; set; }
}
