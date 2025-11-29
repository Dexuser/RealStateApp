namespace RealStateApp.Core.Application.Dtos.PropertyType;

public class PropertyTypeWithCountDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; } 
    public required int PropertiesCount { get; set; }      
}
