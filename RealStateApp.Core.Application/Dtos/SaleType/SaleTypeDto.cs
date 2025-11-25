using RealStateApp.Core.Application.Dtos.Property;

namespace RealStateApp.Core.Application.Dtos.SaleType;

public class SaleTypeDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ICollection<PropertyDto> Properties { get; set; } = [];
}