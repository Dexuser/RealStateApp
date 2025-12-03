using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Features.Agent.Queries.GetAll;
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
[SwaggerTag("Endpoints para mantenimiento de tipos de ventas")]
public class SaleTypeController : BaseApiController
{
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Obtiene todos los tipos de venta",
        Description = "Devuelve un listado con todos los tipos de venta registrados en el sistema."
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
        Summary = "Obtiene un tipo de venta por ID",
        Description = "Busca y devuelve el tipo de venta con el ID especificado."
    )]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetSaleTypeByIdQuery { Id = id });

        if (result == null)
            return NotFound("Tipo de venta no encontrado.");

        return Ok(result);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Crea un nuevo tipo de venta",
        Description = "Registra un nuevo tipo de venta en el sistema."
    )]
    public async Task<IActionResult> Create([FromBody] CreateSaleTypeCommand command)
    {
        var created = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Actualiza un tipo de venta",
        Description = "Modifica la información del tipo de venta especificado."
    )]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSaleTypeCommand command)
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
        Summary = "Elimina un tipo de venta",
        Description = "Elimina el tipo de venta correspondiente al ID recibido."
    )]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var deleted = await Mediator.Send(new DeleteSaleTypeCommand { Id = id });
        return Ok(deleted);
    }
}