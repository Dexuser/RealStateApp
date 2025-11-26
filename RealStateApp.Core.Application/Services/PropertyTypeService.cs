using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.PropertyType;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class PropertyTypeService :  GenericServices<PropertyType, PropertyTypeDto>, IPropertyTypeService
{
    private readonly IPropertyTypeRepository _propertyTypeRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;
    public PropertyTypeService(IPropertyTypeRepository repository, IMapper mapper, IPropertyRepository propertyRepository) : base(repository, mapper)
    {
        _propertyTypeRepository = repository;
        _mapper = mapper;
        _propertyRepository = propertyRepository;
    }

    public async Task<List<PropertyTypeWithCountViewModel>> GetAllPropertyTypesWithCount()
    {
        var propertyTypes = await _propertyTypeRepository.GetAllQueryable().AsNoTracking()
            .Select(pt => new PropertyTypeWithCountViewModel
            {
                Id = pt.Id,
                Name = pt.Name,
                Description = pt.Description,
                PropertiesCount = _propertyRepository.GetAllQueryable().Count(p => p.PropertyTypeId == pt.Id),
            })
            .ToListAsync();
        
        return propertyTypes;
    }

    public override async Task<Result> DeleteAsync(int id)
    {
        var deleteResult = await base.DeleteAsync(id);
        if (deleteResult.IsSuccess)
        {
            var rowAffected = await _propertyRepository.GetAllQueryable().Where(p => p.PropertyTypeId == id).ExecuteDeleteAsync();
        }
        return deleteResult;
    }
}