using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Integration.Tests.Persistence.Repositories;

public class ImprovementRepositoryTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;

    public ImprovementRepositoryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"ImprovementRepositoryTestsDb{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task AddAsync_Should_Add_Improvement_To_Database()
    {
        //Arrage
        var improvement = new Improvement
        {
            Id = 0,
            Name = "Balcon",
            Description = "Balcon bonito, que mas tu quieres?" 
        };

        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new ImprovementRepository(context);

        //Act
        var result = await repository.AddAsync(improvement);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AddAsync_Should_Throw_When_Null()
    {
        // Arrange
        using var context = new RealStateAppContext(_dbOptions);
        var repo = new ImprovementRepository(context);

        //Act
        Func<Task> act = async () => await repo.AddAsync(null!);

        //Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetById_Should_Return_Improvement_When_Exists()
    {
        //Arrage
        var improvement = new Improvement
        {
            Id = 0,
            Name = "Balcon",
            Description = "Balcon bonito",
        };
        await using var context = new RealStateAppContext(_dbOptions);
        context.Add(improvement);
        var repository = new ImprovementRepository(context);

        //Act
        var result = await repository.GetByIdAsync(improvement.Id);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("Balcon");
        result.Description.Should().Be("Balcon bonito");
    }


    [Fact]
    public async Task GetById_Should_Return_Null_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new ImprovementRepository(context);

        // Act
        var result = await repository.GetByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Improvement_To_Database()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var improvement = new Improvement
        {
            Id = 0,
            Name = "Balcon",
            Description = "Balcon bonito",
        };
        await context.AddAsync(improvement);
        await context.SaveChangesAsync();
        var repository = new ImprovementRepository(context);

        //Act
        improvement.Description = "Balcon aun mas bonito";
        var updated = await repository.UpdateAsync(improvement.Id, improvement);

        //Assert
        updated.Should().NotBeNull();
        updated.Description.Should().Be("Balcon aun mas bonito");
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Null_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new ImprovementRepository(context);
        var improvement = new Improvement
        {
            Id = 999,
            Name = "Balcon",
            Description = "Balcon bonito"
        };

        //Act
        var updated = await repository.UpdateAsync(improvement.Id, improvement);

        //Assert
        updated.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Improvement()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new ImprovementRepository(context);
        var improvement = new Improvement
        {
            Id = 0,
            Name = "Balcon",
            Description = "Balcon bonito" 
        };
        await context.AddAsync(improvement);

        //Act
        await repository.DeleteAsync(improvement.Id);

        //Assert
        var result = await repository.GetByIdAsync(improvement.Id);
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
    public async Task GetAllList_Should_Return_All_Improvements()
    {
        //Arrange
        using var context = new RealStateAppContext(_dbOptions);
        context.Improvements.AddRange(
            new Improvement
            {
                Id = 0,
                Name = "Balcon",
                Description = "Balcon bonito" 
            },
            new Improvement
            {
                Id = 0,
                Name = "Terraza",
                Description = "Terraza Bonita"
            });
        await context.SaveChangesAsync();
        var repository = new ImprovementRepository(context);

        //Act
        var result = await repository.GetAllAsync();

        //Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllList_Should_Return_Empty_When_No_Improvement_Exists()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new ImprovementRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetQueryable_Should_Return_Queryable_Improvements()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new ImprovementRepository(context);
        var improvement = new Improvement
        {
            Id = 0,
            Name = "Balcon",
            Description = "Balcon bonito",
        };
        context.Add(improvement);
        await context.SaveChangesAsync();

        // Act
        var query = repository.GetAllQueryable();
        var result = await query.ToListAsync();

        // Assert
        result.Should().NotBeEmpty();
    }
}