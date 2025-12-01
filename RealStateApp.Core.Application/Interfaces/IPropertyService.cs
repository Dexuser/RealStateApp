using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.Agent;
namespace RealStateApp.Core.Application.Interfaces;

public interface IPropertyService : IGenericService<PropertyDto>
{
    Task<List<PropertyDto>> GetAllAvailablePropertiesAsync(PropertyFiltersDto filtersDto);
    Task<Result<List<PropertyDto>>> GetAllByAgentIdAsync(string agentId);
}