using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Features.Property.Queries.GetAll;
using RealStateApp.Core.Application.Features.PropertyType.Commands.CreatePropertyType;
using RealStateApp.Core.Application.Features.PropertyType.Commands.DeleteCommand;
using RealStateApp.Core.Application.Features.PropertyType.Commands.UpdatePropertyType;
using RealStateApp.Core.Application.Features.PropertyType.Queries.GetAll;
using RealStateApp.Core.Application.Features.PropertyType.Queries.GetById;
using RealStateApp.Core.Domain.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApi.Controllers.v1;

public class PropertyTypeController : BaseApiController
{
    [HttpPost]
    [Authorize(Roles = $"{nameof(Roles.Admin)}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PropertyTypeApiDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Creates a new property type",
        Description =
            "Creates a new property type with the specified values."
    )]
    public async Task<IActionResult> Post([FromBody] CreatePropertyTypeCommand command)
    {
        var result = await Mediator.Send( command);
        return Created();
    }
    
    
    [HttpPut]
    [Authorize(Roles = $"{nameof(Roles.Admin)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Updates a property type",
        Description =
            "Updates a property type with the specified values."
    )]
    public async Task<IActionResult> Post([FromBody] UpdatePropertyTypeCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok();
    }
    
    [HttpGet]
    [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Developer)}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PropertyTypeApiDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Retrieves all properties types",
        Description =
            "List s all properties types with theirs names and descriptions."
    )]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllPropertyTypeQuery());
        if (result.Count == 0)
        {
            return NoContent();
        }
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Developer)}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeApiDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Retrieves a property type",
        Description =
            "Returns the property type with the given identifier."
    )]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetPropertyTypeByIdQuery() {Id = id});
        return Ok(result);
    }
    
    [HttpDelete("{id:int}/delete")]
    [Authorize(Roles = $"{nameof(Roles.Admin)}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeApiDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Deletes a property type",
        Description =
            "Deletes a property type with the specified ID."
    )]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var result = await Mediator.Send(new DeletePropertyTypeCommand() {Id = id});
        return NoContent();
    }
    
}