using MediatR;
using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Features.Agent.Commands.ChangeStatus;

/// <summary>
/// Parameters required to change de status of a agent
/// </summary>
public class ChangeAgentStatusCommand : IRequest<Unit>
{
    /// <example></example>
    [SwaggerParameter(Description = "The ID of the agent whose status is going to change.")]
    public required string Id { get; set; }
    /// <example>false</example>
    [SwaggerParameter(Description = "")]
    public required bool StatusToSet { get; set; }
}
public class ChangeAgentStatusCommandHandler : IRequestHandler<ChangeAgentStatusCommand, Unit>
{
    private readonly IBaseAccountService _baseAccountService;
    private readonly IPropertyRepository _propertyRepository;

    public ChangeAgentStatusCommandHandler(IBaseAccountService baseAccountService, IPropertyRepository propertyRepository)
    {
        _baseAccountService = baseAccountService;
        _propertyRepository = propertyRepository;
    }

    public async Task<Unit> Handle(ChangeAgentStatusCommand request, CancellationToken cancellationToken)
    {
        var userDto = await _baseAccountService.GetUserById(request.Id);
        if (userDto == null || userDto.Role != (nameof(Roles.Agent)))
            throw new ApiException("Agent not found", StatusCodes.Status404NotFound);
        
        await _baseAccountService.SetStateOnUser(request.Id, request.StatusToSet);
        return Unit.Value;
    }
}