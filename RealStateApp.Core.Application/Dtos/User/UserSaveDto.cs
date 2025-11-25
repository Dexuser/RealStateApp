namespace RealStateApp.Core.Application.Dtos.User;

public class UserSaveDto
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public string? ProfileImagePath { get; set; }
    public string? PhoneNumber { get; set; }
    public string? IdentityCardNumber { get; set; }
    public required string Role { get; set; }
    
}
