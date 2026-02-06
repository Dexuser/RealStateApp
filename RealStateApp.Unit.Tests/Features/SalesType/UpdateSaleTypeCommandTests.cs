using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Features.SaleType.Commands.Update;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.SalesType;

public class UpdateSaleTypeCommandTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public UpdateSaleTypeCommandTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"SaleTypeDb_Update_{Guid.NewGuid()}")
            .Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<SaleTypeDtoMappingProfile>();
            cfg.AddProfile<SaleTypeMappingProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_Update_SaleType()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);

        context.SaleTypes.Add(new Core.Domain.Entities.SaleType
        {
            Id = 1,
            Name = "Venta",
            Description = "Vieja descripción"
        });
        await context.SaveChangesAsync();

        var repository = new SaleTypeRepository(context);
        var handler = new UpdateSaleTypeCommandHandler(repository, _mapper);

        var command = new UpdateSaleTypeCommand
        {
            Id = 1,
            Name = "Venta Actualizada",
            Description = "Nueva descripción"
        };

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Name.Should().Be("Venta Actualizada");
        result.Description.Should().Be("Nueva descripción");
    }
}