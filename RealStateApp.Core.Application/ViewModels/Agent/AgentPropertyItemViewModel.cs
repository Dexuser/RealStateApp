namespace RealStateApp.Core.Application.ViewModels.Agent;

public class AgentPropertyItemViewModel
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public string? MainImage { get; set; }
}