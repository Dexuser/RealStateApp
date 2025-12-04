using MediatR;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Improvement.Commands.Update;

public class UpdateImprovementCommand : IRequest<ImprovementApiDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class UpdateImprovementCommandHandler 
    : IRequestHandler<UpdateImprovementCommand, ImprovementApiDto>
{
    private readonly IImprovementRepository _repo;

    public UpdateImprovementCommandHandler(IImprovementRepository repo)
    {
        _repo = repo;
    }

    public async Task<ImprovementApiDto> Handle(UpdateImprovementCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.Id);

        if (entity == null)
            throw new KeyNotFoundException($"No existe una mejora con ID {request.Id}");

        entity.Name = request.Name;
        entity.Description = request.Description;

        await _repo.UpdateAsync(entity.Id, entity);

        return new ImprovementApiDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}