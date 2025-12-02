using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application;

namespace RealStateApi.Controllers;

/// <summary>
/// Controlador base del que heredan todos los demás controladores.
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
    
    private IMediator? _mediator;

    /// <summary>
    /// Provee acceso al a la clase MediatR que esta registrado en los servicios de la aplicacion.
    /// </summary>
    protected IMediator Mediator => _mediator ??= HttpContext!.RequestServices.GetService<IMediator>()!;

    protected IActionResult BadRequest400WithErrorMessagesFromResult(Result result)
    {
        if (result.IsSuccess)
        {
            throw new Exception("This result isn't a failure");
        }
        // Este metodo solamente se usa en caso de que el Result haya fallado, es decir, isFailure
        if (!string.IsNullOrEmpty(result.GeneralError))
        {
            return BadRequest(result.GeneralError);
        }

        return BadRequest(result.Errors);
    }
}