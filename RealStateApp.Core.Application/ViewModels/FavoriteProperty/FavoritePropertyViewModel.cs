using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.ViewModels.FavoriteProperty;

public class FavoritePropertyViewModel
{
    public required int Id { get; set; }
    public required string UserId { get; set; }
    public required int PropertyId { get; set; }
    public PropertyViewModel? Property { get; set; }
    public UserViewModel? User { get; set; }
}
