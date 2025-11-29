namespace RealStateApp.Core.Application.Dtos.SaleType;

public class SaleTypeWithCountDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; } 
    public required int PropertiesCount { get; set; } 
}