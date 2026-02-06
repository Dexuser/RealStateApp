using Microsoft.AspNetCore.Mvc.Rendering;
using RealStateApp.Core.Application.Dtos.PropertyImprovement;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Interfaces;

public interface IPropertyImprovementService :  IGenericService<PropertyImprovementDto>
{
    Task<List<SelectListItem>> GetSelectListAsync();
    
    Task<Result> DeleteAllImprovementsOfAPropertyAsync(int propertyId);

}