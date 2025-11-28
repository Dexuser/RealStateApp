using RealStateApp.Core.Application.ViewModels.PropertyType;
using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.ViewModels.Property;

public class AgentPropertiesViewModel
{
    public UserViewModel Agent { get; set; }
    public List<PropertyViewModel> Properties { get; set; }
    public PropertyViewModelFilters Filters { get; set; }
    public List<PropertyTypeViewModel> PropertyTypes { get; set; }
}
