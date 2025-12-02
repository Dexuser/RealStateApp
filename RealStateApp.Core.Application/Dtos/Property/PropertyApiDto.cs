using RealStateApp.Core.Application.Dtos.User;

namespace RealStateApp.Core.Application.Dtos.Property;

public class PropertyApiDto
{
    public required int Id { get; set; }
    public required string Code { get; set; } // 6 digit unique
    public required string  PropertyType { get; set; }
    public required string SaleType { get; set; }
    public required decimal Price { get; set; }
    public required double SizeInMeters { get; set; }
    public required int Rooms { get; set; }
    public required int Bathrooms { get; set; }
    public required string Description { get; set; }
    public bool IsAvailable { get; set; } // true = available, false = sold
    public required DateTime CreatedAt { get; set; }

    // Navigation
    public IEnumerable<string> PropertyImprovements { get; set; } = [];
    public SimpleAgentForPropertyApiDto? Agent { get; set; }
}




