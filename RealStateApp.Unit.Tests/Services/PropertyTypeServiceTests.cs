using AutoMapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Services;

public class PropertyTypeServiceTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public PropertyTypeServiceTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: $"RealStateApp_{Guid.NewGuid().ToString()}").Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PropertyTypeDtoMappingProfile>();
            cfg.AddProfile<PropertyTypeMappingProfile>();
        });
        _mapper = config.CreateMapper();
    }

    public PropertyTypeService CreateService()
    {
        var context = new RealStateAppContext(_dbOptions);
        var propertyTypeRepository = new PropertyTypeRepository(context);
        var propertyRepository = new PropertyRepository(context);
        return new PropertyTypeService(propertyTypeRepository, _mapper, propertyRepository);
    }


    [Fact]
    public async Task GetAll_Should_Return_All_PropertyTypes()
    {
        // Arrange
        var service = CreateService();
        var dto = new List<PropertyTypeDto>
        {
            new PropertyTypeDto
            {
                Id = 0,
                Name = "Casa",
                Description = "Casa en la bahia",
            },
            new PropertyTypeDto
            {
                Id = 0,
                Name = "Proyecto en construccion",
                Description = "Listo para el 2025"
            }
        };
        var result = await service.AddRangeAsync(dto);
        // Act
        var propertyTypes = await service.GetAllAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        propertyTypes.Count.Should().Be(2);
    }

    [Fact]
    public async Task AddAsync_Should_Create_PropertyType()
    {
        // Arrange
        var service = CreateService();
        var dto = new PropertyTypeDto()
        {
            Id = 0,
            Name = "Alquiler",
            Description = "Alquiler descripcion",
        };

        // Act
        var result = await service.AddAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().NotBe(0);
    }

    [Fact]
    public async Task AddAsync_Should_Return_Result_Fail_On_Exception()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.AddAsync(null!);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_PropertyType()
    {
        var propertyType = new PropertyType()
        {
            Id = 0,
            Name = "apartamento",
            Description = "apartamento descripcion"
        };
        var context = new RealStateAppContext(_dbOptions);
        context.Add(propertyType);
        await context.SaveChangesAsync();

        var propertyTypeRepository = new PropertyTypeRepository(context);
        var propertyRepository = new PropertyRepository(context);


        // Arrange
        var service = new PropertyTypeService(propertyTypeRepository, _mapper, propertyRepository);
        var dto = new PropertyTypeDto()
        {
            Id = propertyType.Id,
            Name = "updated",
            Description = "updated description"
        };

        // Act
        var result = await service.UpdateAsync(dto.Id, dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(propertyType.Id);
        result.Value.Name.Should().Be(dto.Name);
        result.Value.Description.Should().Be(dto.Description);
    }


    [Fact]
    public async Task UpdateAsync_Should_Return_Result_Fail_When_Entity_Not_Exists()
    {
        // Arrange
        var service = CreateService();
        var dto = new PropertyTypeDto()
        {
            Id = 999,
            Name = "updated",
            Description = "updated description"
        };

        // Act
        var result = await service.UpdateAsync(dto.Id, dto);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_PropertyType_And_Properties_With_That_Type()
    {
        // Arrange
        // Los delete usan ExecuteDeleteAsync. DBinMemory no lo soporta, pero SQLite si. 
        // SQLite tambien puede usarse unicamente en memoria.
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseSqlite(connection)
            .Options;

        var context = new RealStateAppContext(options);
        await context.Database.EnsureCreatedAsync();

        var propertyType = new PropertyType()
        {
            Id = 0,
            Name = "apartamento",
            Description = "apartamento descripcion"
        };
        context.Add(propertyType);

        var property = new Property
        {
            Id = 0,
            Code = "000001",
            PropertyTypeId = propertyType.Id,
            SaleTypeId = 1,
            Price = 500,
            SizeInMeters = 24,
            Rooms = 2,
            Bathrooms = 1,
            Description = "Casa bonita",
            CreatedAt = DateTime.Now,
            AgentId = "agentid"
        };
        context.Add(property);

        var propertyTypeRepository = new PropertyTypeRepository(context);
        var propertyRepository = new PropertyRepository(context);

        var service = new PropertyTypeService(propertyTypeRepository, _mapper, propertyRepository);
        // Act
        var deleteResult = await service.DeleteAsync(propertyType.Id);

        // Assert
        deleteResult.IsSuccess.Should().BeTrue();
        context.Properties.Should().HaveCount(0);
    }

    [Fact]
    public async Task GetAllPropertyTypeWithCountAsync_Should_Return_PropertyTypes_With_Count()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        var propertyType = new PropertyType()
        {
            Id = 0,
            Name = "apartamento",
            Description = "apartamento descripcion"
        };
        context.Add(propertyType);

        var properties = new List<Property>
        {
            new Property
            {
                Id = 0,
                Code = "000001",
                PropertyTypeId = propertyType.Id,
                SaleTypeId = 1,
                Price = 500,
                SizeInMeters = 24,
                Rooms = 3,
                Bathrooms = 2,
                Description = "Casa bonita",
                CreatedAt = DateTime.Now,
                AgentId = "agentid",
            },
            new Property
            {
                Id = 0,
                Code = "000002",
                PropertyTypeId = propertyType.Id,
                SaleTypeId = 1,
                Price = 500,
                SizeInMeters = 25,
                Rooms = 3,
                Bathrooms = 2,
                Description = "Casa bonita 2",
                CreatedAt = DateTime.Now,
                AgentId = "agentid",
            }
        };

        context.Properties.AddRange(properties);
        await context.SaveChangesAsync();
        var service = CreateService();

        // Act
        var propertyTypes = await service.GetAllPropertyTypesWithCount();

        propertyTypes.Should().HaveCount(1);
        propertyTypes.Single().Id.Should().Be(propertyType.Id);
        propertyTypes.Single().PropertiesCount.Should().Be(2);
    }


    [Fact]
    public async Task GetAllAvailablePropertiesAsync_Returns_Only_Available_Properties()
    {
        // Arrange

        var context = new RealStateAppContext(_dbOptions);
        var houseType = new PropertyType
        {
            Id = 1,
            Name = "Casa",
            Description = "Casa bonita"
        };

        var apartmentType = new PropertyType
        {
            Id = 2,
            Name = "Departamento",
            Description = "Departamento bonito"
        };

        var saleType = new SaleType
        {
            Id = 1,
            Name = "Venta",
            Description = "Venta completa"
        };

        context.PropertyTypes.AddRange(houseType, apartmentType);
        context.SaleTypes.Add(saleType);

        context.Properties.AddRange(
            new Property
            {
                Id = 1,
                Code = "000001",
                IsAvailable = true,
                Price = 100,
                Rooms = 2,
                Bathrooms = 1,
                CreatedAt = DateTime.UtcNow,
                AgentId = "1",
                PropertyTypeId = houseType.Id,
                SaleTypeId = saleType.Id,
                SizeInMeters = 80,
                Description = "Disponible",
            },
            new Property
            {
                Id = 2,
                Code = "000002",
                IsAvailable = false,
                Price = 200,
                Rooms = 3,
                Bathrooms = 2,
                CreatedAt = DateTime.UtcNow,
                AgentId = "2",
                PropertyTypeId = apartmentType.Id,
                SaleTypeId = saleType.Id,
                SizeInMeters = 120,
                Description = "No disponible"
            }
        );
        await context.SaveChangesAsync();

        var accountMock = new Mock<IBaseAccountService>(); // No hace falta introducir usuarios
        var propertyImageRepository = new PropertyImageRepository(context);
        var propertyImprovementRepository = new PropertyImprovementRepository(context);
        var improvementRepository = new ImprovementRepository(context);
        var saleTypeRepository = new SaleTypeRepository(context);
        var propertyRepository = new PropertyRepository(context);
        var propertyTypeRepository = new PropertyTypeRepository(context);

        var service =  new PropertyService(propertyRepository, _mapper, accountMock.Object, propertyImageRepository,
            propertyImprovementRepository, improvementRepository, saleTypeRepository, propertyTypeRepository);

        var filters = new PropertyFiltersDto
        {
            ClientId = "clientId",
            OnlyFavorites = false
        }; // sin filtros adicionales.

        // Act
        var result = await service.GetAllAvailablePropertiesAsync(filters);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);

        var property = result.Single();
        property.IsAvailable.Should().BeTrue();
        property.Code.Should().Be("000001");
    }

    [Fact]
    public async Task GetAllAvailablePropertiesAsync_Returns_Only_Properties_In_The_Price_Range()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);

        var houseType = new PropertyType
        {
            Id = 1,
            Name = "Casa",
            Description = "Casa bonita"
        };

        var apartmentType = new PropertyType
        {
            Id = 2,
            Name = "Departamento",
            Description = "Departamento bonito"
        };

        var saleType = new SaleType
        {
            Id = 1,
            Name = "Venta",
            Description = "Venta completa"
        };

        context.PropertyTypes.AddRange(houseType, apartmentType);
        context.SaleTypes.Add(saleType);

        // Dentro del rango
        var propertyInRange = new Property
        {
            Id = 1,
            Code = "000001",
            IsAvailable = true,
            Price = 250,
            Rooms = 2,
            Bathrooms = 1,
            CreatedAt = DateTime.UtcNow,
            AgentId = "1",
            PropertyTypeId = houseType.Id,
            SaleTypeId = saleType.Id,
            SizeInMeters = 80,
            Description = "Dentro de rango"
        };

        // Fuera del rango (muy barata)
        var propertyBelowRange = new Property
        {
            Id = 2,
            Code = "000002",
            IsAvailable = true,
            Price = 50,
            Rooms = 3,
            Bathrooms = 2,
            CreatedAt = DateTime.UtcNow,
            AgentId = "2",
            PropertyTypeId = apartmentType.Id,
            SaleTypeId = saleType.Id,
            SizeInMeters = 120,
            Description = "Muy barata"
        };

        // Fuera del rango (muy cara)
        var propertyAboveRange = new Property
        {
            Id = 3,
            Code = "000003",
            IsAvailable = true,
            Price = 800,
            Rooms = 4,
            Bathrooms = 3,
            CreatedAt = DateTime.UtcNow,
            AgentId = "3",
            PropertyTypeId = houseType.Id,
            SaleTypeId = saleType.Id,
            SizeInMeters = 200,
            Description = "Muy cara"
        };

        context.Properties.AddRange(propertyInRange, propertyBelowRange, propertyAboveRange);
        await context.SaveChangesAsync();

        var accountMock = new Mock<IBaseAccountService>();
        var propertyImageRepository = new PropertyImageRepository(context);
        var propertyImprovementRepository = new PropertyImprovementRepository(context);
        var improvementRepository = new ImprovementRepository(context);
        var saleTypeRepository = new SaleTypeRepository(context);
        var propertyRepository = new PropertyRepository(context);
        var propertyTypeRepository = new PropertyTypeRepository(context);

        var service =  new PropertyService(propertyRepository, _mapper, accountMock.Object, propertyImageRepository,
            propertyImprovementRepository, improvementRepository, saleTypeRepository, propertyTypeRepository);

        var filters = new PropertyFiltersDto
        {
            MinValue = 200,
            MaxValue = 500,
            ClientId = "clientId",
            OnlyFavorites = false
        };

        // Act
        var result = await service.GetAllAvailablePropertiesAsync(filters);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);

        var property = result.Single();
        property.Price.Should().Be(250);
        property.Code.Should().Be("000001");
        property.IsAvailable.Should().BeTrue();
    }
    
        [Fact]
    public async Task GetAllAvailablePropertiesAsync_Returns_Only_Favorites_Properties()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);

        var houseType = new PropertyType
        {
            Id = 1,
            Name = "Casa",
            Description = "Casa bonita"
        };

        var apartmentType = new PropertyType
        {
            Id = 2,
            Name = "Departamento",
            Description = "Departamento bonito"
        };

        var saleType = new SaleType
        {
            Id = 1,
            Name = "Venta",
            Description = "Venta completa"
        };

        context.PropertyTypes.AddRange(houseType, apartmentType);
        context.SaleTypes.Add(saleType);

        // Dentro del rango
        var propertyInRange = new Property
        {
            Id = 1,
            Code = "000001",
            IsAvailable = true,
            Price = 250,
            Rooms = 2,
            Bathrooms = 1,
            CreatedAt = DateTime.UtcNow,
            AgentId = "1",
            PropertyTypeId = houseType.Id,
            SaleTypeId = saleType.Id,
            SizeInMeters = 80,
            Description = "Dentro de rango"
        };

        // Fuera del rango (muy barata)
        var propertyBelowRange = new Property
        {
            Id = 2,
            Code = "000002",
            IsAvailable = true,
            Price = 50,
            Rooms = 3,
            Bathrooms = 2,
            CreatedAt = DateTime.UtcNow,
            AgentId = "2",
            PropertyTypeId = apartmentType.Id,
            SaleTypeId = saleType.Id,
            SizeInMeters = 120,
            Description = "Muy barata"
        };

        // Fuera del rango (muy cara)
        var propertyAboveRange = new Property
        {
            Id = 3,
            Code = "000003",
            IsAvailable = true,
            Price = 800,
            Rooms = 4,
            Bathrooms = 3,
            CreatedAt = DateTime.UtcNow,
            AgentId = "3",
            PropertyTypeId = houseType.Id,
            SaleTypeId = saleType.Id,
            SizeInMeters = 200,
            Description = "Muy cara"
        };

        context.Properties.AddRange(propertyInRange, propertyBelowRange, propertyAboveRange);
        await context.SaveChangesAsync();

        var favoriteProperty = new FavoriteProperty
        {
            Id = 1,
            UserId = "clientId",
            PropertyId = propertyAboveRange.Id
        };
        context.FavoriteProperties.Add(favoriteProperty);
        await context.SaveChangesAsync();

        var accountMock = new Mock<IBaseAccountService>();
        var propertyImageRepository = new PropertyImageRepository(context);
        var propertyImprovementRepository = new PropertyImprovementRepository(context);
        var improvementRepository = new ImprovementRepository(context);
        var saleTypeRepository = new SaleTypeRepository(context);
        var propertyRepository = new PropertyRepository(context);
        var propertyTypeRepository = new PropertyTypeRepository(context);

        var service =  new PropertyService(propertyRepository, _mapper, accountMock.Object, propertyImageRepository,
            propertyImprovementRepository, improvementRepository, saleTypeRepository, propertyTypeRepository);

        var filters = new PropertyFiltersDto
        {
            ClientId = "clientId",
            OnlyFavorites = true 
        };

        // Act
        var result = await service.GetAllAvailablePropertiesAsync(filters);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);

        var property = result.Single();
        property.Price.Should().Be(800);
        property.Code.Should().Be("000003");
        property.IsAvailable.Should().BeTrue();
        property.IsFavorite.Should().BeTrue();
    }
}