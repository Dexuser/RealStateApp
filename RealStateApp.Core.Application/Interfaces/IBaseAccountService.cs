using ArtemisBanking.Core.Application.Dtos.Login;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Application.Interfaces;

public interface IBaseAccountService
{
    Task<Result<UserDto>> RegisterUser(UserSaveDto saveDto, string? origin, bool? isApi = false);
    Task<Result<UserDto>> EditUser(UserSaveDto saveDto, string? origin, bool? isApi = false);
    Task<Result> ForgotPasswordAsync(ForgotPasswordRequestDto request, bool? isApi = false);
    Task<Result> ResetPasswordAsync(ResetPasswordRequestDto request);
    Task<Result> DeleteAsync(string id);
    Task<UserDto?> GetUserByEmail(string email);
    Task<UserDto?> GetUserById(string id);
    Task<List<UserDto>> GetUsersByIds(List<string> ids);
    Task<UserDto?> GetUserByUserName(string userName);
    Task<UserDto?> GetByIdentityCardNumber(string identityCardNumber);
    Task<List<UserDto>> GetAllUser(bool? isActive = true);
    Task<List<UserDto>> GetAllUserOfRole(Roles role, bool isActive = true);
    Task<List<string>> GetAllUserIdsOfRole(Roles role, bool isActive = true);
    Task<List<string>> GetAllUsersIds(bool isActive = true);
    Task<int> CountUsers(Roles? role, bool? onlyActive = null);
    Task<Result> SetStateOnUser(string userId, bool state);
    Task<Result> ConfirmAccountAsync(string userId, string token);
}

