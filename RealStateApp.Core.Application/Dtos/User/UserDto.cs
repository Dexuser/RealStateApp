namespace RealStateApp.Core.Application.Dtos.User;

public class UserDto
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public string? IdentityCardNumber { get; set; } // Los Admin y dev tienen cedula pero no imagen
    public string? PhoneNumber {get;set;} // Los clientes y agentes tienen numero y perfil, pero no cedula
    public string? ProfileImagePath { get; set; } // Los clientes y agentes tienen numero y perfil, pero no cedula
    public required DateTime RegisteredAt { get; set; }
    public bool? IsVerified { get; set; }
    public required string Role { get; set; }
}