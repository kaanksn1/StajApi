using System.ComponentModel.DataAnnotations;

namespace StajApi.Models.Requests;

/// <summary>
/// Kullanıcı listesinde sayfalama ve arama için kullanılan query parametreleridir.
/// </summary>
public class UserListQuery
{
    /// <summary>
    /// Getirilecek sayfa numarasıdır.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page değeri en az 1 olmalıdır.")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// Bir sayfada bulunabilecek en fazla kullanıcı sayısıdır.
    /// </summary>
    [Range(1, 100, ErrorMessage = "PageSize değeri 1 ile 100 arasında olmalıdır.")]
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Kullanıcı adı veya email adresinde aranacak isteğe bağlı metindir.
    /// </summary>
    [MaxLength(100, ErrorMessage = "Search değeri en fazla 100 karakter olabilir.")]
    public string? Search { get; set; }
}
