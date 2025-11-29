using RealStateApp.Core.Application.Dtos.Property;

namespace RealStateApp.Core.Application.Interfaces;

public interface IPropertyService : IGenericService<PropertyDto>
{
    Task<List<PropertyDto>> GetAllAvailablePropertiesAsync(PropertyFiltersDto filtersDto);
}