namespace RealStateApp.Core.Domain.Entities;

public class SaleType
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ICollection<Property> Properties { get; set; } = [];
}