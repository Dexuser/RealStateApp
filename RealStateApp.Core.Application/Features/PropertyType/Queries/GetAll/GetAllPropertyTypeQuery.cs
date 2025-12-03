using MediatR;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.PropertyType.Queries.GetAll;

public class GetAllPropertyTypeQuery : IRequest<List<PropertyTypeApiDto>>
{
}

public class GetAllPropertyTypeQueryHandler : IRequestHandler<GetAllPropertyTypeQuery, List<PropertyTypeApiDto>>
{
    private readonly IPropertyTypeRepository _propertyTypeRepository;

    public GetAllPropertyTypeQueryHandler(IPropertyTypeRepository propertyTypeRepository)
    {
        _propertyTypeRepository = propertyTypeRepository;
    }

    public async Task<List<PropertyTypeApiDto>> Handle(GetAllPropertyTypeQuery request, CancellationToken cancellationToken)
    {
        var propertyTypes = await _propertyTypeRepository.GetAllQueryable().AsNoTracking()
            .Select(p => new PropertyTypeApiDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            }).ToListAsync(cancellationToken);

        return propertyTypes;
    }
}