using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Dtos.User;

namespace RealStateApp.Core.Application.Interfaces;

public interface IAgentService 
{
    Task<List<AgentWithPropertyCountDto>> GetAllAgentsWithCount();
    Task<List<UserDto>> GetAllAgents(bool onlyActive = false, string? name = null);
    Task<Result> SetStatus(string userId, bool state);
    Task<Result> DeleteAsync(string userId);
    Task<Result<UserDto>> Edit(UserSaveDto dto, string? origin);


}