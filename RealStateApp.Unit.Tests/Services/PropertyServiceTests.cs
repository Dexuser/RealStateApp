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

public class PropertyServiceTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public PropertyServiceTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: $"RealStateApp_{Guid.NewGuid().ToString()}").Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PropertyDtoMappingProfile>();
            cfg.AddProfile<PropertyMappingProfile>();
        });
        _mapper = config.CreateMapper();
    }

  private PropertyService CreateService()
    {
        var context = new RealStateAppContext(_dbOptions);
        var accountMock = new Mock<IBaseAccountService>();
        var repo = new PropertyRepository(context);

        return new PropertyService(repo, _mapper, accountMock.Object);
    }

    private async Task SeedDependencies(RealStateAppContext context)
    {
        context.PropertyTypes.Add(new PropertyType
        {
            Id = 1,
            Name = "Casa",
            Description = "Tipo casa"
        });

        context.SaleTypes.Add(new SaleType
        {
            Id = 1,
            Name = "Venta",
            Description = "Venta completa"
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAll_Should_Return_All_Properties()
    {
        // Arrange
        var service = CreateService();

        var dtoList = new List<PropertyDto>
        {
            new PropertyDto
            {
                Code = "000001",
                PropertyTypeId = 1,
                SaleTypeId = 1,
                Price = 100,
                SizeInMeters = 50,
                Rooms = 2,
                Bathrooms = 1,
                AgentId = "agent1",
                CreatedAt = DateTime.UtcNow,
                Id = 0,
                Description = "casa bonita en punta cana" 
            },
            new PropertyDto
            {
                Code = "000002",
                PropertyTypeId = 1,
                SaleTypeId = 1,
                Price = 200,
                SizeInMeters = 80,
                Rooms = 3,
                Bathrooms = 2,
                AgentId = "agent2",
                CreatedAt = DateTime.UtcNow,
                Id = 0,
                Description = "casa linda en punta cana" 
            }
        };

        await service.AddRangeAsync(dtoList);

        // Act
        var properties = await service.GetAllAsync();

        // Assert
        properties.Should().NotBeNull();
        properties.Count.Should().Be(2);
    }

    [Fact]
    public async Task AddAsync_Should_Create_Property()
    {
        // Arrange
        var service = CreateService();

        var dto = new PropertyDto
        {
            Code = "000003",
            PropertyTypeId = 1,
            SaleTypeId = 1,
            Price = 500,
            SizeInMeters = 120,
            Rooms = 4,
            Bathrooms = 2,
            AgentId = "agent",
            CreatedAt = DateTime.UtcNow,
            Id = 0,
            Description = "descripcion" 
        };

        // Act
        var result = await service.AddAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Code.Should().Be("000003");
    }

    [Fact]
    public async Task AddAsync_Should_Return_Failure_When_Dto_Is_Null()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.AddAsync(null!);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Property()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);

        var entity = new Property
        {
            Id = 1,
            Code = "000001",
            PropertyTypeId = 1,
            SaleTypeId = 1,
            Price = 100,
            SizeInMeters = 50,
            Rooms = 2,
            Bathrooms = 1,
            AgentId = "agent",
            Description = "descripcion",
            CreatedAt = DateTime.UtcNow, 
        };

        context.Properties.Add(entity);
        await context.SaveChangesAsync();

        var service = CreateService();

        var dto = new PropertyDto
        {
            Id = 1,
            Code = "UPDATED",
            PropertyTypeId = 1,
            SaleTypeId = 1,
            Price = 999,
            SizeInMeters = 200,
            Rooms = 5,
            Bathrooms = 3,
            AgentId = "agentUpdated",
            CreatedAt = DateTime.UtcNow,
            Description = "descripcion" 
        };

        // Act
        var result = await service.UpdateAsync(dto.Id, dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Code.Should().Be("UPDATED");
        result.Value.Price.Should().Be(999);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Fail_When_Not_Found()
    {
        var service = CreateService();

        var dto = new PropertyDto
        {
            Id = 999,
            Code = "NOT FOUND",
            PropertyTypeId = 1,
            SaleTypeId = 1,
            Price = 100,
            SizeInMeters = 10,
            Rooms = 5,
            Bathrooms = 3,
            AgentId = "agentUpdated",
            CreatedAt = DateTime.UtcNow,
            Description = "descripcion" 
        };

        var result = await service.UpdateAsync(dto.Id, dto);

        result.IsFailure.Should().BeTrue();
    }



    [Fact]
    public async Task DeleteAsync_Should_Delete_Property()
    {
        var context = new RealStateAppContext(_dbOptions);

        var entity = new Property
        {
            Id = 1,
            Code = "000009",
            PropertyTypeId = 1,
            SaleTypeId = 1,
            Price = 300,
            SizeInMeters = 90,
            Rooms = 5,
            Bathrooms = 3,
            Description = "casa bonita",
            CreatedAt = DateTime.Now,
            AgentId = "agenteid" 
        };

        context.Properties.Add(entity);
        await context.SaveChangesAsync();

        var repository = new PropertyRepository(context);
        var accountMock = new Mock<IBaseAccountService>();
        var service = new PropertyService(repository, _mapper, accountMock.Object);

        var result = await service.DeleteAsync(1);
        result.IsSuccess.Should().BeTrue();
        var exists = await context.Properties.FindAsync(1);
        exists.Should().BeNull();
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
        var repo = new PropertyRepository(context);
        var service = new PropertyService(repo, _mapper, accountMock.Object);

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
        var repo = new PropertyRepository(context);
        var service = new PropertyService(repo, _mapper, accountMock.Object);

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
        var repo = new PropertyRepository(context);
        var service = new PropertyService(repo, _mapper, accountMock.Object);

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