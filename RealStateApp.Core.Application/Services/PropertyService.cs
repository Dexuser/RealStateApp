using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.PropertyImage;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Handler;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.PropertyImage;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Services;


namespace RealStateApp.Core.Application.Services;

public class PropertyService(
    IPropertyRepository propertyRepository,
    IMapper mapper,
    IBaseAccountService accountServiceForWebApp,
    IPropertyImageRepository imageRepository,
    IPropertyImprovementRepository propertyImprovementRepository,
    IImprovementRepository improvementRepository,
    ISaleTypeRepository saleTypeRepository,
    IPropertyTypeRepository propertyTypeRepository,
    ICodeService codeService)
    : GenericServices<Property, PropertyDto>(propertyRepository, mapper), IPropertyService
{
    private readonly IMapper _mapper = mapper;


    public override async Task<PropertyDto?> GetByIdAsync(int id)
    {
        var property = await propertyRepository.GetAllQueryable()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.PropertyImages)
            .Include(p => p.PropertyImprovements).ThenInclude(pi => pi.Improvement)
            .ProjectTo<PropertyDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == id );

        if (property != null)
        {
            property.Agent = await accountServiceForWebApp.GetUserById(property.AgentId);
        }

        return property;
    }

    public async Task<List<PropertyDto>> GetAllAvailablePropertiesAsync(PropertyFiltersDto filtersDto)
    {
        var query = propertyRepository
            .GetAllQueryable()
            .AsNoTracking()
            .AsSplitQuery()
            .Where(p => p.IsAvailable);
        
        if (!string.IsNullOrEmpty(filtersDto.PropertyCode))
            query = query.Where(p => p.Code.Contains(filtersDto.PropertyCode));

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
                    Id = p.PropertyType!.Id,
                    Name = p.PropertyType.Name,
                    Description = p.PropertyType.Description,
                },

                SaleType = new SaleTypeDto
                {
                    Id = p.SaleType!.Id,
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


    public async Task<List<PropertyDto>> GetAllByAgentIdAsync(string agentId)
    {
        try
        {
            var properties = await propertyRepository
                .GetAllQueryable()
                .AsNoTracking()
                .AsSplitQuery()
                .Where(p => p.AgentId == agentId && p.IsAvailable)
                .Include(p => p.PropertyType)
                .Include(p => p.SaleType)
                .Include(p => p.PropertyImages)
                .OrderByDescending(p => p.CreatedAt)
                .ThenBy(p => p.IsAvailable)
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
                        Id = p.PropertyType!.Id,
                        Name = p.PropertyType.Name,
                        Description = p.PropertyType.Description,
                    },

                    SaleType = new SaleTypeDto
                    {
                        Id = p.SaleType!.Id,
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
                        .ToList()
                })
                .ToListAsync();

            return properties;
  
        }
        catch (Exception ex)
        {
            return [];
        }
    }

    public async Task<List<PropertyDto>> GetPropertiesForMaintenanceAsync(string agentId)
    {
        try
        {
            var properties = await propertyRepository
                .GetAllQueryable()
                .AsNoTracking()
                .AsSplitQuery()
                .Where(p => p.AgentId == agentId && p.IsAvailable)
                .Include(p => p.PropertyType)
                .Include(p => p.SaleType)
                .Include(p => p.PropertyImages)
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
                        Id = p.PropertyType!.Id,
                        Name = p.PropertyType.Name,
                        Description = p.PropertyType.Description,
                    },

                    SaleType = new SaleTypeDto
                    {
                        Id = p.SaleType!.Id,
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
                        .ToList()
                })
                .ToListAsync();

            return properties;
        }
        catch (Exception ex)
        {
            return [];
        }
    }

    public async Task<Result<int>> CreatePropertyAsync(PropertyDto vm)
    {
        try
        {
            var property = new Property
            {
                Id = 0,
                Code = await codeService.GenerateIdentifier(),
                PropertyTypeId = vm.PropertyTypeId,
                SaleTypeId = vm.SaleTypeId,
                Price = vm.Price,
                SizeInMeters = vm.SizeInMeters,
                Rooms = vm.Rooms,
                Bathrooms = vm.Bathrooms,
                Description = vm.Description,
                AgentId = vm.AgentId,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow,
            };

            await propertyRepository.AddAsync(property);

            return Result<int>.Ok(property.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Fail($"Error creando propiedad: {ex.Message}");
        }
    }





    public async Task<Result<bool>> EditPropertyAsync(PropertyDto vm)
    {
        try
        {
            var property = await propertyRepository
                .GetAllQueryable()
                .Include(p => p.PropertyImages)
                .Include(p => p.PropertyImprovements)
                .FirstOrDefaultAsync(p => p.Id == vm.Id);

            if (property == null)
                return Result<bool>.Fail("Propiedad no encontrada.");

            // No se actualiza el codigo, ni la fecha ni el agente
            property.PropertyTypeId = vm.PropertyTypeId;
            property.SaleTypeId = vm.SaleTypeId;
            property.Price = vm.Price;
            property.SizeInMeters = vm.SizeInMeters;
            property.Rooms = vm.Rooms;
            property.Bathrooms = vm.Bathrooms;
            property.Description = vm.Description;
            await propertyRepository.UpdateAsync(property.Id, property);

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Error actualizando la propiedad: {ex.Message}");
        }
    }

    public async Task<Result<PropertyDeleteViewModel>> GetByIdForDeleteAsync(int id)
    {
        try
        {
            var property = await propertyRepository
                .GetAllQueryable()
                .AsNoTracking()
                .Include(p => p.PropertyType)
                .Include(p => p.SaleType)
                .Include(p => p.PropertyImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
                return Result<PropertyDeleteViewModel>.Fail("Propiedad no encontrada.");

            var vm = new PropertyDeleteViewModel
            {
                Id = property.Id,
                Code = property.Code,
                Price = property.Price,
                SizeInMeters = property.SizeInMeters,
                Rooms = property.Rooms,
                Bathrooms = property.Bathrooms,
                CreatedAt = property.CreatedAt,

                PropertyTypeId = property.PropertyTypeId,
                PropertyTypeName = property.PropertyType?.Name ?? string.Empty,

                SaleTypeId = property.SaleTypeId,
                SaleTypeName = property.SaleType?.Name ?? string.Empty,

                MainImageUrl = property.PropertyImages
                    .FirstOrDefault(i => i.IsMain)?.ImagePath
            };

            return Result<PropertyDeleteViewModel>.Ok(vm);
        }
        catch (Exception ex)
        {
            return Result<PropertyDeleteViewModel>.Fail($"Error cargando propiedad: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeletePropertyAsync(int id)
    {
        try
        {
            var property = await propertyRepository
                .GetAllQueryable()
                .Include(p => p.PropertyImages)
                .Include(p => p.PropertyImprovements)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
                return Result<bool>.Fail("Propiedad no encontrada.");
            
            // La relacion de PropertyImages y PropertyImprovementes tiene OnDeleteBehavoiur On Cascade

            // Eliminar propiedad
            await propertyRepository.DeleteAsync(property.Id);

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Error eliminando propiedad: {ex.Message}");
        }
    }
}