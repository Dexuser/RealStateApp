using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Improvement.Queries.GetAll;

public class GetAllImprovementsQuery : IRequest<List<ImprovementApiDto>> { }

public class GetAllImprovementsQueryHandler 
    : IRequestHandler<GetAllImprovementsQuery, List<ImprovementApiDto>>
{
    private readonly IImprovementRepository _repo;

    public GetAllImprovementsQueryHandler(IImprovementRepository repo, IMapper mapper)
    {
        _repo = repo;
    }

    public async Task<List<ImprovementApiDto>> Handle(GetAllImprovementsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repo.GetAllAsync();

        return entities.Select(i => new ImprovementApiDto
        {
            Id = i.Id,
            Name = i.Name,
            Description = i.Description
        }).ToList();
    }
}