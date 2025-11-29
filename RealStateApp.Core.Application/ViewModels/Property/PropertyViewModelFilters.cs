namespace RealStateApp.Core.Application.ViewModels.Property;

public class PropertyViewModelFilters
{
    public string? AgentId { get; set; }
    public int? SelectedPropertyTypeId { get; set; }
    public double? MinValue { get; set; }
    public double? MaxValue { get; set; }
    public int?  Bathrooms { get; set; }
    public int?  Rooms { get; set; }
}