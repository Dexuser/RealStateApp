using System.Net.Mime;
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
using RealStateApp.Core.Application.ViewModels.Property.Actions;
using RealStateApp.Core.Application.ViewModels.PropertyImage;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;


namespace RealStateApp.Core.Application.Services;

public class PropertyService(
    IPropertyRepository propertyRepository,
    IMapper mapper,
    IBaseAccountService accountServiceForWebApp,
    IPropertyImageRepository imageRepository,
    IPropertyImprovementRepository propertyImprovementRepository,
    IImprovementRepository improvementRepository,
    ISaleTypeRepository saleTypeRepository,
    IPropertyTypeRepository propertyTypeRepository)
    : GenericServices<Property, PropertyDto>(propertyRepository, mapper), IPropertyService
{
    private readonly IMapper _mapper = mapper;

    public override async Task<PropertyDto?> GetByIdAsync(int id)
    {
        var property = await propertyRepository.GetAllQueryable().AsNoTracking()
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
            property.Agent = await accountServiceForWebApp.GetUserById(property.AgentId);
        }

        return property;
    }

    public async Task<List<PropertyDto>> GetAllAvailablePropertiesAsync(PropertyFiltersDto filtersDto)
    {
        var query = propertyRepository
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

    public async Task<Result<List<PropertyDto>>> GetAllByAgentIdAsync(string agentId)
    {
        try
        {
            var properties = await propertyRepository
                .GetAllQueryable()
                .Where(x => x.AgentId == agentId)
                .Include(x => x.PropertyType)
                .Include(x => x.SaleType)
                .Include(x => x.PropertyImages)
                .Include(x => x.PropertyImprovements)
                .ThenInclude(pm => pm.Improvement)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            if (!properties.Any())
                return Result<List<PropertyDto>>.Fail("El Agente no tiene propiedades a su nombre");

            var dto = _mapper.Map<List<PropertyDto>>(properties);

            return Result<List<PropertyDto>>.Ok(dto);
        }
        catch (Exception ex)
        {
            return Result<List<PropertyDto>>.Fail($"Error: {ex.Message}");
        }
    }

    public async Task<Result<List<PropertyDto>>> GetPropertiesForMaintenanceAsync(string agentId)
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
                .Include(p => p.PropertyImprovements)
                .ThenInclude(pm => pm.Improvement)
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

                    PropertyImprovements = p.PropertyImprovements
                        .Select(pm => new ImprovementDto
                        {
                            Id = pm.Improvement!.Id,
                            Name = pm.Improvement.Name,
                            Description = pm.Improvement.Description
                        }).ToList()
                }).ToListAsync();
            if (!properties.Any())
                return Result<List<PropertyDto>>.Fail("No tienes propiedades disponibles registradas.");

            return Result<List<PropertyDto>>.Ok(properties);
        }
        catch (Exception ex)
        {
            return Result<List<PropertyDto>>.Fail($"Error al obtener propiedades: {ex.Message}");
        }
    }

    public async Task<Result<int>> CreatePropertyAsync(PropertyCreateViewModel vm, string agentId)
    {
        try
        {
            var property = new Property
            {
                Id = 0,
                Code = vm.Code,
                PropertyTypeId = vm.PropertyTypeId,
                SaleTypeId = vm.SaleTypeId,
                Price = vm.Price,
                SizeInMeters = vm.SizeInMeters,
                Rooms = vm.Rooms,
                Bathrooms = vm.Bathrooms,
                Description = vm.Description,
                AgentId = agentId,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow,
            };

            await propertyRepository.AddAsync(property);


            if (vm.MainImage != null)
            {
                string imagePath = FileHandler.Upload(vm.MainImage, property.Id.ToString(), "properties")!;

                var mainImage = new PropertyImage
                {
                    PropertyId = property.Id,
                    ImagePath = imagePath,
                    IsMain = true
                };

                await imageRepository.AddAsync(mainImage);
            }


            if (vm.AdditionalImages != null)
            {
                foreach (var img in vm.AdditionalImages)
                {
                    string path = FileHandler.Upload(img, property.Id.ToString(), "properties")!;

                    var imgEntity = new PropertyImage
                    {
                        PropertyId = property.Id,
                        ImagePath = path,
                        IsMain = false
                    };

                    await imageRepository.AddAsync(imgEntity);
                }
            }


            if (vm.SelectedImprovements.Any())
            {
                foreach (var improvementId in vm.SelectedImprovements)
                {
                    await propertyImprovementRepository.AddAsync(new PropertyImprovement
                    {
                        PropertyId = property.Id,
                        ImprovementId = improvementId
                    });
                }
            }

            return Result<int>.Ok(property.Id);
        }
        catch (Exception ex)
        {
            return Result<int>.Fail($"Error creando propiedad: {ex.Message}");
        }
    }

    public async Task<Result<PropertyEditViewModel>> GetByIdForEditAsync(int id)
    {
        try
        {
            var property = await propertyRepository
                .GetAllQueryable()
                .Include(p => p.PropertyImages)
                .Include(p => p.PropertyImprovements)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
                return Result<PropertyEditViewModel>.Fail("Propiedad no encontrada.");

            // Map basic fields
            var vm = new PropertyEditViewModel
            {
                Id = property.Id,
                Code = property.Code,
                PropertyTypeId = property.PropertyTypeId,
                SaleTypeId = property.SaleTypeId,
                Price = property.Price,
                SizeInMeters = property.SizeInMeters,
                Rooms = property.Rooms,
                Bathrooms = property.Bathrooms,
                Description = property.Description,

                CurrentImages = property.PropertyImages
                    .Select(i => new PropertyImageViewModel
                    {
                        Id = i.Id,
                        ImagePath = i.ImagePath,
                        IsMain = i.IsMain
                    }).ToList(),

                SelectedImprovements = property.PropertyImprovements
                    .Select(x => x.ImprovementId)
                    .ToList()
            };

            var types = await propertyTypeRepository.GetAllAsync();
            vm.PropertyTypes = types
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name })
                .ToList();

            var sales = await saleTypeRepository.GetAllAsync();
            vm.SaleTypes = sales
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList();

            var improvements = await improvementRepository.GetAllAsync();
            vm.Improvements = improvements
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
                .ToList();

            return Result<PropertyEditViewModel>.Ok(vm);
        }
        catch (Exception ex)
        {
            return Result<PropertyEditViewModel>.Fail($"Error al cargar la propiedad: {ex.Message}");
        }
    }

    public async Task<Result<bool>> EditPropertyAsync(PropertyEditViewModel vm)
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

            property.Code = vm.Code;
            property.PropertyTypeId = vm.PropertyTypeId;
            property.SaleTypeId = vm.SaleTypeId;
            property.Price = vm.Price;
            property.SizeInMeters = vm.SizeInMeters;
            property.Rooms = vm.Rooms;
            property.Bathrooms = vm.Bathrooms;
            property.Description = vm.Description;

            await propertyRepository.UpdateAsync(property.Id, property);

            if (vm.ImagesToDelete.Count != 0)
            {
                foreach (var imgId in vm.ImagesToDelete)
                {
                    var imgEntity = property.PropertyImages.FirstOrDefault(i => i.Id == imgId);
                    if (imgEntity != null)
                    {
                        // Borrar archivo físico
                        FileHandler.DeleteFile(imgEntity.ImagePath);

                        // Borrar registro
                      //  await imageRepository.DeleteAsync(imgEntity);
                    }
                }
            }

            if (vm.NewMainImage != null)
            {
                // Buscar la imagen principal actual (puede ser null)
                var oldMain = property.PropertyImages.FirstOrDefault(i => i.IsMain);

                // Si existe oldMain, pasar su path para que FileHandler la elimine cuando subamos la nueva
                string oldMainPath = oldMain?.ImagePath ?? string.Empty;

                // Subir nueva imagen principal (isEditMode = true para que FileHandler elimine la vieja)
                var newMainPath = FileHandler.Upload(vm.NewMainImage, property.Id.ToString(), "properties", true,
                    oldMainPath);

                // Desmarcar el oldMain en BD si existe
                if (oldMain != null)
                {
                    oldMain.IsMain = false;
                    await imageRepository.UpdateAsync(oldMain.Id, oldMain);
                }

                // Agregar la nueva imagen como main
                var mainImgEntity = new PropertyImage
                {
                    PropertyId = property.Id,
                    ImagePath = newMainPath!,
                    IsMain = true
                };

                await imageRepository.AddAsync(mainImgEntity);
            }

            // 5) Nuevas imágenes adicionales
            if (vm.NewAdditionalImages != null && vm.NewAdditionalImages.Any())
            {
                foreach (var file in vm.NewAdditionalImages)
                {
                    var path = FileHandler.Upload(file, property.Id.ToString(), "properties");

                    var imgEntity = new PropertyImage
                    {
                        PropertyId = property.Id,
                        ImagePath = path,
                        IsMain = false
                    };

                    await imageRepository.AddAsync(imgEntity);
                }
            }

            // Asegurar que exista una imagen principal si hay imágenes
            var anyMain = imageRepository
                    .GetAllQueryable()
                    .FirstOrDefault(i => i.PropertyId == property.Id && i.IsMain);

            if (anyMain == null)
            {
                // Buscar primera imagen existente
                var firstImg = imageRepository
                    .GetAllQueryable()
                    .FirstOrDefault(i => i.PropertyId == property.Id);

                if (firstImg != null)
                {
                    firstImg.IsMain = true;
                    await imageRepository.UpdateAsync(firstImg.Id, firstImg);
                }
            }

            // 7) Actualizar mejoras: eliminar las previas y crear las nuevas
            // Borrar las relaciones anteriores
            await propertyImprovementRepository.DeleteAsync(property.Id);

            // Insertar las actuales seleccionadas
            if (vm.SelectedImprovements.Count != 0)
            {
                foreach (var impId in vm.SelectedImprovements)
                {
                    var newPi = new PropertyImprovement
                    {
                        PropertyId = property.Id,
                        ImprovementId = impId
                    };
                    await propertyImprovementRepository.AddAsync(newPi);
                }
            }

            // 8) Fin — devolver éxito
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Error actualizando la propiedad: {ex.Message}");
        }
    }
}

