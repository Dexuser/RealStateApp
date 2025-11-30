using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Application.ViewModels.Offer;

public class OfferViewModel
{
    public required int Id { get; set; }

    public required int PropertyId { get; set; }
    public PropertyViewModel? Property { get; set; }
    public required string UserId { get; set; } // Client
    public required decimal Amount { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required OfferStatus Status { get; set; }
    
    public UserViewModel? User { get; set; }
}
