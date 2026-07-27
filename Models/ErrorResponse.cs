namespace StajApi.Models;

/// <summary>
/// API hata cevaplarını temsil eder.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Hatanın nedenini açıklayan mesajdır.
    /// </summary>
    /// <example>ID'si 99 olan stajyer bulunamadı.</example>
    public string Message { get; set; } = string.Empty; // başlangıçta boş metin vererek null olmasını önler
}
