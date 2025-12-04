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
[SwaggerTag("Endpoints para el mantenimiento de mejoras (Improvements)")]
public class ImprovementController : BaseApiController
{
   
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Obtiene todas las mejoras",
        Description = "Devuelve un listado con todas las mejoras registradas en el sistema."
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
        Summary = "Obtiene una mejora por ID",
        Description = "Devuelve la mejora correspondiente al ID especificado."
    )]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetImprovementByIdQuery { Id = id });

        if (result == null)
            return NotFound("Mejora no encontrada.");

        return Ok(result);
    }

    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Crea una nueva mejora",
        Description = "Registra una nueva mejora en el sistema."
    )]
    public async Task<IActionResult> Create([FromBody] CreateImprovementCommand command)
    {
        var created = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Actualiza una mejora",
        Description = "Modifica la información de la mejora especificada."
    )]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateImprovementCommand command)
    {
        if (id != command.Id)
            return BadRequest("El ID de la ruta no coincide con el del body.");

        var updated = await Mediator.Send(command);
        return Ok(updated);
    }

    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Elimina una mejora",
        Description = "Elimina la mejora correspondiente al ID recibido."
    )]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var deleted = await Mediator.Send(new DeleteImprovementCommand { Id = id });
        return Ok(deleted);
    }
}