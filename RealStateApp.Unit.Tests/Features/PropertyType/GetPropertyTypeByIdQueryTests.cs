using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.PropertyType.Queries.GetById;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.PropertyType;

public class GetPropertyTypeByIdQueryTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;

    public GetPropertyTypeByIdQueryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: $"Db_{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task Handle_Should_return_PropertyType_When_Exists()

    {
        var context = new RealStateAppContext(_dbOptions);
        var houseType = new Core.Domain.Entities.PropertyType
        {
            Id = 1,
            Name = "Casa",
            Description = "Casa bonita"
        };

        context.PropertyTypes.AddRange(houseType);
        await context.SaveChangesAsync();
        var repository = new PropertyTypeRepository(context);


        var query = new GetPropertyTypeByIdQuery { Id = 1 };
        var handler = new GetPropertyTypeByIdQueryHandler(repository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Casa");
    }

    [Fact]
    public async Task Handle_Should_return_List_Empty_When_No_PropertiesTypes_Exists()
    {
        var context = new RealStateAppContext(_dbOptions);
        await context.SaveChangesAsync();
        var repository = new PropertyTypeRepository(context);

        var query = new GetPropertyTypeByIdQuery { Id = 1 };
        var handler = new GetPropertyTypeByIdQueryHandler(repository);

        // Act
        Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ApiException>().WithMessage("PropertyType not found");
    }
}