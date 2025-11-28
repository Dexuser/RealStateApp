using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Integration.Tests.Persistence.Repositories;

public class OfferRepositoryTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;

    public OfferRepositoryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"OfferRepositoryTestsDb{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task AddAsync_Should_Add_Offer_To_Database()
    {
        //Arrage
        var offer = new Offer
        {
            Id = 0,
            PropertyId = 1,
            UserId = "userid",
            Amount = 5000m,
            CreatedAt = DateTime.Now,
            Status = OfferStatus.Pending
        };

        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new OfferRepository(context);

        //Act
        var result = await repository.AddAsync(offer);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AddAsync_Should_Throw_When_Null()
    {
        // Arrange
        using var context = new RealStateAppContext(_dbOptions);
        var repo = new OfferRepository(context);

        //Act
        Func<Task> act = async () => await repo.AddAsync(null!);

        //Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetById_Should_Return_Offer_When_Exists()
    {
        //Arrage
        var offer = new Offer
        {
            Id = 0,
            PropertyId = 1,
            UserId = "userid",
            Amount = 5000m,
            CreatedAt = DateTime.Now,
            Status = OfferStatus.Pending
        };
        await using var context = new RealStateAppContext(_dbOptions);
        context.Add(offer);
        var repository = new OfferRepository(context);

        //Act
        var result = await repository.GetByIdAsync(offer.Id);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.PropertyId.Should().Be(offer.PropertyId);
        result.UserId.Should().Be(offer.UserId);
        result.Amount.Should().Be(offer.Amount);
        result.CreatedAt.Should().Be(offer.CreatedAt);
        result.Status.Should().Be(offer.Status);
    }


    [Fact]
    public async Task GetById_Should_Return_Null_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new OfferRepository(context);

        // Act
        var result = await repository.GetByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Offer_To_Database()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var offer = new Offer
        {
            Id = 0,
            PropertyId = 1,
            UserId = "userid",
            Amount = 5000m,
            CreatedAt = DateTime.Now,
            Status = OfferStatus.Pending
        };
        await context.AddAsync(offer);
        await context.SaveChangesAsync();
        var repository = new OfferRepository(context);

        //Act
        offer.Amount = 6000m;
        var updated = await repository.UpdateAsync(offer.Id, offer);

        //Assert
        updated.Should().NotBeNull();
        updated.Amount.Should().Be(offer.Amount);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Null_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new OfferRepository(context);
        var offer = new Offer
        {
            Id = 0,
            PropertyId = 1,
            UserId = "userid",
            Amount = 5000m,
            CreatedAt = DateTime.Now,
            Status = OfferStatus.Pending
        };

        //Act
        var updated = await repository.UpdateAsync(offer.Id, offer);

        //Assert
        updated.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Offer()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new OfferRepository(context);
        var offer = new Offer
        {
            Id = 0,
            PropertyId = 1,
            UserId = "userid",
            Amount = 5000m,
            CreatedAt = DateTime.Now,
            Status = OfferStatus.Pending
        };
        await context.AddAsync(offer);

        //Act
        await repository.DeleteAsync(offer.Id);

        //Assert
        var result = await repository.GetByIdAsync(offer.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Not_Throw_When_Id_Not_Found()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new ImprovementRepository(context);

        //Act
        Func<Task> act = async () => await repository.DeleteAsync(999);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task GetAllList_Should_Return_All_Offers()
    {
        //Arrange
        using var context = new RealStateAppContext(_dbOptions);
        context.Offers.AddRange(
            new Offer
            {
                Id = 0,
                PropertyId = 1,
                UserId = "userid1",
                Amount = 5000m,
                CreatedAt = DateTime.Now,
                Status = OfferStatus.Pending
            },
            new Offer
            {
                Id = 0,
                PropertyId = 1,
                UserId = "userid2",
                Amount = 6000m,
                CreatedAt = DateTime.Now,
                Status = OfferStatus.Pending
            });
        await context.SaveChangesAsync();
        var repository = new OfferRepository(context);

        //Act
        var result = await repository.GetAllAsync();

        //Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllList_Should_Return_Empty_When_No_Offers_Exists()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new OfferRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetQueryable_Should_Return_Queryable_Offers()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new OfferRepository(context);
        var offer = new Offer
        {
            Id = 0,
            PropertyId = 1,
            UserId = "userid",
            Amount = 5000m,
            CreatedAt = DateTime.Now,
            Status = OfferStatus.Pending
        };
        context.Add(offer);
        await context.SaveChangesAsync();

        // Act
        var query = repository.GetAllQueryable();
        var result = await query.ToListAsync();

        // Assert
        result.Should().NotBeEmpty();
    }
}