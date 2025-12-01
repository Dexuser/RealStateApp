using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Application.Services;

public class DeveloperService : IDeveloperService
{
    private readonly IBaseAccountService _accountServiceForWebApp;

    public DeveloperService(IBaseAccountService accountServiceForWebApp)
    {
        _accountServiceForWebApp = accountServiceForWebApp;
    }

    public async Task<List<UserDto>> GetAllDevelopers()
    {
        var userDeveloper = await _accountServiceForWebApp.GetAllUserOfRole(Roles.Developer, false);
        return userDeveloper;
    }

    public async Task<Result> SetStateAsync(string userId, bool state)
    {
        return await _accountServiceForWebApp.SetStateOnUser(userId, state);
    }
    
    public async Task<Result> DeleteAsync(string userId)
    {
        return await _accountServiceForWebApp.DeleteAsync(userId);
    }

    public async Task<Result<UserDto>> Create(UserSaveDto dto, string? origin)
    {
        dto.Role = nameof(Roles.Developer); // Solo para rectificar. Al fin y al cabo este metodo es un decorador
        return await _accountServiceForWebApp.RegisterUser(dto, origin);
    }

    public async Task<Result<UserDto>> Edit(UserSaveDto dto, string? origin)
    {
        return await _accountServiceForWebApp.EditUser(dto, origin);
    }
}