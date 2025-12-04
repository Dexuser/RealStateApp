using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Features.Agent.Commands.ChangeStatus;
using RealStateApp.Core.Application.Features.Agent.Queries.GetAll;
using RealStateApp.Core.Application.Features.Agent.Queries.GetAllPropertyOfAgent;
using RealStateApp.Core.Application.Features.Agent.Queries.GetById;
using RealStateApp.Core.Application.Features.Property.Queries.GetAll;
using RealStateApp.Core.Domain.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApi.Controllers.v1;

[Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Developer)}")]
[SwaggerTag("Provides endpoints for querying and enable and disabling agents.")]
public class AgentController : BaseApiController
{
    // GET
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AgentApiDto>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Retrieve all the Agents",
        Description = "Lists all agents in the system along with the number of properties they manage"
    )]
    public async Task<IActionResult> Get()
    {
        var Agents = await Mediator.Send(new GetAllAgentsQuery());
        if (Agents.Count == 0)
        {
            return NoContent();
        }
        
        return Ok(Agents);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentApiDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Retrieve a specific Agent",
        Description = "Retrieve an agent with the specified ID and the number of properties they manage"
    )]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var Agents = await Mediator.Send(new GetAgentByIdQuery() {Id = id});
        return Ok(Agents);
    }
    
    
    [HttpGet("{id:guid}/properties")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PropertyApiDto>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Retrieve all the properties managed by a specific Agent",
        Description = "Retrieve all the properties managed by a specific Agent, including theirs improvements, saleType and propertyType"
    )]
    public async Task<IActionResult> GetPropertiesByAgentId([FromRoute] string id)
    {
        var properties = await Mediator.Send(new GetAllPropertyOfAgentQuery() {Id = id});
        if (properties.Count == 0) return NoContent();
        return Ok(properties);
    }
    
    [Authorize(Roles = $"{nameof(Roles.Admin)}")]
    [HttpPost("change-status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Changes the status of a specific Agent",
        Description = "Change the status of the agent with the given ID"
    )]
    public async Task<IActionResult> ChangeStatusById([FromBody] ChangeAgentStatusCommand command)
    {
        var properties = await Mediator.Send(command);
        return NoContent();
    }
}