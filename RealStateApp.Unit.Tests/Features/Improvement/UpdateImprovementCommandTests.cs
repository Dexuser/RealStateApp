using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Features.Improvement.Commands.Update;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.Improvement;

public class UpdateImprovementCommandTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public UpdateImprovementCommandTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"ImprovementDb_{Guid.NewGuid()}")
            .Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ImprovementDtoMappingProfile>();
            cfg.AddProfile<ImprovementMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_Update_Improvement()
    {
        using var context = new RealStateAppContext(_dbOptions);

        var improvement = new RealStateApp.Core.Domain.Entities.Improvement
        {
            Id = 1,
            Name = "Piscina",
            Description = "Piscinon"
        };

        context.Improvements.Add(improvement);
        await context.SaveChangesAsync();

        var repository = new ImprovementRepository(context);
        var handler = new UpdateImprovementCommandHandler(repository, _mapper);

        var command = new UpdateImprovementCommand
        {
            Id = 1,
            Name = "Piscina Actualizada",
            Description = "Nueva descripción"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Piscina Actualizada");

        var dbItem = await context.Improvements.FindAsync(1);
        dbItem!.Name.Should().Be("Piscina Actualizada");
    }
}