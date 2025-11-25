using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.ViewModels.Agent;

public class AgentWithPropertyCountViewModel
{
    public required UserViewModel User { get; set; }
    public required int PropertyCount { get; set; }
}