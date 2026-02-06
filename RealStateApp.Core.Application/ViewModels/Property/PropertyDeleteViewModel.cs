namespace RealStateApp.Core.Application.ViewModels.Property;

public class PropertyDeleteViewModel
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public double SizeInMeters { get; set; }
    public int Rooms { get; set; }
    public int Bathrooms { get; set; }

    public int PropertyTypeId { get; set; }
    public string PropertyTypeName { get; set; } = string.Empty;

    public int SaleTypeId { get; set; }
    public string SaleTypeName { get; set; } = string.Empty;

    public string? MainImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }
}