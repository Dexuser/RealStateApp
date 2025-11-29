namespace RealStateApp.Core.Application.ViewModels.SaleType;

public class SaleTypeWithCountViewModel
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; } 
    public required int PropertiesCount { get; set; } 
}