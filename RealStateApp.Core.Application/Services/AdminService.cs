using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class AdminService : IAdminService
{
    private readonly IBaseAccountService _accountServiceForWebApp;

    public AdminService(IBaseAccountService accountServiceForWebApp)
    {
        _accountServiceForWebApp = accountServiceForWebApp;
    }

    public async Task<List<UserDto>> GetAllAdmins()
    {
        var userAdmin = await _accountServiceForWebApp.GetAllUserOfRole(Roles.Admin, false);
        return userAdmin;
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
        dto.Role = nameof(Roles.Admin); // Solo para rectificar. Al fin y al cabo este metodo es un decorador
        return await _accountServiceForWebApp.RegisterUser(dto, origin);
    }

    public async Task<Result<UserDto>> Edit(UserSaveDto dto, string? origin)
    {
        return await _accountServiceForWebApp.EditUser(dto, origin);
    }
}