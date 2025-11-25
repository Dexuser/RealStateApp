namespace RealStateApp.Core.Domain.Entities;

public class FavoriteProperty
{
    public required int Id { get; set; }
    public required string UserId { get; set; }
    public required int PropertyId { get; set; }
    public Property? Property { get; set; }
}
