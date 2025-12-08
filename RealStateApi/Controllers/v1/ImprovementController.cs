using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Features.Improvement.Commands.Create;
using RealStateApp.Core.Application.Features.Improvement.Commands.Delete;
using RealStateApp.Core.Application.Features.Improvement.Commands.Update;
using RealStateApp.Core.Application.Features.Improvement.Queries.GetAll;
using RealStateApp.Core.Application.Features.Improvement.Queries.GetById;
using RealStateApp.Core.Domain.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApi.Controllers.v1;

[ApiVersion("1.0")]
[Authorize(Roles = $"{nameof(Roles.Developer)},{nameof(Roles.Admin)}")]
[SwaggerTag("Endpoints for managing improvements (Improvements)")]
public class ImprovementController : BaseApiController
{

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Gets all improvements",
        Description = "Returns a list of all improvements registered in the system."
    )]
    public async Task<IActionResult> Get()
    {
        var result = await Mediator.Send(new GetAllImprovementsQuery());

        if (result.Count == 0)
            return NoContent();

        return Ok(result);
    }


    [HttpGet("by-id/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Gets an improvement by ID",
        Description = "Returns the improvement corresponding to the specified ID."
    )]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetImprovementByIdQuery { Id = id });

        if (result == null)
            return NotFound("Improvement not found.");

        return Ok(result);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Creates a new improvement",
        Description = "Registers a new improvement in the system."
    )]
    public async Task<IActionResult> Create([FromBody] CreateImprovementCommand command)
    {
        var created = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }


    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Updates an improvement",
        Description = "Modifies the information of the specified improvement."
    )]
    public async Task<IActionResult> Update([FromBody] UpdateImprovementCommand command)
    {
        var updated = await Mediator.Send(command);
        return Ok(updated);
    }


    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Deletes an improvement",
        Description = "Deletes the improvement corresponding to the specified ID."
    )]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var deleted = await Mediator.Send(new DeleteImprovementCommand { Id = id });
        return Ok(deleted);
    }
}
