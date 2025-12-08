using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Features.Improvement.Commands.Delete;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.Improvement;

public class DeleteImprovementCommandTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;

    public DeleteImprovementCommandTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"Db_DeleteImprovement_{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task Handle_Should_Delete_Improvement()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);

        context.Improvements.Add(new RealStateApp.Core.Domain.Entities.Improvement
        {
            Id = 1,
            Name = "Piscina",
            Description = "Mejora de lujo"
        });

        await context.SaveChangesAsync();

        var repo = new ImprovementRepository(context);
        var handler = new DeleteImprovementCommandHandler(repo);

        var command = new DeleteImprovementCommand { Id = 1 };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        var deletedEntity = await context.Improvements.FindAsync(1);
        deletedEntity.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Not_Exists()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        await context.SaveChangesAsync();

        var repo = new ImprovementRepository(context);
        var handler = new DeleteImprovementCommandHandler(repo);

        var command = new DeleteImprovementCommand { Id = 999 };

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("La mejora con ID 999 no existe.");
    }
}