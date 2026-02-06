using Microsoft.AspNetCore.Http;

namespace RealStateApp.Core.Application.ViewModels.Agent;

public class AgentProfileViewModel
{
    public string Id { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string UserName { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string? ProfileImagePath { get; set; }

    public IFormFile? ProfileImage { get; set; }
}