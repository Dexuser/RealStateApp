using RealStateApp.Core.Application.Dtos.User;

namespace RealStateApp.Core.Application.Dtos.Agent;

public class AgentWithPropertyCountDto
{
    public required UserDto User { get; set; }
    public required int PropertyCount { get; set; }
}