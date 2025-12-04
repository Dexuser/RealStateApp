using AutoMapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Services;

public class SaleTypeServiceTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public SaleTypeServiceTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: $"RealStateApp_{Guid.NewGuid().ToString()}").Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<SaleTypeDtoMappingProfile>();
            cfg.AddProfile<SaleTypeMappingProfile>();
        });
        _mapper = config.CreateMapper();
    }

    public SaleTypeService CreateService()
    {
        var context = new RealStateAppContext(_dbOptions);
        var saleTypeRepository = new SaleTypeRepository(context);
        var propertyRepository = new PropertyRepository(context);
        return new SaleTypeService(saleTypeRepository, _mapper, propertyRepository);
    }


    [Fact]
    public async Task GetAll_Should_Return_All_SalesTypes()
    {
        // Arrange
        var service = CreateService();
        var dto = new List<SaleTypeDto>
        {
            new SaleTypeDto
            {
                Id = 0,
                Name = "Alquiler",
                Description = "Alquiler descripcion",
            },
            new SaleTypeDto
            {
                Id = 0,
                Name = "casa",
                Description = "casa descripcion"
            }
        };
        var result = await service.AddRangeAsync(dto);
        
        // Act
        var salesTypes = await service.GetAllAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        salesTypes.Count.Should().Be(2);
    }

    [Fact]
    public async Task AddAsync_Should_Create_SaleType()
    {
        // Arrange
        var service = CreateService();
        var dto = new SaleTypeDto
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
    public async Task UpdateAsync_Should_Update_SaleType()
    {
        var saleType = new SaleType
        {
            Id = 0,
            Name = "apartamento",
            Description = "apartamento descripcion"
        };
        var context = new RealStateAppContext(_dbOptions);
        context.SaleTypes.Add(saleType);

        var saleTypeRepository = new SaleTypeRepository(context);
        var propertyRepository = new PropertyRepository(context);


        // Arrange
        var service = new SaleTypeService(saleTypeRepository, _mapper, propertyRepository);
        var dto = new SaleTypeDto
        {
            Id = saleType.Id,
            Name = "updated",
            Description = "updated description"
        };

        // Act
        var result = await service.UpdateAsync(dto.Id, dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(saleType.Id);
        result.Value.Name.Should().Be(dto.Name);
        result.Value.Description.Should().Be(dto.Description);
    }


    [Fact]
    public async Task UpdateAsync_Should_Return_Result_Fail_When_Entity_Not_Exists()
    {
        // Arrange
        var service = CreateService();
        var dto = new SaleTypeDto
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
    public async Task DeleteAsync_Should_Delete_SaleType_And_Properties_With_That_Type()
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
        
        var saleType = new SaleType
        {
            Id = 0,
            Name = "apartamento",
            Description = "apartamento descripcion"
        };
        context.SaleTypes.Add(saleType);
        
        var property = new Property
        {
            Id = 0,
            Code = "000001",
            PropertyTypeId = 1,
            SaleTypeId = saleType.Id,
            Price = 500,
            SizeInMeters = 24,
            Rooms = 2,
            Bathrooms = 1,
            Description = "Casa bonita",
            CreatedAt = DateTime.Now,
            AgentId = "agentid"
        };
        context.Properties.Add(property);

        var saleTypeRepository = new SaleTypeRepository(context);
        var propertyRepository = new PropertyRepository(context);

        var service = new SaleTypeService(saleTypeRepository, _mapper, propertyRepository);
        // Act
        var deleteResult = await service.DeleteAsync(saleType.Id);

        // Assert
        deleteResult.IsSuccess.Should().BeTrue();
        context.Properties.Should().HaveCount(0);
    }

    [Fact]
    public async Task GetAllSaleTypeWithCountAsync_Should_Return_SalesTypes_With_Count()
    {
        
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        var saleType = new SaleType
        {
            Id = 0,
            Name = "apartamento",
            Description = "apartamento descripcion"
        };
        context.SaleTypes.Add(saleType);
        
        var properties = new List<Property>
        {
            new Property
            {
                Id = 0,
                Code = "000001",
                PropertyTypeId = 1,
                SaleTypeId = saleType.Id,
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
                PropertyTypeId = 1,
                SaleTypeId = saleType.Id,
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
        var salesTypes = await service.GetAllSaleTypeWithCountAsync();
        
        salesTypes.Should().HaveCount(1);
        salesTypes.Single().Id.Should().Be(saleType.Id);
        salesTypes.Single().PropertiesCount.Should().Be(2);
    }
}