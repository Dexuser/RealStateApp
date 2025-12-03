using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.ViewModels.Property.Actions;

namespace RealStateApp.Core.Application.Interfaces;

public interface IPropertyService : IGenericService<PropertyDto>
{
    Task<List<PropertyDto>> GetAllAvailablePropertiesAsync(PropertyFiltersDto filtersDto);
    Task<Result<List<PropertyDto>>> GetAllByAgentIdAsync(string agentId);

    Task<Result<List<PropertyDto>>> GetPropertiesForMaintenanceAsync(string agentId);
    Task<Result<int>> CreatePropertyAsync(PropertyCreateViewModel vm, string agentId);
    Task<Result<PropertyEditViewModel>> GetByIdForEditAsync(int id);
    Task<Result<bool>> EditPropertyAsync(PropertyEditViewModel vm);
    Task<Result<PropertyDeleteViewModel>> GetByIdForDeleteAsync(int id);
    Task<Result<bool>> DeletePropertyAsync(int id);
}