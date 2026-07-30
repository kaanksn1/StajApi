using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StajApi.Data;
using StajApi.Exceptions;
using StajApi.Models.Entities;
using StajApi.Models.Requests;
using StajApi.Models.Responses;
using StajApi.Services.Interfaces;

namespace StajApi.Services;

/// <summary>
/// Tüm veritabanı işlemlerinin (Ekleme, Listeleme, Güncelleme, Silme)
/// ve iş mantığının yürütüldüğü Service sınıfıdır.
/// </summary>
public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    /// <summary>
    /// Dependency Injection: AppDbContext dışarıdan buraya enjekte edilir.
    /// Böylece veritabanı ile konuşabiliriz.
    /// </summary>
    public UserService(
        AppDbContext context,
        IPasswordHasher<User> passwordHasher
    )
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Veritabanındaki tüm kullanıcıları liste olarak döner.
    /// </summary>
    public async Task<List<UserDto>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            })
            .ToListAsync();
    }

    /// <summary>
    /// ID'ye göre veritabanında arama yapar, bulursa kullanıcıyı döner.
    /// </summary>
    public async Task<UserDto> GetByIdAsync(string id)
    {
        if (!Guid.TryParse(id, out Guid parsedId))
        {
            throw new BadRequestException(
                $"Girdiğiniz ID değeri ('{id}') geçerli bir GUID formatında değildir."
            );
        }

        User? user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == parsedId);

        if (user is null)
        {
            throw new NotFoundException($"ID'si {parsedId} olan kullanıcı bulunamadı.");
        }

        return ToDto(user);
    }

    /// <summary>
    /// Yeni kullanıcıyı veritabanına ekler ve otomatik üretilen ID ile birlikte geri döner.
    /// </summary>
    public async Task<UserDto> CreateAsync(UserCreateModel model)
    {
        string normalizedEmail = model.Email.Trim().ToLowerInvariant();

        bool emailExists = await _context.Users
            .AnyAsync(user => user.Email == normalizedEmail);

        if (emailExists)
        {
            throw new ConflictException($"{normalizedEmail} email adresi zaten kullanılıyor.");
        }

        User user = new User
        {
            Id = Guid.NewGuid(),
            Name = model.Name.Trim(),
            Email = normalizedEmail,
            UserType = "User"
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

        _context.Users.Add(user);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new ConflictException($"{normalizedEmail} email adresi zaten kullanılıyor.");
        }

        return ToDto(user);
    }

    /// <summary>
    /// Belirtilen ID'ye sahip kullanıcıyı bulur ve bilgilerini günceller.
    /// </summary>
    public async Task<UserActionResponse> UpdateAsync(Guid id, UserUpdateModel model)
    {
        User? user = await _context.Users
            .FirstOrDefaultAsync(user => user.Id == id);

        if (user is null)
        {
            throw new NotFoundException($"ID'si {id} olan kullanıcı güncellenemedi, kayıt bulunamadı.");
        }

        user.Name = model.Name.Trim();

        await _context.SaveChangesAsync();

        return new UserActionResponse
        {
            Message = $"User \"{id}\" updated"
        };
    }

    /// <summary>
    /// Belirtilen ID'ye sahip kullanıcıyı veritabanından siler.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        User? user = await _context.Users
            .FirstOrDefaultAsync(user => user.Id == id);

        if (user is null)
        {
            throw new NotFoundException($"ID'si {id} olan kullanıcı silinemedi, kayıt bulunamadı.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    private static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}
