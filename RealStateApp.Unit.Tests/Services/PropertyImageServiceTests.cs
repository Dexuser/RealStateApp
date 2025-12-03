using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.PropertyImage;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Services;

public class PropertyImageServiceTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public PropertyImageServiceTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: $"RealStateApp_{Guid.NewGuid()}")
            .Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PropertyImageDtoMappingProfile>();
            cfg.AddProfile<PropertyImageMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    private PropertyImageService CreateService()
    {
        var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImageRepository(context);
        return new PropertyImageService(repository, _mapper);
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
    public async Task GetAll_Should_Return_All_PropertyImages()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var dto1 = new PropertyImageDto
        {
            Id = 0,
            ImagePath = "image1.jpg",
            PropertyId = 1,
            IsMain = false
        };

        var dto2 = new PropertyImageDto
        {
            Id = 0,
            ImagePath = "image2.jpg",
            PropertyId = 1,
            IsMain = true
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
    public async Task AddAsync_Should_Create_PropertyImage()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var service = CreateService();

        var dto = new PropertyImageDto
        {
            Id = 0,
            ImagePath = "main.jpg",
            PropertyId = 1,
            IsMain = true
        };

        // Act
        var result = await service.AddAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.PropertyId.Should().Be(1);
        result.Value.ImagePath.Should().Be("main.jpg");
        result.Value.IsMain.Should().BeTrue();
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
    public async Task UpdateAsync_Should_Update_PropertyImage()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var entity = new PropertyImage
        {
            Id = 1,
            ImagePath = "old.jpg",
            PropertyId = 1,
            IsMain = false
        };

        context.PropertyImages.Add(entity);
        await context.SaveChangesAsync();

        var service = CreateService();

        var dto = new PropertyImageDto
        {
            Id = 1,
            ImagePath = "updated.jpg",
            PropertyId = 1,
            IsMain = true
        };

        // Act
        var result = await service.UpdateAsync(dto.Id, dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(1);
        result.Value.ImagePath.Should().Be("updated.jpg");
        result.Value.IsMain.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Fail_When_Not_Found()
    {
        // Arrange
        var service = CreateService();

        var dto = new PropertyImageDto
        {
            Id = 999,
            ImagePath = "weird.jpg",
            PropertyId = 1,
            IsMain = false
        };

        // Act
        var result = await service.UpdateAsync(999, dto);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_PropertyImage()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var entity = new PropertyImage
        {
            Id = 1,
            ImagePath = "delete.jpg",
            PropertyId = 1,
            IsMain = false
        };

        context.PropertyImages.Add(entity);
        await context.SaveChangesAsync();

        var repository = new PropertyImageRepository(context);
        var service = new PropertyImageService(repository, _mapper);

        // Act
        var result = await service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var exists = await context.PropertyImages.FindAsync(1);
        exists.Should().BeNull();
    }
}
