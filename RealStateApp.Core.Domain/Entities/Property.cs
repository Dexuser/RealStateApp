namespace RealStateApp.Core.Domain.Entities;

public class Property
{
    public required int Id { get; set; }
    public required string Code { get; set; } // 6 digit unique

    public required int PropertyTypeId { get; set; }
    public PropertyType? PropertyType { get; set; }

    public required int SaleTypeId { get; set; }
    public SaleType? SaleType { get; set; }

    public required decimal Price { get; set; }
    public required double SizeInMeters { get; set; }
    public required int Rooms { get; set; }
    public required int Bathrooms { get; set; }
    public required string Description { get; set; }
    public bool IsAvailable { get; set; } // true = available, false = sold

    public required string AgentId { get; set; }

    // Navigation
    public ICollection<PropertyImage> PropertyImages { get; set; } = [];
    public ICollection<PropertyImprovement> PropertyImprovements { get; set; } = [];
    public ICollection<Offer> Offers { get; set; } = [];
    public ICollection<ChatMessage> ChatMessages { get; set; } = [];
    public ICollection<FavoriteProperty> FavoriteProperties { get; set; } = [];
}
