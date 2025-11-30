using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.PropertyImage;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class PropertyService : GenericServices<Property, PropertyDto>, IPropertyService
{
    private readonly IAccountServiceForWebApp _accountServiceForWebApp;

    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public PropertyService(IPropertyRepository repository, IMapper mapper,
        IAccountServiceForWebApp accountServiceForWebApp) : base(repository, mapper)
    {
        _propertyRepository = repository;
        _mapper = mapper;
        _accountServiceForWebApp = accountServiceForWebApp;
    }

    public override async Task<PropertyDto?> GetByIdAsync(int id)
    {
        var property = await _propertyRepository.GetAllQueryable().AsNoTracking()
            .AsNoTracking()
            .AsSplitQuery()
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
    var query = _propertyRepository
        .GetAllQueryable()
        .AsNoTracking()
        .AsSplitQuery() // para evitar una consulta grandisima
        .Where(p => p.IsAvailable);

    if (!string.IsNullOrEmpty(filtersDto.AgentId))
        query = query.Where(p => p.AgentId == filtersDto.AgentId);

    if (filtersDto.SelectedPropertyTypeId.HasValue)
        query = query.Where(p => p.PropertyTypeId == filtersDto.SelectedPropertyTypeId);

    if (filtersDto.MinValue.HasValue)
        query = query.Where(p => p.Price >= (decimal)filtersDto.MinValue.Value);

    if (filtersDto.MaxValue.HasValue)
        query = query.Where(p => p.Price <= (decimal)filtersDto.MaxValue.Value);

    if (filtersDto.Bathrooms.HasValue)
        query = query.Where(p => p.Bathrooms >= filtersDto.Bathrooms.Value);

    if (filtersDto.Rooms.HasValue)
        query = query.Where(p => p.Rooms >= filtersDto.Rooms.Value);

    if (filtersDto.OnlyFavorites)
    {
        query = query.Where(p =>
            p.FavoriteProperties.Any(fp => fp.UserId == filtersDto.ClientId));
    }

    query = query
        .Include(p => p.PropertyType)
        .Include(p => p.SaleType)
        .Include(p => p.PropertyImages);

    var result = await query
        .OrderByDescending(p => p.CreatedAt)
        .Select(p => new PropertyDto
        {
            Id = p.Id,
            Code = p.Code,
            PropertyTypeId = p.PropertyTypeId,
            SaleTypeId = p.SaleTypeId,
            Price = p.Price,
            SizeInMeters = p.SizeInMeters,
            Rooms = p.Rooms,
            Bathrooms = p.Bathrooms,
            Description = p.Description,
            CreatedAt = p.CreatedAt,
            AgentId = p.AgentId,
            IsAvailable = p.IsAvailable,

            PropertyType = new PropertyTypeDto
            {
                Id = p.PropertyType.Id,
                Name = p.PropertyType.Name,
                Description = p.PropertyType.Description,
            },

            SaleType = new SaleTypeDto
            {
                Id = p.SaleType.Id,
                Name = p.SaleType.Name,
                Description = p.SaleType.Description,
            },

            PropertyImages = p.PropertyImages
                .Where(pi => pi.IsMain)
                .Select(pi => new PropertyImageDto
                {
                    Id = pi.Id,
                    ImagePath = pi.ImagePath,
                    PropertyId = pi.PropertyId,
                    IsMain = pi.IsMain,
                })
                .ToList(),

            IsFavorite = p.FavoriteProperties
                .Any(fp => fp.UserId == filtersDto.ClientId)
        })
        .ToListAsync();

    return result;
}

}