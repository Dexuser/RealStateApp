using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Features.SaleType.Commands.Create;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.SalesType;

public class CreateSaleTypeCommandTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public CreateSaleTypeCommandTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"SaleTypeDb_Create_{Guid.NewGuid()}")
            .Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<SaleTypeDtoMappingProfile>();
            cfg.AddProfile<SaleTypeMappingProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_Create_New_SaleType()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        var repository = new SaleTypeRepository(context);
        var handler = new CreateSaleTypeCommandHandler(repository, _mapper);

        var command = new CreateSaleTypeCommand
        {
            Name = "Renta",
            Description = "Pago mensual"
        };

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<SaleTypeApiDto>();
        result.Name.Should().Be("Renta");

        var saved = await context.SaleTypes.FirstAsync();
        saved.Description.Should().Be("Pago mensual");
    }

}