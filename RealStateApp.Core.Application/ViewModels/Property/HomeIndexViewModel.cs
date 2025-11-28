using RealStateApp.Core.Application.ViewModels.PropertyType;

namespace RealStateApp.Core.Application.ViewModels.Property;

public class HomeIndexViewModel
{
    public List<PropertyViewModel> Properties { get; set; }
    public HomeIndexFilters Filters { get; set; }
    public List<PropertyTypeViewModel> PropertyTypes { get; set; }
}

public class HomeIndexFilters
{
    public int? SelectedPropertyTypeId { get; set; }
    public double? MinValue { get; set; }
    public double? MaxValue { get; set; }
    public int?  Bathrooms { get; set; }
    public int?  Rooms { get; set; }
}