using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Features.SaleType.Queries.GetAll;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.SalesType;

public class GetAllSaleTypesQueryTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public GetAllSaleTypesQueryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"SaleTypeDb_GetAll_{Guid.NewGuid()}")
            .Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<SaleTypeDtoMappingProfile>();
            cfg.AddProfile<SaleTypeMappingProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_Return_All_SaleTypes()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);

        context.SaleTypes.AddRange(
            new Core.Domain.Entities.SaleType { Id = 1, Name = "Venta", Description = "Venta regular" },
            new Core.Domain.Entities.SaleType { Id = 2, Name = "Alquiler", Description = "Pago mensual" }
        );
        await context.SaveChangesAsync();

        var repository = new SaleTypeRepository(context);
        var handler = new GetAllSaleTypesQueryHandler(repository, _mapper);

        // Act
        var result = await handler.Handle(new GetAllSaleTypesQuery(), default);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeAssignableTo<List<SaleTypeApiDto>>();
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_SaleTypes()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);
        var repository = new SaleTypeRepository(context);
        var handler = new GetAllSaleTypesQueryHandler(repository, _mapper);

        // Act
        var result = await handler.Handle(new GetAllSaleTypesQuery(), default);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}