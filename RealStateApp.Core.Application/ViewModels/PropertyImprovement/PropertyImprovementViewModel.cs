using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.ViewModels.Improvement;
using RealStateApp.Core.Application.ViewModels.Property;

namespace RealStateApp.Core.Application.ViewModels.PropertyImprovement;

public class PropertyImprovementViewModel
{
    public required int Id { get; set; }
    public required int PropertyId { get; set; }
    public PropertyViewModel? Property { get; set; }

    public required int ImprovementId { get; set; }
    public ImprovementViewModel? Improvement { get; set; }
}
