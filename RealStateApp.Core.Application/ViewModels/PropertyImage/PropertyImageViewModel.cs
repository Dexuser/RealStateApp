using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.ViewModels.Property;

namespace RealStateApp.Core.Application.ViewModels.PropertyImage;

public class PropertyImageViewModel
{
    public required int Id { get; set; }
    public required string ImagePath { get; set; }
    public required int PropertyId { get; set; }
    public PropertyViewModel? Property { get; set; }
}
