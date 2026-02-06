using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.ViewModels.Property;

public class AgentDetailsViewModel
{
    public required List<UserViewModel> ChatClients { get; set; } = [];
    public required List<UserViewModel> OffersClients { get; set; } = [];
    public required PropertyViewModel Property { get; set; }
}