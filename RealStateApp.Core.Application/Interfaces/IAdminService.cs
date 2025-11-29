using RealStateApp.Core.Application.Dtos.User;

namespace RealStateApp.Core.Application.Interfaces;

public interface IAdminService
{
    Task<List<UserDto>> GetAllAdmins();
    Task<Result> SetStateAsync(string userId, bool state);
    Task<Result> DeleteAsync(string userId);
    Task<Result<UserDto>> Create(UserSaveDto dto, string? origin);
    Task<Result<UserDto>> Edit(UserSaveDto dto, string? origin);
}