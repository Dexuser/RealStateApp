using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Property.Queries.GetById;

public class GetPropertyByIdQuery : IRequest<PropertyApiDto>
{
    public required int Id { get; set; }
}

public class GetPropertyByIdHandler : IRequestHandler<GetPropertyByIdQuery, PropertyApiDto>
{
    private readonly IBaseAccountService _baseAccountService;
    private readonly IPropertyRepository _repository;

    public GetPropertyByIdHandler(IPropertyRepository repository, IBaseAccountService baseAccountService)
    {
        _repository = repository;
        _baseAccountService = baseAccountService;
    }

    public async Task<PropertyApiDto> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
    {
        var property = await  _repository.GetAllQueryable().AsNoTracking()
            .Include(p => p.SaleType)
            .Include(p => p.PropertyType)
            .Include(p => p.PropertyImprovements).ThenInclude(p => p.Improvement)
            .FirstOrDefaultAsync(p => p.Id == request.Id ,cancellationToken);
        
        if (property == null) throw new ApiException("Property not found", StatusCodes.Status404NotFound);
        var userDto = await _baseAccountService.GetUserById(property.AgentId);
        var dto = new PropertyApiDto
        {
            Id = property.Id,
            Code = property.Code,
            PropertyType = property.PropertyType!.Name,
            SaleType = property.SaleType!.Name,
            Price = property.Price,
            SizeInMeters = property.SizeInMeters,
            Rooms = property.Rooms,
            Bathrooms = property.Bathrooms,
            Description = property.Description,
            CreatedAt = property.CreatedAt,
            PropertyImprovements = property.PropertyImprovements.Select(pi => pi.Improvement!.Name),
            IsAvailable = property.IsAvailable,
            Agent = new SimpleAgentForPropertyApiDto
            {
                Id = userDto!.Id,
                Name = $"{userDto.FirstName} {userDto.LastName}" 
            },
        };
        return dto;
    }
}