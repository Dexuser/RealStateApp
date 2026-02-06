using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Features.Improvement.Queries.GetAll;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Infrastructure.Persistence.Contexts;
using RealStateApp.Infrastructure.Persistence.Repositories;

namespace RealStateApp.Unit.Tests.Features.Improvement;

public class GetAllImprovementsQueryTests
{
    
    private readonly DbContextOptions<RealStateAppContext> _dbOptions;
    private readonly IMapper _mapper;

    public GetAllImprovementsQueryTests()
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
    public async Task Handle_Should_Return_All_Improvements()
    {
        using var context = new RealStateAppContext(_dbOptions);

        context.Improvements.AddRange(
            new RealStateApp.Core.Domain.Entities.Improvement
            {
                Id = 1,
                Name = "Piscina",
                Description = "Diversion para los niños"
            },
            new RealStateApp.Core.Domain.Entities.Improvement
            {
                Id = 2,
                Name = "Jardín",
                Description = "Parque de flores"
            }
        );

        await context.SaveChangesAsync();

        var repository = new ImprovementRepository(context);
        var handler = new GetAllImprovementsQueryHandler(repository, _mapper);

        var result = await handler.Handle(new GetAllImprovementsQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Piscina");
    }
}