using StajApi.Models;

namespace StajApi.Services;

/// <summary>
/// Service katmanımızın sözleşmesidir (Interface).
/// Controller katmanı doğrudan veritabanı işlemlerini bilmez, 
/// sadece bu interface üzerinden haberleşir.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Veritabanındaki tüm kullanıcıları getirir.
    /// </summary>
    Task<List<User>> GetAllUsersAsync();

    /// <summary>
    /// Belirtilen ID'ye sahip tek bir kullanıcıyı getirir.
    /// </summary>
    Task<User?> GetUserByIdAsync(int id);

    /// <summary>
    /// Yeni bir kullanıcı oluşturur ve veritabanına kaydeder.
    /// </summary>
    Task<User> CreateUserAsync(User user);

    /// <summary>
    /// Var olan bir kullanıcının bilgilerini günceller.
    /// </summary>
    Task<bool> UpdateUserAsync(int id, User user);

    /// <summary>
    /// Belirtilen ID'ye sahip kullanıcıyı veritabanından siler.
    /// </summary>
    Task<bool> DeleteUserAsync(int id);
}
