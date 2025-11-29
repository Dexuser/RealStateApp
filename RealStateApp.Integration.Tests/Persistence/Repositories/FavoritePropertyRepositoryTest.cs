using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Integration.Tests.Persistence.Repositories;

public class FavoritePropertyRepositoryTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;

    public FavoritePropertyRepositoryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"FavoritePropertyDb{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task AddAsync_Should_Add_FavoriteProperty_To_Database()
    {
        //Arrage
        var favoriteProperty = new FavoriteProperty
        {
            Id = 0,
            UserId = "userid",
            PropertyId = 1
        };

        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new FavoritePropertyRepository(context);

        //Act
        var result = await repository.AddAsync(favoriteProperty);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AddAsync_Should_Throw_When_Null()
    {
        // Arrange
        using var context = new RealStateAppContext(_dbOptions);
        var repo = new FavoritePropertyRepository(context);

        //Act
        Func<Task> act = async () => await repo.AddAsync(null!);

        //Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetById_Should_Return_FavoriteProperty_When_Exists()
    {
        //Arrage
        var favoriteProperty = new FavoriteProperty
        {
            Id = 0,
            UserId = "userid",
            PropertyId = 1,
        };
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new FavoritePropertyRepository(context);
        context.FavoriteProperties.Add(favoriteProperty);

        //Act
        var result = await repository.GetByIdAsync(favoriteProperty.Id);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.UserId.Should().Be("userid");
        result.PropertyId.Should().Be(1);
    }


    [Fact]
    public async Task GetById_Should_Return_Null_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new FavoritePropertyRepository(context);

        // Act
        var result = await repository.GetByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_FavoriteProperty_To_Database()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var favoriteProperty = new FavoriteProperty
        {
            Id = 0,
            PropertyId = 1,
            UserId = "userid"
        };
        await context.FavoriteProperties.AddAsync(favoriteProperty);
        await context.SaveChangesAsync();
        var repository = new FavoritePropertyRepository(context);

        //Act
        favoriteProperty.PropertyId = 2;
        var updated = await repository.UpdateAsync(favoriteProperty.Id, favoriteProperty);

        //Assert
        updated.Should().NotBeNull();
        updated.PropertyId.Should().Be(2);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Null_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new FavoritePropertyRepository(context);
        var favoriteProperty = new FavoriteProperty
        {
            Id = 999,
            PropertyId = 1,
            UserId = "userid"
        };

        //Act
        var updated = await repository.UpdateAsync(favoriteProperty.Id, favoriteProperty);

        //Assert
        updated.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_FavoriteProperty()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new FavoritePropertyRepository(context);
        var favoriteProperty = new FavoriteProperty
        {
            Id = 0,
            PropertyId = 1,
            UserId = "userid"
        };
        await context.AddAsync(favoriteProperty);

        //Act
        await repository.DeleteAsync(favoriteProperty.Id);

        //Assert
        var result = await repository.GetByIdAsync(favoriteProperty.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Not_Throw_When_Id_Not_Found()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new FavoritePropertyRepository(context);

        //Act
        Func<Task> act = async () => await repository.DeleteAsync(999);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task GetAllList_Should_Return_All_FavoriteProperties()
    {
        //Arrange
        using var context = new RealStateAppContext(_dbOptions);
        context.FavoriteProperties.AddRange(
            new FavoriteProperty
            {
                Id = 0,
                UserId = "userid",
                PropertyId = 1
            },
            new FavoriteProperty
            {
                Id = 0,
                UserId = "userid",
                PropertyId = 2
            });
        await context.SaveChangesAsync();
        var repository = new FavoritePropertyRepository(context);

        //Act
        var result = await repository.GetAllAsync();

        //Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllList_Should_Return_Empty_When_No_AssetTypes()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new FavoritePropertyRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetQueryable_Should_Return_Queryable_ChatMessages()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new FavoritePropertyRepository(context);
        var favoriteProperty = new FavoriteProperty
        {
            Id = 0,
            PropertyId = 1,
            UserId = "userid",
        };
        context.Add(favoriteProperty);
        await context.SaveChangesAsync();

        // Act
        var query = repository.GetAllQueryable();
        var result = await query.ToListAsync();

        // Assert
        result.Should().NotBeEmpty();
    }
}