using System.Diagnostics.CodeAnalysis;
using MediatR;
using RealStateApp.Core.Application.Exceptions;
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
        var propertyType = await _propertyTypeRepository.GetByIdAsync(request.Id);
        if (propertyType == null) throw new ApiException("property type not found");
        
        await _propertyTypeRepository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}