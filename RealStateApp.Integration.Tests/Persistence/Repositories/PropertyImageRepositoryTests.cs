using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Integration.Tests.Persistence.Repositories;

public class PropertyImageRepositoryTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;

    public PropertyImageRepositoryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"PropertyImageRepositoryTestsDb{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task AddAsync_Should_Add_PropertyImage_To_Database()
    {
        //Arrage
        var propertyImage = new PropertyImage
        {
            Id = 0,
            PropertyId = 1,
            ImagePath = "imagePath",
            IsMain = false
        };

        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImageRepository(context);

        //Act
        var result = await repository.AddAsync(propertyImage);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AddAsync_Should_Throw_When_Null()
    {
        // Arrange
        using var context = new RealStateAppContext(_dbOptions);
        var repo = new PropertyImageRepository(context);

        //Act
        Func<Task> act = async () => await repo.AddAsync(null!);

        //Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetById_Should_Return_PropertyImage_When_Exists()
    {
        //Arrage
        var propertyImage = new PropertyImage
        {
            Id = 0,
            PropertyId = 1,
            ImagePath = "imagePath",
            IsMain = false
        };
        await using var context = new RealStateAppContext(_dbOptions);
        context.Add(propertyImage);
        var repository = new PropertyImageRepository(context);

        //Act
        var result = await repository.GetByIdAsync(propertyImage.Id);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.PropertyId.Should().Be(propertyImage.PropertyId);
        result.ImagePath.Should().Be(propertyImage.ImagePath);
        result.IsMain.Should().Be(propertyImage.IsMain);
    }


    [Fact]
    public async Task GetById_Should_Return_Null_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImageRepository(context);

        // Act
        var result = await repository.GetByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_PropertyImage_To_Database()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var propertyImage = new PropertyImage
        {
            Id = 0,
            PropertyId = 1,
            ImagePath = "imagePath",
            IsMain = false
        };
        await context.AddAsync(propertyImage);
        await context.SaveChangesAsync();
        var repository = new PropertyImageRepository(context);

        //Act
        propertyImage.ImagePath = "imagePath2";
        var updated = await repository.UpdateAsync(propertyImage.Id, propertyImage);

        //Assert
        updated.Should().NotBeNull();
        updated.PropertyId.Should().Be(propertyImage.PropertyId);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_PropertyImage_When_NotExists()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImageRepository(context);
        var propertyImage = new PropertyImage
        {
            Id = 999,
            PropertyId = 1,
            ImagePath = "imagePath",
            IsMain = false
        };

        //Act
        var updated = await repository.UpdateAsync(propertyImage.Id, propertyImage);

        //Assert
        updated.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_PropertyImage()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImageRepository(context);
        var propertyImage = new PropertyImage
        {
            Id = 0,
            PropertyId = 1,
            ImagePath = "imagePath",
            IsMain = false
        };
        await context.AddAsync(propertyImage);

        //Act
        await repository.DeleteAsync(propertyImage.Id);

        //Assert
        var result = await repository.GetByIdAsync(propertyImage.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Not_Throw_When_Id_Not_Found()
    {
        //Arrage
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImageRepository(context);

        //Act
        Func<Task> act = async () => await repository.DeleteAsync(999);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task GetAllList_Should_Return_All_PropertyImages()
    {
        //Arrange
        using var context = new RealStateAppContext(_dbOptions);
        context.PropertyImages.AddRange(
            new PropertyImage
            {
                Id = 0,
                PropertyId = 1,
                ImagePath = "imagePath",
                IsMain = false
            },
            new PropertyImage
            {
                Id = 0,
                PropertyId = 1,
                ImagePath = "imagePath",
                IsMain = false
            });
        await context.SaveChangesAsync();
        var repository = new PropertyImageRepository(context);

        //Act
        var result = await repository.GetAllAsync();

        //Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllList_Should_Return_Empty_When_No_PropertyImages_Exists()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImageRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetQueryable_Should_Return_Queryable_PropertyImages()
    {
        // Arrange
        await using var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyImageRepository(context);
        var propertyImages = new PropertyImage
        {
            Id = 0,
            PropertyId = 1,
            ImagePath = "imagePath",
            IsMain = false
        };
        context.Add(propertyImages);
        await context.SaveChangesAsync();

        // Act
        var query = repository.GetAllQueryable();
        var result = await query.ToListAsync();

        // Assert
        result.Should().NotBeEmpty();
    }
}