namespace StajApi.Models.Responses;

/// <summary>
/// Sayfalama uygulanan liste sonuçlarının ortak response modelidir.
/// </summary>
public class PagedResponse<T>
{
    /// <summary>
    /// İstenen sayfadaki kayıtları içerir.
    /// </summary>
    public List<T> Items { get; set; } = [];

    /// <summary>
    /// Şu an getirilen sayfanın numarasıdır.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Bir sayfada bulunabilecek kayıt sayısıdır.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Filtreye uyan toplam kayıt sayısıdır.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Toplam sayfa sayısıdır.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Önceki bir sayfanın bulunup bulunmadığını gösterir.
    /// </summary>
    public bool HasPreviousPage => Page > 1;

    /// <summary>
    /// Sonraki bir sayfanın bulunup bulunmadığını gösterir.
    /// </summary>
    public bool HasNextPage => Page < TotalPages;
}
