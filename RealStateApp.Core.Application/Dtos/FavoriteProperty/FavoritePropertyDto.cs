using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Dtos.FavoriteProperty;

public class FavoritePropertyDto
{
    public required int Id { get; set; }
    public required string UserId { get; set; }
    public required int PropertyId { get; set; }
    public PropertyDto? Property { get; set; }
    public UserDto? User { get; set; }
}
