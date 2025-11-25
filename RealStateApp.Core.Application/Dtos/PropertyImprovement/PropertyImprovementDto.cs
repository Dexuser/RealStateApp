using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Dtos.Property;

namespace RealStateApp.Core.Application.Dtos.PropertyImprovement;

public class PropertyImprovementDto
{
    public required int Id { get; set; }
    public required int PropertyId { get; set; }
    public PropertyDto? Property { get; set; }

    public required int ImprovementId { get; set; }
    public ImprovementDto? Improvement { get; set; }
}
