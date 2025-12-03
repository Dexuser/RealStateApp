namespace RealStateApp.Core.Domain.Entities;

public class PropertyImprovement
{
    public int Id { get; set; }
    public required int PropertyId { get; set; }
    public Property? Property { get; set; }

    public required int ImprovementId { get; set; }
    public Improvement? Improvement { get; set; }
}
