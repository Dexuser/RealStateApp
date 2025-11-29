using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Integration.Tests.Persistence.Repositories;

public class PropertyImprovementRepositoryTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;

    public PropertyImprovementRepositoryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"PropertyImprovementRepositoryTestsDb{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task AddAsync_Should_Add_PropertyImprovement_To_Database()
    {
        //Arrage
        var propertyImprovement = new PropertyImprovement
        {
            Id = 0,
            PropertyId = 1,
            ImprovementId = 1
        };

        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImprovementRepository(context);

        //Act
        var result = await repository.AddAsync(propertyImprovement);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AddAsync_Should_Throw_When_Null()
    {
        // Arrange
        using var context = new RealStateAppContext(_dbOptions);
        var repo = new PropertyImprovementRepository(context);

        //Act
        Func<Task> act = async () => await repo.AddAsync(null!);

        //Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetById_Should_Return_PropertyImprovement_When_Exists()
    {
        //Arrage
        var propertyImprovement = new PropertyImprovement
        {
            Id = 0,
            PropertyId = 1,
            ImprovementId = 1
        };
        await using var context = new RealStateAppContext(_dbOptions);
        context.Add(propertyImprovement);
        var repository = new PropertyImprovementRepository(context);

        //Act
        var result = await repository.GetByIdAsync(propertyImprovement.Id);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.PropertyId.Should().Be(propertyImprovement.PropertyId);
        result.ImprovementId.Should().Be(propertyImprovement.ImprovementId);
    }


    [Fact]
    public async Task GetById_Should_Return_Null_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImprovementRepository(context);

        // Act
        var result = await repository.GetByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_PropertyImprovement_To_Database()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var propertyImprovement = new PropertyImprovement
        {
            Id = 0,
            PropertyId = 1,
            ImprovementId = 1
        };
        await context.AddAsync(propertyImprovement);
        await context.SaveChangesAsync();
        var repository = new PropertyImprovementRepository(context);

        //Act
        propertyImprovement.ImprovementId = 2;
        var updated = await repository.UpdateAsync(propertyImprovement.Id, propertyImprovement);

        //Assert
        updated.Should().NotBeNull();
        updated.PropertyId.Should().Be(propertyImprovement.PropertyId);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_PropertyImage_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImprovementRepository(context);
        var propertyImprovement = new PropertyImprovement
        {
            Id = 1,
            PropertyId = 1,
            ImprovementId = 1
        };

        //Act
        var updated = await repository.UpdateAsync(propertyImprovement.Id, propertyImprovement);

        //Assert
        updated.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_PropertyImprovement()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImprovementRepository(context);
        var propertyImprovement = new PropertyImprovement
        {
            Id = 0,
            PropertyId = 1,
            ImprovementId = 1
        };
        await context.AddAsync(propertyImprovement);

        //Act
        await repository.DeleteAsync(propertyImprovement.Id);

        //Assert
        var result = await repository.GetByIdAsync(propertyImprovement.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Not_Throw_When_Id_Not_Found()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImprovementRepository(context);

        //Act
        Func<Task> act = async () => await repository.DeleteAsync(999);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task GetAllList_Should_Return_All_PropertyImprovement()
    {
        //Arrange
        using var context = new RealStateAppContext(_dbOptions);
        context.PropertyImprovements.AddRange(
            new PropertyImprovement
            {
                Id = 0,
                PropertyId = 1,
                ImprovementId = 1
            },
            new PropertyImprovement
            {
                Id = 0,
                PropertyId = 1,
                ImprovementId = 2
            });
        await context.SaveChangesAsync();
        var repository = new PropertyImprovementRepository(context);

        //Act
        var result = await repository.GetAllAsync();

        //Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllList_Should_Return_Empty_When_No_PropertyImprovement_Exists()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImprovementRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetQueryable_Should_Return_Queryable_PropertyImprovement()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImprovementRepository(context);
        var propertyImprovement = new PropertyImprovement
        {
            Id = 0,
            PropertyId = 1,
            ImprovementId = 1
        };
        context.Add(propertyImprovement);
        await context.SaveChangesAsync();

        // Act
        var query = repository.GetAllQueryable();
        var result = await query.ToListAsync();

        // Assert
        result.Should().NotBeEmpty();
    }
}