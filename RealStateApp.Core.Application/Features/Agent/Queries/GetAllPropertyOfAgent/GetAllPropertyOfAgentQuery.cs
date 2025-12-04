using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Agent.Queries.GetAllPropertyOfAgent;

public class GetAllPropertyOfAgentQuery : IRequest<List<PropertyApiDto>>
{
    public required string Id { get; set; }
}

public class GetAllPropertiesOfAgentQueryHandler : IRequestHandler<GetAllPropertyOfAgentQuery, List<PropertyApiDto>>
{
    private readonly IPropertyRepository _repository;
    private readonly IBaseAccountService _baseAccountService;

    public GetAllPropertiesOfAgentQueryHandler(IPropertyRepository propertyRepository, IBaseAccountService baseAccountService)
    {
        _repository = propertyRepository;
        _baseAccountService = baseAccountService;
    }

    public async Task<List<PropertyApiDto>> Handle(GetAllPropertyOfAgentQuery request, CancellationToken cancellationToken)
    {
        
        var agent = await _baseAccountService.GetUserById(request.Id);

        if (agent == null || agent.Role != nameof(Roles.Agent)) throw new ApiException("Agent not found", StatusCodes.Status404NotFound);
        
        var query = _repository.GetAllQueryable().AsNoTracking();

        var properties = await query
            .Include(p => p.SaleType)
            .Include(p => p.PropertyType)
            .Include(p => p.PropertyImprovements)
            .Where(p => p.AgentId == request.Id)
            .Select(property => new PropertyApiDto
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
                    Id = agent.Id,
                    Name = $"{agent.FirstName} {agent.LastName}" 
                }
            }).ToListAsync(cancellationToken: cancellationToken);

        return properties;
    }
}