using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Core.Domain.Entities;

public class Offer
{
    public required int Id { get; set; }

    public required int PropertyId { get; set; }
    public Property? Property { get; set; }
    public required string UserId { get; set; } // Client
    public required decimal Amount { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required OfferStatus Status { get; set; }
}
