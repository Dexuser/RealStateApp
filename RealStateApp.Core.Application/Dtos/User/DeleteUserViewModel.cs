namespace RealStateApp.Core.Application.Dtos.User;

public class DeleteUserViewModel
{
    public required string UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}