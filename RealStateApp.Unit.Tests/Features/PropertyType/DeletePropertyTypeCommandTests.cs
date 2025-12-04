using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.PropertyType.Commands.DeleteCommand;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.PropertyType;

public class DeletePropertyTypeCommandTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;

    public DeletePropertyTypeCommandTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: $"Db_{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task Handle_Should_Delete_PropertyType_When_Exists()
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

        var command = new DeletePropertyTypeCommand
        {
            Id = propertyType.Id
        };

        var handler = new DeletePropertyTypeCommandHandler(repository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(MediatR.Unit.Value);
        var createdEntity = await repository.GetByIdAsync(propertyType.Id);
        createdEntity.Should().BeNull();

    }


    [Fact]
    public async Task Handle_Should_Throw_When_PropertyType_Doest_Exist()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await context.SaveChangesAsync();
        var repository = new PropertyTypeRepository(context);

        var command = new DeletePropertyTypeCommand { Id = 999 };
        var handler = new DeletePropertyTypeCommandHandler(repository);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ApiException>().WithMessage("property type not found");
    }
}