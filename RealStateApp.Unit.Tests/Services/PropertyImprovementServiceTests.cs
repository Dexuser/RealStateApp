using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.PropertyImprovement;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Services;

public class PropertyImprovementTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public PropertyImprovementTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: $"RealStateApp_{Guid.NewGuid()}")
            .Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PropertyImprovementDtoMappingProfile>();
            cfg.AddProfile<PropertyImprovementMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    private PropertyImprovementService CreateService()
    {
        var context = new RealStateAppContext(_dbOptions);
        var propertyImprovementRepository = new PropertyImprovementRepository(context);
        return new PropertyImprovementService(propertyImprovementRepository, _mapper);
    }

    private async Task SeedDependencies(RealStateAppContext context)
    {
        context.Properties.Add(new Property
        {
            Id = 1,
            Code = "000001",
            PropertyTypeId = 1,
            SaleTypeId = 1,
            Price = 100,
            SizeInMeters = 25,
            Rooms = 1,
            Bathrooms = 1,
            Description = "Propiedad",
            CreatedAt = DateTime.UtcNow,
            AgentId = "agent"
        });

        context.Improvements.AddRange(
            new Improvement
            {
                Id = 1,
                Name = "Piscina",
                Description = "Piscina grande"
            },
            new Improvement
            {
                Id = 2,
                Name = "Aire acondicionado",
                Description = "aire acondicionado descripcion"
            });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAll_Should_Return_All_PropertyImprovements()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);
        await context.SaveChangesAsync();

        var dto1 = new PropertyImprovementDto
        {
            Id = 0,
            PropertyId = 1,
            ImprovementId = 1
        };

        var dto2 = new PropertyImprovementDto
        {
            Id = 0,
            PropertyId = 1,
            ImprovementId = 1
        };
        
        var service = CreateService();
        await service.AddAsync(dto1);
        await service.AddAsync(dto2);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(2);
    }

    [Fact]
    public async Task AddAsync_Should_Create_PropertyImprovement()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var service = CreateService();

        var dto = new PropertyImprovementDto
        {
            PropertyId = 1,
            ImprovementId = 1,
            Id = 0
        };

        // Act
        var result = await service.AddAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.PropertyId.Should().Be(1);
        result.Value.ImprovementId.Should().Be(1);
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
    public async Task UpdateAsync_Should_Update_PropertyImprovement()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var entity = new PropertyImprovement
        {
            Id = 1,
            PropertyId = 1,
            ImprovementId = 1
        };

        context.PropertyImprovements.Add(entity);
        await context.SaveChangesAsync();

        var service = CreateService();

        var dto = new PropertyImprovementDto
        {
            Id = 1,
            PropertyId = 1,
            ImprovementId = 2
        };

        // Act
        var result = await service.UpdateAsync(dto.Id, dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(1);
        result.Value!.ImprovementId.Should().Be(2);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Fail_When_Not_Found()
    {
        // Arrange
        var service = CreateService();

        var dto = new PropertyImprovementDto
        {
            Id = 999,
            PropertyId = 1,
            ImprovementId = 1
        };

        // Act
        var result = await service.UpdateAsync(999, dto);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_PropertyImprovement()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var entity = new PropertyImprovement
        {
            Id = 0,
            PropertyId = 1,
            ImprovementId = 1
        };
        context.PropertyImprovements.Add(entity);
        
        await context.SaveChangesAsync();
        var repository = new PropertyImprovementRepository(context);
        var service = new PropertyImprovementService(repository, _mapper);

        // Act
        var result = await service.DeleteAsync(entity.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var exists = await context.PropertyImprovements.FindAsync(1);
        exists.Should().BeNull();
    }
}