using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Integration.Tests.Persistence.Repositories;

public class PropertyRepositoryTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;

    public PropertyRepositoryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"PropertyRepositoryTestsDb{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task AddAsync_Should_Add_Property_To_Database()
    {
        //Arrage
        var property = new Property
        {
            Id = 0,
            Code = "000001",
            PropertyTypeId = 1,
            SaleTypeId = 1,
            Price = 5000,
            SizeInMeters = 25.5d,
            Rooms = 3,
            Bathrooms = 2,
            Description = "Casa bonita",
            AgentId = "userid",
            CreatedAt = DateTime.Now 
        };

        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyRepository(context);

        //Act
        var result = await repository.AddAsync(property);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AddAsync_Should_Throw_When_Null()
    {
        // Arrange
        using var context = new RealStateAppContext(_dbOptions);
        var repo = new PropertyRepository(context);

        //Act
        Func<Task> act = async () => await repo.AddAsync(null!);

        //Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetById_Should_Return_Property_When_Exists()
    {
        //Arrage
        var property = new Property
        {
            Id = 0,
            Code = "000001",
            PropertyTypeId = 1,
            SaleTypeId = 1,
            Price = 5000,
            SizeInMeters = 25.5d,
            Rooms = 3,
            Bathrooms = 2,
            Description = "Casa bonita",
            AgentId = "userid",
            CreatedAt = new DateTime(2025, 11, 28) 
        };
        await using var context = new RealStateAppContext(_dbOptions);
        context.Add(property);
        var repository = new PropertyRepository(context);

        //Act
        var result = await repository.GetByIdAsync(property.Id);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Code.Should().Be("000001");
        result.PropertyTypeId.Should().Be(1);
        result.SaleTypeId.Should().Be(1);
        result.Price.Should().Be(5000);
        result.SizeInMeters.Should().Be(25.5d);
        result.Rooms.Should().Be(3);
        result.Bathrooms.Should().Be(2);
        result.Description.Should().Be("Casa bonita");
        result.AgentId.Should().Be("userid");
        result.CreatedAt.Should().Be(new DateTime(2025, 11, 28));
    }


    [Fact]
    public async Task GetById_Should_Return_Null_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyRepository(context);

        // Act
        var result = await repository.GetByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Property_To_Database()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var property = new Property
        {
            Id = 0,
            Code = "000001",
            PropertyTypeId = 1,
            SaleTypeId = 1,
            Price = 5000,
            SizeInMeters = 25.5d,
            Rooms = 3,
            Bathrooms = 2,
            Description = "Casa bonita",
            AgentId = "userid",
            CreatedAt = DateTime.Now
            
        };
        await context.AddAsync(property);
        await context.SaveChangesAsync();
        var repository = new PropertyRepository(context);

        //Act
        property.Price = 30;
        var updated = await repository.UpdateAsync(property.Id, property);

        //Assert
        updated.Should().NotBeNull();
        updated.Price.Should().Be(30);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Property_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyRepository(context);
        var property = new Property
        {
            Id = 1,
            Code = "000001",
            PropertyTypeId = 1,
            SaleTypeId = 1,
            Price = 5000,
            SizeInMeters = 25.5d,
            Rooms = 3,
            Bathrooms = 2,
            Description = "Casa bonita",
            AgentId = "userid",
            CreatedAt = DateTime.Now
        };

        //Act
        var updated = await repository.UpdateAsync(property.Id, property);

        //Assert
        updated.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Property()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyRepository(context);
        var property = new Property
        {
            Id = 0,
            Code = "000001",
            PropertyTypeId = 1,
            SaleTypeId = 1,
            Price = 5000,
            SizeInMeters = 25.5d,
            Rooms = 3,
            Bathrooms = 2,
            Description = "Casa bonita",
            AgentId = "userid",
            CreatedAt = DateTime.Now
        };
        await context.AddAsync(property);

        //Act
        await repository.DeleteAsync(property.Id);

        //Assert
        var result = await repository.GetByIdAsync(property.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Not_Throw_When_Id_Not_Found()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyRepository(context);

        //Act
        Func<Task> act = async () => await repository.DeleteAsync(999);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task GetAllList_Should_Return_All_Property()
    {
        //Arrange
        using var context = new RealStateAppContext(_dbOptions);
        context.Properties.AddRange(
            new Property
            {
                Id = 0,
                Code = "000001",
                PropertyTypeId = 1,
                SaleTypeId = 1,
                Price = 5000,
                SizeInMeters = 25.5d,
                Rooms = 3,
                Bathrooms = 2,
                Description = "Casa bonita",
                AgentId = "userid1",
                CreatedAt = DateTime.Now
            },
            new Property
            {
                Id = 0,
                Code = "000002",
                PropertyTypeId = 2,
                SaleTypeId = 2,
                Price = 7000,
                SizeInMeters = 30,
                Rooms = 2,
                Bathrooms = 1,
                Description = "Casa linda",
                AgentId = "userid2",
                CreatedAt = DateTime.Now
            });
        await context.SaveChangesAsync();
        var repository = new PropertyRepository(context);

        //Act
        var result = await repository.GetAllAsync();

        //Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllList_Should_Return_Empty_When_No_Property_Exists()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetQueryable_Should_Return_Queryable_Property()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyRepository(context);
        var property = new Property
        {
            Id = 0,
            Code = "000001",
            PropertyTypeId = 1,
            SaleTypeId = 1,
            Price = 5000,
            SizeInMeters = 25.5d,
            Rooms = 3,
            Bathrooms = 2,
            Description = "Casa bonita",
            AgentId = "userid",
            CreatedAt = DateTime.Now
        };
        context.Add(property);
        await context.SaveChangesAsync();

        // Act
        var query = repository.GetAllQueryable();
        var result = await query.ToListAsync();

        // Assert
        result.Should().NotBeEmpty();
    }
}