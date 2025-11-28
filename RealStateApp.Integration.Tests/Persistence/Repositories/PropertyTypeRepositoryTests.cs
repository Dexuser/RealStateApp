using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Integration.Tests.Persistence.Repositories;

public class PropertyTypeRepositoryTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;

    public PropertyTypeRepositoryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"PropertyTypeRepositoryTestsDb{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task AddAsync_Should_Add_PropertyType_To_Database()
    {
        //Arrage
        var propertyType = new PropertyType
        {
            Id = 0,
            Name = "Apartamento",
            Description = "Apartamento duplex",
        };

        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyTypeRepository(context);

        //Act
        var result = await repository.AddAsync(propertyType);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AddAsync_Should_Throw_When_Null()
    {
        // Arrange
        using var context = new RealStateAppContext(_dbOptions);
        var repo = new PropertyTypeRepository(context);

        //Act
        Func<Task> act = async () => await repo.AddAsync(null!);

        //Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetById_Should_Return_Property_When_Exists()
    {
        //Arrage
        var propertyType = new PropertyType
        {
            Id = 0,
            Name = "Apartamento",
            Description = "Apartamento duplex",
        };
        await using var context = new RealStateAppContext(_dbOptions);
        context.Add(propertyType);
        var repository = new PropertyTypeRepository(context);

        //Act
        var result = await repository.GetByIdAsync(propertyType.Id);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("Apartamento");
        result.Description.Should().Be("Apartamento duplex");
    }


    [Fact]
    public async Task GetById_Should_Return_Null_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyTypeRepository(context);

        // Act
        var result = await repository.GetByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_PropertyType_To_Database()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var propertyType = new PropertyType
        {
            Id = 0,
            Name = "Apartamento",
            Description = "Apartamento duplex",
        };
        await context.AddAsync(propertyType);
        await context.SaveChangesAsync();
        var repository = new PropertyTypeRepository(context);

        //Act
        propertyType.Name = "Apartamento2";
        var updated = await repository.UpdateAsync(propertyType.Id, propertyType);

        //Assert
        updated.Should().NotBeNull();
        updated.Name.Should().Be(propertyType.Name);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_PropertyType_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyTypeRepository(context);
        var propertyType = new PropertyType
        {
            Id = 1,
            Name = "Apartamento",
            Description = "Apartamento duplex",
        };

        //Act
        var updated = await repository.UpdateAsync(propertyType.Id, propertyType);

        //Assert
        updated.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_PropertyType()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyTypeRepository(context);
        var propertyType = new PropertyType
        {
            Id = 0,
            Name = "Apartamento",
            Description = "Apartamento duplex",
        };
        await context.AddAsync(propertyType);

        //Act
        await repository.DeleteAsync(propertyType.Id);

        //Assert
        var result = await repository.GetByIdAsync(propertyType.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Not_Throw_When_Id_Not_Found()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyTypeRepository(context);

        //Act
        Func<Task> act = async () => await repository.DeleteAsync(999);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task GetAllList_Should_Return_All_PropertyTypes()
    {
        //Arrange
        using var context = new RealStateAppContext(_dbOptions);
        context.PropertyTypes.AddRange(
            new PropertyType
            {
                Id = 0,
                Name = "Apartamento",
                Description = "Apartamento duplex",
            },
            new PropertyType
            {
                Id = 0,
                Name = "Casa de campo",
                Description = "Casa de campo bonita compai",
            });
        await context.SaveChangesAsync();
        var repository = new PropertyTypeRepository(context);

        //Act
        var result = await repository.GetAllAsync();

        //Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllList_Should_Return_Empty_When_No_PropertyType_Exists()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyTypeRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetQueryable_Should_Return_Queryable_PropertyType()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyTypeRepository(context);
        var propertyType = new PropertyType
        {
            Id = 0,
            Name = "Apartamento",
            Description = "Apartamento duplex",
        };
        context.Add(propertyType);
        await context.SaveChangesAsync();

        // Act
        var query = repository.GetAllQueryable();
        var result = await query.ToListAsync();

        // Assert
        result.Should().NotBeEmpty();
    }
}