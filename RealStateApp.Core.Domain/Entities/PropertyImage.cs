namespace RealStateApp.Core.Domain.Entities;

public class PropertyImage
{
    public int Id { get; set; }
    public required string ImagePath { get; set; }
    public required int PropertyId { get; set; }
    public required bool IsMain { get; set; }
    public Property? Property { get; set; }
}
