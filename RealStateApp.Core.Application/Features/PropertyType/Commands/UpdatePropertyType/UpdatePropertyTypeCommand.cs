using MediatR;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Features.PropertyType.Commands.UpdatePropertyType;

/// <summary>
/// Parameters required to update an existing property type record
/// </summary>
public class UpdatePropertyTypeCommand : IRequest<Unit>
{
    /// <example>10</example>
    [SwaggerParameter(Description = "The ID of the property type record to update.")]
    public int Id { get; set; }
    
    /// <example>Apartamento</example>
    [SwaggerParameter(Description = "The updated name of the property type record to update.")]
    public required string Name { get; set; }
    
    /// <example>Apartamento</example>
    [SwaggerParameter(Description = "The updated description of the property type record to update.")]
    public required string Description { get; set; }
}

public class CreatePropertyTypeCommandHandler : IRequestHandler<UpdatePropertyTypeCommand, Unit>
{
    private readonly IPropertyTypeRepository _propertyTypeRepository;

    public CreatePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository)
    {
        _propertyTypeRepository = propertyTypeRepository;
    }


    public async Task<Unit> Handle(UpdatePropertyTypeCommand request, CancellationToken cancellationToken)
    {
        var propertyType = new Domain.Entities.PropertyType
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description
        };
        var result = await _propertyTypeRepository.UpdateAsync(propertyType.Id, propertyType);
        return Unit.Value;
    }
}