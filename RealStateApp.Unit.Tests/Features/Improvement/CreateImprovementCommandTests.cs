using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Features.Improvement.Commands.Create;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.Improvement;

public class CreateImprovementCommandTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper = null!;

    public CreateImprovementCommandTests()
    {
        _dbOptions = new DbContextOptionsBuilder<RealStateAppContext>()
            .UseInMemoryDatabase($"ImprovementDb_{Guid.NewGuid()}")
            .Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ImprovementDtoMappingProfile>();
            cfg.AddProfile<ImprovementMappingProfile>();
        });
    }

    [Fact]
    public async Task Handle_Should_Create_Improvement()
    {
        using var context = new RealStateAppContext(_dbOptions);
        var repository = new ImprovementRepository(context);
        var handler = new CreateImprovementCommandHandler(repository, _mapper);

        var command = new CreateImprovementCommand
        {
            Name = "Piscina",
            Description = "Piscina grande"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Piscina");

        var dbItem = await context.Improvements.FirstOrDefaultAsync();
        dbItem.Should().NotBeNull();
    }
}