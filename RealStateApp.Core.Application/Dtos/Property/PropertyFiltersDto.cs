namespace RealStateApp.Core.Application.Dtos.Property;

public class PropertyFiltersDto
{
    public int? SelectedPropertyTypeId { get; set; }
    public double? MinValue { get; set; }
    public double? MaxValue { get; set; }
    public int?  Bathrooms { get; set; }
    public int?  Rooms { get; set; }
}