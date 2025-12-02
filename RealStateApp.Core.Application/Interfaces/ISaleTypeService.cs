using Microsoft.AspNetCore.Mvc.Rendering;
using RealStateApp.Core.Application.Dtos.SaleType;

namespace RealStateApp.Core.Application.Interfaces;

public interface ISaleTypeService : IGenericService<SaleTypeDto>
{
    Task<List<SaleTypeWithCountDto>> GetAllSaleTypeWithCountAsync();
    Task<List<SelectListItem>> GetSelectListAsync(); 
    
}