using MediatR;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Agent.Queries.GetAll;

public class GetAllAgentsQuery : IRequest<List<AgentApiDto>>
{
}

public class GetAllAgentsQueryHandler : IRequestHandler<GetAllAgentsQuery, List<AgentApiDto>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IBaseAccountService _baseAccountService;

    public GetAllAgentsQueryHandler(IPropertyRepository propertyRepository, IBaseAccountService baseAccountService)
    {
        _propertyRepository = propertyRepository;
        _baseAccountService = baseAccountService;
    }

    public async Task<List<AgentApiDto>> Handle(GetAllAgentsQuery request, CancellationToken cancellationToken)
    {
        var userAgents = await _baseAccountService.GetAllUserOfRole(Roles.Agent, false);
        var agentsWithPropertyCount = new List<AgentApiDto>();
        
        foreach (var userAgent in userAgents)
        {
            int propertyCount = await _propertyRepository.GetAllQueryable().AsNoTracking()
                .CountAsync(p => p.AgentId == userAgent.Id, cancellationToken: cancellationToken);
            
            agentsWithPropertyCount.Add(new AgentApiDto
            {
                Id = userAgent.Id,
                FirstName = userAgent.FirstName,
                LastName = userAgent.LastName,
                Email = userAgent.Email,
                PhoneNumber = userAgent.PhoneNumber ?? "", 
                PropertyCount = propertyCount,
            });
        }
        return agentsWithPropertyCount;
    }
}