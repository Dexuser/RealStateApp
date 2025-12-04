using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Features.Property.Queries.GetAll;
using RealStateApp.Core.Application.Features.Property.Queries.GetByCode;
using RealStateApp.Core.Application.Features.Property.Queries.GetById;
using RealStateApp.Core.Domain.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApi.Controllers.v1;

[ApiVersion("1.0")]
[Authorize(Roles = $"{nameof(Roles.Developer)},{nameof(Roles.Admin)}")]
[SwaggerTag("Endpoints to retrieve properties")]
public class PropertyController : BaseApiController
{
    // GET
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PropertyApiDto>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Gets a list of all properties",
        Description = "Retrieves a list of all properties including the information of the agent who manages them"
    )]
    public async Task<IActionResult> Get()
    {
        var properties = await Mediator.Send(new GetAllPropertyQuery());
        if (properties.Count == 0)
        {
            return NoContent();
        }

        return Ok(properties);
    }

    [HttpGet("by-id/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyApiDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Gets a property by Id",
        Description = "Retrieves the property matching the provided ID."
    )]
    public async Task<IActionResult> GetPropertyById([FromRoute] int id)
    {
        var properties = await Mediator.Send(new GetPropertyByIdQuery() { Id = id });
        return Ok(properties);
    }

    [HttpGet("by-code/{code}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyApiDto))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Gets a property by code",
        Description = "Retrieves the property matching the provided code."
    )]
    public async Task<IActionResult> GetPropertyByCode([FromRoute] string code)
    {
        var properties = await Mediator.Send(new GetPropertyByCodeQuery() { Code = code });
        return Ok(properties);
    }
}