using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.Improvement.Queries.GetById;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.Improvement;

public class GetImprovementByIdQueryTests
{
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public GetImprovementByIdQueryTests()
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
    public async Task Handle_Should_Return_Improvement_When_Exists()
    {
        using var context = new RealStateAppContext(_dbOptions);

        var improvement = new RealStateApp.Core.Domain.Entities.Improvement
        {
            Id = 1,
            Name = "Piscina",
            Description = "Diversion para los niños"
        };

        context.Improvements.Add(improvement);
        await context.SaveChangesAsync();

        var repository = new ImprovementRepository(context);
        var handler = new GetImprovementByIdQueryHandler(repository, _mapper);

        var result = await handler.Handle(new GetImprovementByIdQuery { Id = 1 }, CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Piscina");
    }

    [Fact]
    public async Task Handle_Should_Throw_When_NotFound()
    {
        using var context = new RealStateAppContext(_dbOptions);

        var repository = new ImprovementRepository(context);
        var handler = new GetImprovementByIdQueryHandler(repository, _mapper);

        Func<Task> act = async () =>
            await handler.Handle(new GetImprovementByIdQuery { Id = 99 }, CancellationToken.None);

        await act.Should().ThrowAsync<ApiException>()
            .WithMessage("Improvement not found");
    }
}