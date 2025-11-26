using RealStateApp.Core.Application.Dtos.Agent;

namespace RealStateApp.Core.Application.Interfaces;

public interface IAgentService
{
    Task<List<AgentWithPropertyCountDto>> GetAllAgents();
    Task<Result> SetStatus(string userId, bool state);
    Task<Result> DeleteAsync(string userId);
}