namespace RealStateApp.Core.Application.Dtos.Agent;

public class AgentApiDto
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required int PropertyCount { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
}