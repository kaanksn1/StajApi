using Microsoft.EntityFrameworkCore;
using StajApi.Data;
using StajApi.Models;

namespace StajApi.Services;

/// <summary>
/// Tüm veritabanı işlemlerinin (Ekleme, Listeleme, Güncelleme, Silme)
/// ve iş mantığının yürütüldüğü Service sınıfıdır.
/// </summary>
public class UserService : IUserService
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Dependency Injection: AppDbContext dışarıdan buraya enjekte edilir.
    /// Böylece veritabanı ile konuşabiliriz.
    /// </summary>
    public UserService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Veritabanındaki tüm kullanıcıları liste olarak döner.
    /// </summary>
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    /// <summary>
    /// ID'ye göre veritabanında arama yapar, bulursa kullanıcıyı döner.
    /// </summary>
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    /// <summary>
    /// Yeni kullanıcıyı veritabanına ekler ve otomatik üretilen ID ile birlikte geri döner.
    /// </summary>
    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(); // Değişiklikler veritabanına kaydedilir.
        return user;
    }

    /// <summary>
    /// Belirtilen ID'ye sahip kullanıcıyı bulur ve bilgilerini günceller.
    /// </summary>
    public async Task<bool> UpdateUserAsync(int id, User updatedUser)
    {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
            return false; // Kullanıcı yoksa güncelleme başarısız.

        existingUser.Name = updatedUser.Name;
        existingUser.Email = updatedUser.Email;

        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Belirtilen ID'ye sahip kullanıcıyı veritabanından siler.
    /// </summary>
    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false; // Kullanıcı bulunamadıysa silinemez.

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}