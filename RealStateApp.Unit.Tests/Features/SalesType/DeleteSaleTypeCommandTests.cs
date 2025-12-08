using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Features.SaleType.Commands.Delete;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.SalesType;

public class DeleteSaleTypeCommandTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;

    public DeleteSaleTypeCommandTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"SaleTypeDb_Delete_{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task Handle_Should_Delete_SaleType()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);

        context.SaleTypes.Add(new Core.Domain.Entities.SaleType
        {
            Id = 1,
            Name = "Venta",
            Description = "Vieja Descripcion",
        });
        await context.SaveChangesAsync();

        var repository = new SaleTypeRepository(context);
        var handler = new DeleteSaleTypeCommandHandler(repository);

        // Act
        var result = await handler.Handle(new DeleteSaleTypeCommand { Id = 1 }, default);

        // Assert
        result.Should().Be("Tipo de venta eliminado correctamente.");
        (await context.SaleTypes.AnyAsync()).Should().BeFalse();
    }
}