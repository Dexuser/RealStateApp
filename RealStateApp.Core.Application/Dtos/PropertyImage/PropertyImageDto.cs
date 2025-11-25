using RealStateApp.Core.Application.Dtos.Property;

namespace RealStateApp.Core.Application.Dtos.PropertyImage;

public class PropertyImageDto
{
    public required int Id { get; set; }
    public required string ImagePath { get; set; }
    public required int PropertyId { get; set; }
    public PropertyDto? Property { get; set; }
}
