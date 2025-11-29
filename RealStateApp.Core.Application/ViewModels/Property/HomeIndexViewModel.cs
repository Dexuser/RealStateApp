using RealStateApp.Core.Application.ViewModels.PropertyType;

namespace RealStateApp.Core.Application.ViewModels.Property;

public class HomeIndexViewModel
{
    public List<PropertyViewModel> Properties { get; set; }
    public PropertyViewModelFilters Filters { get; set; }
    public List<PropertyTypeViewModel> PropertyTypes { get; set; }
}
