using MediatR;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.PropertyType.Queries.GetById;

public class GetPropertyTypeByIdQuery : IRequest<PropertyTypeApiDto>
{
    public required int Id { get; set; }
}

public class GetPropertyTypeByIdQueryHandler : IRequestHandler<GetPropertyTypeByIdQuery, PropertyTypeApiDto>
{
    private readonly IPropertyTypeRepository _propertyTypeRepository;

    public GetPropertyTypeByIdQueryHandler(IPropertyTypeRepository propertyTypeRepository)
    {
        _propertyTypeRepository = propertyTypeRepository;
    }

    public async Task<PropertyTypeApiDto> Handle(GetPropertyTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var propertyType = await _propertyTypeRepository.GetByIdAsync(request.Id);
        
        if (propertyType == null) throw new ApiException("Property not found", StatusCodes.Status404NotFound);

        return new PropertyTypeApiDto
        {
            Id = propertyType.Id,
            Name = propertyType.Name,
            Description = propertyType.Description,
        };
    }
}