using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Agent.Queries.GetById;

public class GetAgentByIdQuery : IRequest<AgentApiDto>
{
    public required string Id { get; set; }
}

public class GetAgentByIdQueryHandler : IRequestHandler<GetAgentByIdQuery, AgentApiDto>
{
    private readonly IBaseAccountService _baseAccountService;
    private readonly IPropertyRepository _propertyRepository;

    public GetAgentByIdQueryHandler(IBaseAccountService baseAccountService, IPropertyRepository propertyRepository)
    {
        _baseAccountService = baseAccountService;
        _propertyRepository = propertyRepository;
    }

    public async Task<AgentApiDto> Handle(GetAgentByIdQuery request, CancellationToken cancellationToken)
    {
        var userDto = await _baseAccountService.GetUserById(request.Id);
        
        if (userDto == null || userDto.Role != nameof(Roles.Agent)) throw new ApiException("Agent not found", StatusCodes.Status404NotFound);

        return new AgentApiDto
        {
            Id = userDto.Id,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            PropertyCount = await _propertyRepository.GetAllQueryable()
                .CountAsync(property => property.AgentId == userDto.Id, cancellationToken: cancellationToken),
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber ?? string.Empty,
        };
    }
}