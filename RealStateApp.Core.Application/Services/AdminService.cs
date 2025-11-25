using ArtemisBanking.Core.Application.Dtos.Login;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class AdminService
{
    private readonly IAccountServiceForWebApp _accountServiceForWebApp;
    private readonly IPropertyRepository _propertyRepository;

    public AdminService(IAccountServiceForWebApp accountServiceForWebApp, IPropertyRepository propertyRepository)
    {
        _accountServiceForWebApp = accountServiceForWebApp;
        _propertyRepository = propertyRepository;
    }

    public async Task<List<UserDto>> GetAllAdmins()
    {
        var userAdmin = await _accountServiceForWebApp.GetAllUserOfRole(Roles.Admin, false);
        return userAdmin;
    }

    public async Task<Result> SetStatusOnAdmin(string userId, bool state)
    {
        return await _accountServiceForWebApp.SetStateOnUser(userId, state);
    }
    
    public async Task<Result> DeleteAdmin(string userId)
    {
        return await _accountServiceForWebApp.DeleteAsync(userId);
    }

    public async Task<Result<UserDto>> CreateAdmin(UserSaveDto dto, string? origin)
    {
        dto.Role = nameof(Roles.Admin); // Solo para rectificar. Al fin y al cabo este metodo es un decorador
        return await _accountServiceForWebApp.RegisterUser(dto, origin);
    }

    public async Task<Result<UserDto>> EditAdmin(UserSaveDto dto, string? origin)
    {
        return await _accountServiceForWebApp.EditUser(dto, origin);
    }
}