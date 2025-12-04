using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Services;

public class OfferServiceTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public OfferServiceTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: $"RealStateApp_{Guid.NewGuid()}")
            .Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OfferDtoMappingProfile>();
            cfg.AddProfile<OfferMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    private OfferService CreateService()
    {
        var context = new RealStateAppContext(_dbOptions);
        var repository = new OfferRepository(context);
        return new OfferService(repository, _mapper);
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
    public async Task GetAll_Should_Return_All_Offers()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var dto1 = new OfferDto
        {
            Id = 0,
            PropertyId = 1,
            UserId = "user1",
            Amount = 1000,
            CreatedAt = DateTime.UtcNow,
            Status = OfferStatus.Pending
        };

        var dto2 = new OfferDto
        {
            Id = 0,
            PropertyId = 1,
            UserId = "user2",
            Amount = 2000,
            CreatedAt = DateTime.UtcNow,
            Status = OfferStatus.Pending
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
    public async Task AddAsync_Should_Create_Offer()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var service = CreateService();

        var dto = new OfferDto
        {
            Id = 0,
            PropertyId = 1,
            UserId = "user1",
            Amount = 1500,
            CreatedAt = DateTime.UtcNow,
            Status = OfferStatus.Pending
        };

        // Act
        var result = await service.AddAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        result.Value!.PropertyId.Should().Be(1);
        result.Value.UserId.Should().Be("user1");
        result.Value.Amount.Should().Be(1500);
        result.Value.Status.Should().Be(OfferStatus.Pending);
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
    public async Task UpdateAsync_Should_Update_Offer()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var entity = new Offer
        {
            Id = 1,
            PropertyId = 1,
            UserId = "oldUser",
            Amount = 1000,
            CreatedAt = DateTime.UtcNow,
            Status = OfferStatus.Pending
        };

        context.Offers.Add(entity);
        await context.SaveChangesAsync();

        var service = CreateService();

        var dto = new OfferDto
        {
            Id = 1,
            PropertyId = 1,
            UserId = "updatedUser",
            Amount = 2500,
            CreatedAt = DateTime.UtcNow,
            Status = OfferStatus.Accepted
        };

        // Act
        var result = await service.UpdateAsync(dto.Id, dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(1);
        result.Value.UserId.Should().Be("updatedUser");
        result.Value.Amount.Should().Be(2500);
        result.Value.Status.Should().Be(OfferStatus.Accepted);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Fail_When_Not_Found()
    {
        // Arrange
        var service = CreateService();

        var dto = new OfferDto
        {
            Id = 999,
            PropertyId = 1,
            UserId = "user",
            Amount = 1000,
            CreatedAt = DateTime.UtcNow,
            Status = OfferStatus.Pending
        };

        // Act
        var result = await service.UpdateAsync(999, dto);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Offer()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var entity = new Offer
        {
            Id = 1,
            PropertyId = 1,
            UserId = "deleteUser",
            Amount = 800,
            CreatedAt = DateTime.UtcNow,
            Status = OfferStatus.Pending
        };

        context.Offers.Add(entity);
        await context.SaveChangesAsync();

        var repository = new OfferRepository(context);
        var service = new OfferService(repository, _mapper);

        // Act
        var result = await service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var exists = await context.Offers.FindAsync(1);
        exists.Should().BeNull();
    }
    

    [Fact]
    public async Task GetAllOffersOfThisClientOnThisProperty_Should_ReturnAll_The_Offers_Of_User()
    {
        var context = new RealStateAppContext(_dbOptions);
        await SeedDependencies(context);

        var entity = new Offer
        {
            Id = 1,
            PropertyId = 1,
            UserId = "deleteUser",
            Amount = 800,
            CreatedAt = DateTime.UtcNow,
            Status = OfferStatus.Pending
        };
        var entity2 = new Offer
        {
            Id = 2,
            PropertyId = 1,
            UserId = "deleteUser",
            Amount = 800,
            CreatedAt = DateTime.UtcNow,
            Status = OfferStatus.Pending
        };
        
    }
}
