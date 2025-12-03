using MediatR;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.PropertyType.Commands.DeleteCommand;

public class DeletePropertyTypeCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}

public class DeletePropertyTypeCommandHandler : IRequestHandler<DeletePropertyTypeCommand, Unit>
{
    private readonly IPropertyTypeRepository _propertyTypeRepository;

    public DeletePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository)
    {
        _propertyTypeRepository = propertyTypeRepository;
    }

    public async Task<Unit> Handle(DeletePropertyTypeCommand request, CancellationToken cancellationToken)
    {
        await _propertyTypeRepository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}