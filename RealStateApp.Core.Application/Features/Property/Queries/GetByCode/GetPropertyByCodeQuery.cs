using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.Property.Queries.GetById;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Property.Queries.GetByCode;

public class GetPropertyByCodeQuery : IRequest<PropertyApiDto>
{
    public required string Code { get; set; }
}

public class GetPropertyByCodeHandler : IRequestHandler<GetPropertyByCodeQuery, PropertyApiDto>
{
    private readonly IBaseAccountService  _baseAccountService;
    private readonly IPropertyRepository _repository;
    private readonly IMapper _mapper;

    public GetPropertyByCodeHandler(IPropertyRepository repository, IMapper mapper, IBaseAccountService baseAccountService)
    {
        _repository = repository;
        _mapper = mapper;
        _baseAccountService = baseAccountService;
    }

    public async Task<PropertyApiDto> Handle(GetPropertyByCodeQuery request, CancellationToken cancellationToken)
    {
        var property = await  _repository.GetAllQueryable().AsNoTracking()
            .Include(p => p.SaleType)
            .Include(p => p.PropertyType)
            .Include(p => p.PropertyImprovements).ThenInclude(p => p.Improvement)
            .FirstOrDefaultAsync(p => p.Code == request.Code,cancellationToken);
        
        if (property == null) throw new ApiException("Property not found", StatusCodes.Status404NotFound);
        var userDto = await _baseAccountService.GetUserById(property.AgentId);
        var dto = new PropertyApiDto
        {
            Id = property.Id,
            Code = property.Code,
            PropertyType = property.PropertyType.Name,
            SaleType = property.SaleType.Name,
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
                Id = userDto.Id,
                Name = $"{userDto.FirstName} {userDto.LastName}" 
            },
        };
            
        return dto;
    }
}