using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Property.Queries.GetAll;

public class GetAllPropertyQuery : IRequest<IList<PropertyApiDto>>
{
}

public class GetAllPropertyQueryHandler : IRequestHandler<GetAllPropertyQuery, IList<PropertyApiDto>>
{
    private readonly IBaseAccountService _baseAccountService;
    private readonly IPropertyRepository _repository;
    private readonly IMapper _mapper;

    public GetAllPropertyQueryHandler(IPropertyRepository repository, IMapper mapper,
        IBaseAccountService baseAccountService)
    {
        _repository = repository;
        _mapper = mapper;
        _baseAccountService = baseAccountService;
    }

    public async Task<IList<PropertyApiDto>> Handle(GetAllPropertyQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetAllQueryable().AsNoTracking();
        var agentsIds = query.Select(p => p.AgentId).Distinct().ToList();
        var agentsDict = (await _baseAccountService.GetUsersByIds(agentsIds)).ToDictionary(a => a.Id, a => a);

        var properties = await query
            .Include(p => p.SaleType)
            .Include(p => p.PropertyType)
            .Include(p => p.PropertyImprovements)
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
                    Id = property.AgentId,
                    Name = "" 
                }
            }).ToListAsync();

        foreach (var property in properties)
        {
            var currentAgent = agentsDict[property.Agent!.Id];
            property.Agent!.Name = $"{currentAgent.FirstName} {currentAgent.LastName}";
        }

        return properties;
    }
}