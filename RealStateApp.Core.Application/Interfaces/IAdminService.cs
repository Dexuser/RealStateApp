using RealStateApp.Core.Application.Dtos.User;

namespace RealStateApp.Core.Application.Interfaces;

public interface IAdminService
{
    Task<List<UserDto>> GetAllAdmins();
    Task<Result> SetStatusOnAdminAsync(string userId, bool state);
    Task<Result> DeleteAdminAsync(string userId);
    Task<Result<UserDto>> CreateAdmin(UserSaveDto dto, string? origin);
    Task<Result<UserDto>> EditAdmin(UserSaveDto dto, string? origin);
}