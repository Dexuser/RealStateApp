using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Features.SaleType.Commands.Create;
using RealStateApp.Core.Application.Features.SaleType.Commands.Delete;
using RealStateApp.Core.Application.Features.SaleType.Commands.Update;
using RealStateApp.Core.Application.Features.SaleType.Queries.GetAll;
using RealStateApp.Core.Application.Features.SaleType.Queries.GetById;
using RealStateApp.Core.Domain.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApi.Controllers.v1;

[ApiVersion("1.0")]
[Authorize(Roles = $"{nameof(Roles.Developer)},{nameof(Roles.Admin)}")]
[SwaggerTag("Endpoints for managing sale types.")]
public class SaleTypeController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Gets all sale types",
        Description = "Returns a list of all sale types registered in the system."
    )]
    public async Task<IActionResult> Get()
    {
        var result = await Mediator.Send(new GetAllSaleTypesQuery());

        if (result.Count == 0)
            return NoContent();

        return Ok(result);
    }

    [HttpGet("by-id/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Gets a sale type by ID",
        Description = "Searches and returns the sale type with the specified ID."
    )]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetSaleTypeByIdQuery { Id = id });

        if (result == null)
            return NotFound("Sale type not found.");

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Creates a new sale type",
        Description = "Registers a new sale type in the system."
    )]
    public async Task<IActionResult> Create([FromBody] CreateSaleTypeCommand command)
    {
        var created = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Updates a sale type",
        Description = "Updates the information of the specified sale type."
    )]
    public async Task<IActionResult> Update([FromBody] UpdateSaleTypeCommand command)
    {
        var updated = await Mediator.Send(command);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Deletes a sale type",
        Description = "Deletes the sale type associated with the provided ID."
    )]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var deleted = await Mediator.Send(new DeleteSaleTypeCommand { Id = id });
        return Ok(deleted);
    }
}
