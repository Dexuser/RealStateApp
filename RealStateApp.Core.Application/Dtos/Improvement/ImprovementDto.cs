using RealStateApp.Core.Application.Dtos.PropertyImprovement;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Dtos.Improvement;

public class ImprovementDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ICollection<PropertyImprovementDto> PropertyImprovements { get; set; } = [];
}
