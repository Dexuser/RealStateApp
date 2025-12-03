using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Services;

public class ImprovementServiceTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public ImprovementServiceTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase(databaseName: $"RealStateApp_{Guid.NewGuid()}")
            .Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ImprovementDtoMappingProfile>();
            cfg.AddProfile<ImprovementMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    private ImprovementService CreateService()
    {
        var context = new RealStateAppContext(_dbOptions);
        var repository = new ImprovementRepository(context);
        return new ImprovementService(repository, _mapper);
    }

    [Fact]
    public async Task GetAll_Should_Return_All_Improvements()
    {
        // Arrange
        var service = CreateService();

        var dto1 = new ImprovementDto
        {
            Id = 0,
            Name = "Piscina",
            Description = "Piscina grande"
        };

        var dto2 = new ImprovementDto
        {
            Id = 0,
            Name = "Garaje",
            Description = "Garaje cerrado"
        };

        await service.AddAsync(dto1);
        await service.AddAsync(dto2);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(2);
    }

    [Fact]
    public async Task AddAsync_Should_Create_Improvement()
    {
        // Arrange
        var service = CreateService();

        var dto = new ImprovementDto
        {
            Id = 0,
            Name = "Jardín",
            Description = "Jardín trasero"
        };

        // Act
        var result = await service.AddAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        result.Value!.Name.Should().Be("Jardín");
        result.Value.Description.Should().Be("Jardín trasero");
    }

    [Fact]
    public async Task AddAsync_Should_Return_Failure_When_Dto_Is_Null()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.AddAsync(null!);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Improvement()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);

        var entity = new Improvement
        {
            Id = 1,
            Name = "Viejo nombre",
            Description = "Vieja descripción"
        };

        context.Improvements.Add(entity);
        await context.SaveChangesAsync();

        var service = CreateService();

        var dto = new ImprovementDto
        {
            Id = 1,
            Name = "Nuevo nombre",
            Description = "Nueva descripción"
        };

        // Act
        var result = await service.UpdateAsync(dto.Id, dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(1);
        result.Value.Name.Should().Be("Nuevo nombre");
        result.Value.Description.Should().Be("Nueva descripción");
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Fail_When_Not_Found()
    {
        // Arrange
        var service = CreateService();

        var dto = new ImprovementDto
        {
            Id = 999,
            Name = "No existe",
            Description = "No existe"
        };

        // Act
        var result = await service.UpdateAsync(999, dto);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Improvement()
    {
        // Arrange
        var context = new RealStateAppContext(_dbOptions);

        var entity = new Improvement
        {
            Id = 1,
            Name = "A eliminar",
            Description = "A eliminar"
        };

        context.Improvements.Add(entity);
        await context.SaveChangesAsync();

        var repository = new ImprovementRepository(context);
        var service = new ImprovementService(repository, _mapper);

        // Act
        var result = await service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var exists = await context.Improvements.FindAsync(1);
        exists.Should().BeNull();
    }
}
