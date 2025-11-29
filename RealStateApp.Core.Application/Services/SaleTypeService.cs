using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class SaleTypeService : GenericServices<SaleType, SaleTypeDto>, ISaleTypeService
{
    private readonly ISaleTypeRepository _saleTypeRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;
    public SaleTypeService(ISaleTypeRepository repository, IMapper mapper, IPropertyRepository propertyRepository) : base(repository, mapper)
    {
        _saleTypeRepository = repository;
        _mapper = mapper;
        _propertyRepository = propertyRepository;
    }

    public async Task<List<SaleTypeWithCountDto>> GetAllSaleTypeWithCountAsync()
    {
        var salesTypesWithCount = await _saleTypeRepository.GetAllQueryable().AsNoTracking()
            .Select(s => new SaleTypeWithCountDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                PropertiesCount = _propertyRepository.GetAllQueryable().AsNoTracking().Count(p => p.SaleTypeId == s.Id),
            }).ToListAsync();
        
        return salesTypesWithCount;
    }

    public override async Task<Result> DeleteAsync(int id)
    {
        var deleteResult = await base.DeleteAsync(id);
        if (deleteResult.IsSuccess)
        {
            var rowAffected = await _propertyRepository.GetAllQueryable().Where(p => p.SaleTypeId == id).ExecuteDeleteAsync();
        }
        return deleteResult;
    }
}