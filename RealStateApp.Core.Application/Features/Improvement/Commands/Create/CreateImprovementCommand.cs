using MediatR;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Improvement.Commands.Create;

public class CreateImprovementCommand : IRequest<ImprovementApiDto>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CreateImprovementCommandHandler 
    : IRequestHandler<CreateImprovementCommand, ImprovementApiDto>
{
    private readonly IImprovementRepository _repo;

    public CreateImprovementCommandHandler(IImprovementRepository repo)
    {
        _repo = repo;
    }

    public async Task<ImprovementApiDto> Handle(CreateImprovementCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Improvement
        {
            Name = request.Name,
            Description = request.Description
        };

        await _repo.AddAsync(entity);

        return new ImprovementApiDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}