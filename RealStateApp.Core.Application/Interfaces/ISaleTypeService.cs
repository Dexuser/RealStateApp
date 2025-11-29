using RealStateApp.Core.Application.Dtos.SaleType;

namespace RealStateApp.Core.Application.Interfaces;

public interface ISaleTypeService : IGenericService<SaleTypeDto>
{
    Task<List<SaleTypeWithCountDto>> GetAllSaleTypeWithCountAsync();
}