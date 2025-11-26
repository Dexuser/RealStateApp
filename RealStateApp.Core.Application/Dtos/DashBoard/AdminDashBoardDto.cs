namespace RealStateApp.Core.Application.Dtos.DashBoard;

public class AdminDashBoardDto
{
    public required int AvailablePropertiesCount { get; set; }
    public required int SoldPropertiesCount { get; set; }
    public required int ActiveAgentsCount { get; set; }
    public required int InactiveAgentsCount { get; set; }
    public required int ActiveClientsCount { get; set; }
    public required int InactiveClientsCount { get; set; }
    public required int ActiveDevelopersCount { get; set; }
    public required int InActiveDevelopersCount { get; set; }
}