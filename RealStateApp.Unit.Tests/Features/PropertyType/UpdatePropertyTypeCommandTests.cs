using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.PropertyType.Commands.UpdatePropertyType;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.PropertyType;

public class UpdatePropertyTypeCommandTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
        
    public UpdatePropertyTypeCommandTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: $"Db_{Guid.NewGuid()}")
            .Options;
    }
    
    [Fact]
    public async Task Handle_Should_Modify_PropertyType_When_Exists()
    {
            
        // Arrange
        var propertyType = new Core.Domain.Entities.PropertyType
        {
            Id = 0,
            Name = "Apartamento",
            Description = "Apartamento descripcion"
        };
        var context = new RealStateAppContext(_dbOptions);
        context.PropertyTypes.Add(propertyType);
        await context.SaveChangesAsync();
        var repository = new PropertyTypeRepository(context);

        var command = new UpdatePropertyTypeCommand
        {
            Id = propertyType.Id,
            Name = "Updated",
            Description = "Updated" 
        };

        var handler = new UpdatePropertyTypeCommandHandler(repository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(MediatR.Unit.Value);
        var updated = await repository.GetByIdAsync(propertyType.Id);
        updated.Should().NotBeNull();
        updated.Id.Should().Be(propertyType.Id);
        updated.Name.Should().Be("Updated");
        updated.Description.Should().Be("Updated");
    }

        
    [Fact]
    public async Task Handle_Should_Thorw_When_PropertyType_Does_Not_Exist()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        var repository = new PropertyTypeRepository(context);

        var command = new UpdatePropertyTypeCommand
        {
            Id = 999,
            Name = "Updated",
            Description = "Updated" 
        };

        var handler = new UpdatePropertyTypeCommandHandler(repository);
            
        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            
        // Assert
        await act.Should().ThrowAsync<ApiException>().WithMessage("property type not found");
    }
}