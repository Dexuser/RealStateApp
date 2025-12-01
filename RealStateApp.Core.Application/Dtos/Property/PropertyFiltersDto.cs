namespace RealStateApp.Core.Application.Dtos.Property;

public class PropertyFiltersDto
{
    public string? AgentId { get; set; }
    public required string ClientId { get; set; } // para poder dicriminar los favoritos de las publicaciones
    public int? SelectedPropertyTypeId { get; set; }
    public double? MinValue { get; set; }
    public double? MaxValue { get; set; }
    public int?  Bathrooms { get; set; }
    public int?  Rooms { get; set; }
    public required bool OnlyFavorites { get; set; }
}