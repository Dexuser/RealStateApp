using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.DashBoard;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class DashBoardService : IDashBoardService
{
    private readonly IBaseAccountService _accountServiceForWebApp;
    private readonly IPropertyRepository _propertyRepository;

    public DashBoardService(IBaseAccountService accountServiceForWebApp, IPropertyRepository propertyRepository)
    {
        _accountServiceForWebApp = accountServiceForWebApp;
        _propertyRepository = propertyRepository;
    }

    public async Task<AdminDashBoardDto> GetAdminDashBoard()
    {
        var soldProperties = await _propertyRepository.GetAllQueryable().AsNoTracking().CountAsync(p => !p.IsAvailable);
        var availableProperties = await _propertyRepository.GetAllQueryable().AsNoTracking().CountAsync(p => p.IsAvailable);
        var activeClients = await _accountServiceForWebApp.CountUsers(Roles.Client, true);
        var inactiveClients = await _accountServiceForWebApp.CountUsers(Roles.Client, false);
        var activeAgents = await _accountServiceForWebApp.CountUsers(Roles.Agent, true);
        var inactiveAgents = await _accountServiceForWebApp.CountUsers(Roles.Agent, false);       
        var activeDevelopers = await _accountServiceForWebApp.CountUsers(Roles.Developer, true);
        var inactiveDevelopers = await _accountServiceForWebApp.CountUsers(Roles.Developer, false);

        return new AdminDashBoardDto
        {
            AvailablePropertiesCount = availableProperties,
            SoldPropertiesCount = soldProperties,
            ActiveAgentsCount = activeAgents,
            InactiveAgentsCount = inactiveAgents,
            ActiveClientsCount = activeClients,
            InactiveClientsCount = inactiveClients,
            ActiveDevelopersCount = activeDevelopers,
            InActiveDevelopersCount = inactiveDevelopers,
        };
    }
    
}