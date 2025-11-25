using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class AgentService : IAgentService
{
    private readonly IAccountServiceForWebApp _accountServiceForWebApp;
    private readonly IPropertyRepository _propertyRepository;

    public AgentService(IAccountServiceForWebApp accountServiceForWebApp, IPropertyRepository propertyRepository)
    {
        _accountServiceForWebApp = accountServiceForWebApp;
        _propertyRepository = propertyRepository;
    }

    public async Task<List<AgentWithPropertyCountDto>> GetAllAgents()
    {
        var userAgents = await _accountServiceForWebApp.GetAllUserOfRole(Roles.Agent, false);
        var agentsWithPropertyCount = new List<AgentWithPropertyCountDto>();
        
        foreach (var userAgent in userAgents)
        {
            int propertyCount = await _propertyRepository.GetAllQueryable().AsNoTracking()
                .CountAsync(p => p.AgentId == userAgent.Id);
            
            agentsWithPropertyCount.Add(new AgentWithPropertyCountDto
            {
                PropertyCount = propertyCount,
                User = userAgent
            });
        }
        return agentsWithPropertyCount;
    }

    // Estos metodos simplemente devuelven el Objeto result del accountService
    public async Task<Result> SetStatusOnAgent(string userId, bool state)
    {
        return await _accountServiceForWebApp.SetStateOnUser(userId, state);
    }
    
    public async Task<Result> DeleteAgentAsync(string userId)
    {
         var deleteResult = await _accountServiceForWebApp.DeleteAsync(userId);
         if (deleteResult.IsSuccess)
         {
             var rowAffected = await _propertyRepository.GetAllQueryable().Where(property => property.AgentId == userId).ExecuteDeleteAsync();
         }
         return deleteResult;
    }
}