using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Features.SaleType.Queries.GetById;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.SalesType;

public class GetSaleTypeByIdQueryTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public GetSaleTypeByIdQueryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"SaleTypeDb_GetById_{Guid.NewGuid()}")
            .Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<SaleTypeDtoMappingProfile>();
            cfg.AddProfile<SaleTypeMappingProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_Return_SaleType_When_Exists()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);

        context.SaleTypes.Add(new Core.Domain.Entities.SaleType
        {
            Id = 1,
            Name = "Venta",
            Description = "Venta directa"
        });
        await context.SaveChangesAsync();

        var repository = new SaleTypeRepository(context);
        var handler = new GetSaleTypeByIdQueryHandler(repository, _mapper);

        // Act
        var result = await handler.Handle(new GetSaleTypeByIdQuery { Id = 1 }, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<SaleTypeApiDto>();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Venta");
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        var repository = new SaleTypeRepository(context);
        var handler = new GetSaleTypeByIdQueryHandler(repository, _mapper);

        // Act
        var result = await handler.Handle(new GetSaleTypeByIdQuery { Id = 999 }, default);

        // Assert
        result.Should().BeNull();
    }
}