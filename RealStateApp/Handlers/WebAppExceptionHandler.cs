using System.Text.Json;
using RealStateApp.Core.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace RealStateApp.Handlers;
public class WebAppExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problem = new ProblemDetails
        {
            Title = "Ha ocurrido un error",
            Status = StatusCodes.Status500InternalServerError,
            Detail = "Ha ocurrido un error interno en el servidor. Revisa si este handler esta dando problemas",
            Instance = context.Request.Path
        };

        if (exception is AppException)
        {
            problem.Title = "Error de negocio";
            problem.Status = StatusCodes.Status400BadRequest;
            problem.Detail = exception.Message;
            string json = JsonSerializer.Serialize(problem);
            context.Session.SetString("ProblemDetails", json);
            // Redirigir al controlador Error
            context.Response.Redirect("/Home/Error");
            return ValueTask.FromResult(true);
        }
        
        return ValueTask.FromResult(false);
   }
}




