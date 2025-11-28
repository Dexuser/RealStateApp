using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Integration.Tests.Persistence.Repositories;

public class SaleTypeRepositoryTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;

    public SaleTypeRepositoryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"SaleTypeRepositoryTestsDb{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task AddAsync_Should_Add_SaleType_To_Database()
    {
        //Arrage
        var saleType = new SaleType
        {
            Id = 0,
            Name = "Alquiler",
            Description = "Alquiler quincenal",
        };

        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new SaleTypeRepository(context);

        //Act
        var result = await repository.AddAsync(saleType);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AddAsync_Should_Throw_When_Null()
    {
        // Arrange
        using var context = new RealStateAppContext(_dbOptions);
        var repo = new SaleTypeRepository(context);

        //Act
        Func<Task> act = async () => await repo.AddAsync(null!);

        //Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetById_Should_Return_SaleType_When_Exists()
    {
        //Arrage
        var saleType = new SaleType
        {
            Id = 0,
            Name = "Alquiler",
            Description = "Alquiler quincenal",
        };
        await using var context = new RealStateAppContext(_dbOptions);
        context.Add(saleType);
        var repository = new SaleTypeRepository(context);

        //Act
        var result = await repository.GetByIdAsync(saleType.Id);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("Alquiler");
        result.Description.Should().Be("Alquiler quincenal");
    }


    [Fact]
    public async Task GetById_Should_Return_Null_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new SaleTypeRepository(context);

        // Act
        var result = await repository.GetByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_SaleType_To_Database()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var saleType = new SaleType
        {
            Id = 0,
            Name = "Alquiler",
            Description = "Alquiler quincenal",
        };
        await context.AddAsync(saleType);
        await context.SaveChangesAsync();
        var repository = new SaleTypeRepository(context);

        //Act
        saleType.Name = "Alquiler2";
        var updated = await repository.UpdateAsync(saleType.Id, saleType);

        //Assert
        updated.Should().NotBeNull();
        updated.Name.Should().Be(saleType.Name);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_SaleType_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new SaleTypeRepository(context);
        var saleType = new SaleType
        {
            Id = 1,
            Name = "Alquiler",
            Description = "Alquiler quincenal",
        };

        //Act
        var updated = await repository.UpdateAsync(saleType.Id, saleType);

        //Assert
        updated.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_SaleType()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new SaleTypeRepository(context);
        var saleType = new SaleType
        {
            Id = 0,
            Name = "Alquiler",
            Description = "Alquiler quincenal",
        };
        await context.AddAsync(saleType);

        //Act
        await repository.DeleteAsync(saleType.Id);

        //Assert
        var result = await repository.GetByIdAsync(saleType.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Not_Throw_When_Id_Not_Found()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new SaleTypeRepository(context);

        //Act
        Func<Task> act = async () => await repository.DeleteAsync(999);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task GetAllList_Should_Return_All_SaleTypes()
    {
        //Arrange
        using var context = new RealStateAppContext(_dbOptions);
        context.SaleTypes.AddRange(
            new SaleType
            {
                Id = 0,
                Name = "Alquiler",
                Description = "Alquiler quincenal",
            },
            new SaleType
            {
                Id = 0,
                Name = "Compra amueblada",
                Description = "esta casa tiene muebles",
            });
        await context.SaveChangesAsync();
        var repository = new SaleTypeRepository(context);

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
        var repository = new SaleTypeRepository(context);

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
        var repository = new SaleTypeRepository(context);
        var saleType = new SaleType
        {
            Id = 0,
            Name = "Alquiler",
            Description = "Alquiler quincenal",
        };
        context.Add(saleType);
        await context.SaveChangesAsync();

        // Act
        var query = repository.GetAllQueryable();
        var result = await query.ToListAsync();

        // Assert
        result.Should().NotBeEmpty();
    }
}