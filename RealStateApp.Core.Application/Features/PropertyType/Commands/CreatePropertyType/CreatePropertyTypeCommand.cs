using MediatR;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Features.PropertyType.Commands.CreatePropertyType;

/// <summary>
/// Parameters required to create a new property type record
/// </summary>
public class CreatePropertyTypeCommand : IRequest<int>
{
    /// <example>Apartamento</example>
    [SwaggerParameter(Description = "The name of the property type")]
    public required string Name { get; set; }
    
    /// <example>Apartamento muy bonito</example>
    [SwaggerParameter(Description = "The description of the property type")]
    public required string Description { get; set; }
}

public class CreatePropertyTypeCommandHandler : IRequestHandler<CreatePropertyTypeCommand, int>
{
    private readonly IPropertyTypeRepository _propertyTypeRepository;

    public CreatePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository)
    {
        _propertyTypeRepository = propertyTypeRepository;
    }


    public async Task<int> Handle(CreatePropertyTypeCommand request, CancellationToken cancellationToken)
    {
        var propertyType = new Domain.Entities.PropertyType
        {
            Id = 0,
            Name = request.Name,
            Description = request.Description
        };
        var result = await _propertyTypeRepository.AddAsync(propertyType);
        return result?.Id ?? throw new ApiException("Error creating property type", StatusCodes.Status422UnprocessableEntity);
    }
}