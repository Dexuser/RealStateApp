using Microsoft.AspNetCore.Identity;

namespace RealStateApp.Infrastructure.Identity.Entities;

public class AppUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? ProfileImagePath { get; set; }
    public string? IdentityCardNumber { get; set; }
    public required DateTime RegisteredAt { get; set; }
}