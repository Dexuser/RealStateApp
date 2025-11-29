using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class PropertyService : GenericServices<Property, PropertyDto>, IPropertyService
{
    private readonly IAccountServiceForWebApp _accountServiceForWebApp;
    
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;
    public PropertyService(IPropertyRepository repository, IMapper mapper, IAccountServiceForWebApp accountServiceForWebApp) : base(repository, mapper)
    {
        _propertyRepository = repository;
        _mapper = mapper;
        _accountServiceForWebApp = accountServiceForWebApp;
    }

    public override async Task<PropertyDto?> GetByIdAsync(int id)
    {
         var property =  await _propertyRepository.GetAllQueryable().AsNoTracking()
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.PropertyImages)
            .Include(p => p.PropertyImprovements).ThenInclude(pi => pi.Improvement)
            .ProjectTo<PropertyDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsAvailable);

         if (property != null)
         {
             property.Agent = await _accountServiceForWebApp.GetUserById(property.AgentId);
         }
         
         return property;
    }

    public async Task<List<PropertyDto>> GetAllAvailablePropertiesAsync(PropertyFiltersDto filtersDto)
    {
        var query = _propertyRepository.GetAllQueryable().AsNoTracking()
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.PropertyImages.Where(pi => pi.IsMain))
            .OrderByDescending(p => p.CreatedAt)
            .Where(p => p.IsAvailable);

        if (!String.IsNullOrEmpty(filtersDto.AgentId))
        {
            query = query.Where(p => p.AgentId == filtersDto.AgentId);
        }
        if (filtersDto.SelectedPropertyTypeId.HasValue)
        {
            query = query.Where(p => p.PropertyTypeId == filtersDto.SelectedPropertyTypeId);
        }
        if (filtersDto.MinValue.HasValue)
        {
            query = query.Where(p => p.Price >= (decimal)filtersDto.MinValue);
        }
        if (filtersDto.MaxValue.HasValue)
        {
            query = query.Where(p => p.Price <= (decimal)filtersDto.MaxValue);
        }
        if (filtersDto.Bathrooms.HasValue)
        {
            query = query.Where(p => p.Bathrooms >= filtersDto.Bathrooms.Value);
        }
        if (filtersDto.Rooms.HasValue)
        {
            query = query.Where(p => p.Rooms >= filtersDto.Rooms.Value);
        }
        
        var availableProperties =
             await query.ProjectTo<PropertyDto>(_mapper.ConfigurationProvider).ToListAsync();

        return availableProperties;
    }

}