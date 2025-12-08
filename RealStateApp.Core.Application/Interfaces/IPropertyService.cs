using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.ViewModels.Property;

namespace RealStateApp.Core.Application.Interfaces;

public interface IPropertyService : IGenericService<PropertyDto>
{
    Task<List<PropertyDto>> GetAllAvailablePropertiesAsync(PropertyFiltersDto filtersDto);
    Task<List<PropertyDto>> GetAllByAgentIdAsync(string agentId);

    Task<List<PropertyDto>> GetPropertiesForMaintenanceAsync(string agentId);
    Task<Result<int>> CreatePropertyAsync(PropertyDto vm);
    Task<Result<bool>> EditPropertyAsync(PropertyDto vm);
    Task<Result<PropertyDeleteViewModel>> GetByIdForDeleteAsync(int id);
    Task<Result<bool>> DeletePropertyAsync(int id);
}