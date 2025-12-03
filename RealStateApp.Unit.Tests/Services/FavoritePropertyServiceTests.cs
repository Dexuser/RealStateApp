using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.FavoriteProperty;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Services;

public class FavoritePropertyServiceTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public FavoritePropertyServiceTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: $"RealStateApp_{Guid.NewGuid()}")
            .Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<FavoritePropertyDtoMappingProfile>();
            cfg.AddProfile<FavoritePropertyMappingProfile>();
        });



        _mapper = config.CreateMapper();
    }

    private FavoritePropertyService CreateService()
    {
        var context = new RealStateAppContext(_dbOptions);
        var repository = new FavoritePropertyRepository(context);
        return new FavoritePropertyService(repository, _mapper);
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

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAll_Should_Return_All_FavoriteProperties()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var dto1 = new FavoritePropertyDto
        {
            Id = 0,
            UserId = "user1",
            PropertyId = 1
        };

        var dto2 = new FavoritePropertyDto
        {
            Id = 0,
            UserId = "user2",
            PropertyId = 1
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
    public async Task AddAsync_Should_Create_FavoriteProperty()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var service = CreateService();

        var dto = new FavoritePropertyDto
        {
            Id = 0,
            UserId = "user1",
            PropertyId = 1
        };

        // Act
        var result = await service.AddAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        result.Value!.UserId.Should().Be("user1");
        result.Value.PropertyId.Should().Be(1);
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
    public async Task UpdateAsync_Should_Update_FavoriteProperty()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var entity = new FavoriteProperty
        {
            Id = 1,
            UserId = "oldUser",
            PropertyId = 1
        };

        context.FavoriteProperties.Add(entity);
        await context.SaveChangesAsync();

        var service = CreateService();

        var dto = new FavoritePropertyDto
        {
            Id = 1,
            UserId = "updatedUser",
            PropertyId = 1
        };

        // Act
        var result = await service.UpdateAsync(dto.Id, dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(1);
        result.Value.UserId.Should().Be("updatedUser");
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Fail_When_Not_Found()
    {
        // Arrange
        var service = CreateService();

        var dto = new FavoritePropertyDto
        {
            Id = 999,
            UserId = "userX",
            PropertyId = 1
        };

        // Act
        var result = await service.UpdateAsync(999, dto);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_FavoriteProperty()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var entity = new FavoriteProperty
        {
            Id = 1,
            UserId = "deleteUser",
            PropertyId = 1
        };

        context.FavoriteProperties.Add(entity);
        await context.SaveChangesAsync();

        var repository = new FavoritePropertyRepository(context);
        var service = new FavoritePropertyService(repository, _mapper);

        // Act
        var result = await service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var exists = await context.FavoriteProperties.FindAsync(1);
        exists.Should().BeNull();
    }
}
