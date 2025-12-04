using System.Diagnostics.CodeAnalysis;
using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Features.PropertyType.Commands.DeleteCommand;

/// <summary>
/// Parameters required to delete a Property type.
/// </summary>
public class DeletePropertyTypeCommand : IRequest<Unit>
{
    /// <example>10</example>
    [SwaggerParameter(Description = "The Id of the property type to delete")]
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