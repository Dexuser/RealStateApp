using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Exceptions;
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

    public GetImprovementByIdQueryHandler(IImprovementRepository repo, IMapper mapper)
    {
        _repo = repo;
    }

    public async Task<ImprovementApiDto?> Handle(GetImprovementByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.Id);

        if (entity == null)
            throw new ApiException("Improvement not found");

        return new ImprovementApiDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}