using MediatR;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Improvement.Queries.GetById;

public class GetImprovementByIdQuery : IRequest<ImprovementApiDto?>
{
    public int Id { get; set; }
}

public class GetImprovementByIdQueryHandler 
    : IRequestHandler<GetImprovementByIdQuery, ImprovementApiDto?>
{
    private readonly IImprovementRepository _repo;

    public GetImprovementByIdQueryHandler(IImprovementRepository repo)
    {
        _repo = repo;
    }

    public async Task<ImprovementApiDto?> Handle(GetImprovementByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.Id);

        if (entity == null)
            return null;

        return new ImprovementApiDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}