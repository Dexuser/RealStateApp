using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Features.PropertyType.Queries.GetAll;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.PropertyType;

public class GetAllPropertyTypeQueryTests
{
    
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;

    public GetAllPropertyTypeQueryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: $"Db_{Guid.NewGuid()}")
            .Options;
    }
    
    [Fact]
    public async Task Handle_Should_return_all_propertiesTypes()
    
    {
        var context = new RealStateAppContext(_dbOptions);
        var houseType = new Core.Domain.Entities.PropertyType
        {
            Id = 1,
            Name = "Casa",
            Description = "Casa bonita"
        };

        var apartmentType = new Core.Domain.Entities.PropertyType
        {
            Id = 2,
            Name = "Departamento",
            Description = "Departamento bonito"
        };

        context.PropertyTypes.AddRange(houseType, apartmentType);
        await context.SaveChangesAsync();
        var repository = new PropertyTypeRepository(context);


        var query = new GetAllPropertyTypeQuery();
        var handler = new GetAllPropertyTypeQueryHandler(repository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task Handle_Should_return_List_Empty_When_No_PropertiesTypes_Exists()
    {
        var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyTypeRepository(context);

        var query = new GetAllPropertyTypeQuery();
        var handler = new GetAllPropertyTypeQueryHandler(repository);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}