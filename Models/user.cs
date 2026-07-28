namespace StajApi.Models;

/// <summary>
/// Veritabanındaki 'Users' tablosuna denk gelen Entity sınıfımızdır.
/// </summary>
public class User
{
    /// <summary>
    /// EF Core "Id" ve "int" ikilisini gördüğü an bunu Otomatik Artan (Identity/Serial) yapar.
    /// DB kendisi 1, 2, 3... diye sırayla değer atar.
    /// </summary>
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}