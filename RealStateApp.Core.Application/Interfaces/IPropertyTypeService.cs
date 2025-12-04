using Microsoft.AspNetCore.Mvc.Rendering;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.ViewModels.PropertyType;

namespace RealStateApp.Core.Application.Interfaces;

public interface IPropertyTypeService :  IGenericService<PropertyTypeDto>
{
 
    public Task<List<PropertyTypeWithCountViewModel>> GetAllPropertyTypesWithCount();
    Task<List<SelectListItem>> GetSelectListAsync();
}