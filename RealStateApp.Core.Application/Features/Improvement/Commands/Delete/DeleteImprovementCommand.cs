using MediatR;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Improvement.Commands.Delete;

public class DeleteImprovementCommand : IRequest<bool>
{
    public int Id { get; set; }
}

public class DeleteImprovementCommandHandler 
    : IRequestHandler<DeleteImprovementCommand, bool>
{
    private readonly IImprovementRepository _repo;

    public DeleteImprovementCommandHandler(IImprovementRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(DeleteImprovementCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.Id);

        if (entity == null)
            throw new KeyNotFoundException($"La mejora con ID {request.Id} no existe.");

        await _repo.DeleteAsync(entity.Id);
        return true;
    }
}