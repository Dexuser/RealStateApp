namespace RealStateApp.Core.Application.Dtos.Agent;

public class AgentDto
{
    public int Id { get; set; } 

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public string? ProfilePhoto { get; set; }

    // Información opcional
    public int TotalProperties { get; set; }
    public int AvailableProperties { get; set; }
    public int SoldProperties { get; set; }
}