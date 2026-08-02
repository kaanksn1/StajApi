using StajApi.Models.Requests;
using StajApi.Models.Responses;

namespace StajApi.Services.Interfaces;

/// <summary>
/// Service katmanımızın sözleşmesidir (Interface).
/// Controller katmanı doğrudan veritabanı işlemlerini bilmez, 
/// sadece bu interface üzerinden haberleşir.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Kullanıcıları arama ve sayfalama seçeneklerine göre getirir.
    /// </summary>
    Task<PagedResponse<UserDto>> GetAllAsync(UserListQuery query);

    /// <summary>
    /// Belirtilen ID'ye sahip tek bir kullanıcıyı getirir.
    /// </summary>
    Task<UserDto> GetByIdAsync(string id);

    /// <summary>
    /// Yeni bir kullanıcı oluşturur ve veritabanına kaydeder.
    /// </summary>
    Task<UserDto> CreateAsync(UserCreateModel model);

    /// <summary>
    /// Var olan bir kullanıcının bilgilerini günceller.
    /// </summary>
    Task<UserActionResponse> UpdateAsync(Guid id, UserUpdateModel model);

    /// <summary>
    /// Belirtilen ID'ye sahip kullanıcıyı veritabanından siler.
    /// </summary>
    Task DeleteAsync(Guid id);
}
