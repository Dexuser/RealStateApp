using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Domain.Common;
namespace RealStateApp.Core.Application.Dtos.Offer;

public class OfferDto
{
    public required int Id { get; set; }

    public required int PropertyId { get; set; }
    public PropertyDto? Property { get; set; }
    public required string UserId { get; set; } // Client
    public required decimal Amount { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required OfferStatus Status { get; set; }
    
    public UserDto? User { get; set; }
}
