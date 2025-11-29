using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.ViewModels.Property;

namespace RealStateApp.Core.Application.ViewModels.SaleType;

public class SaleTypeViewModel
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ICollection<PropertyViewModel> Properties { get; set; } = [];
}