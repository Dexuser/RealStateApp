using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Dtos.Login;

/// <summary>
///  Campos requeridos para obtener un token de cambio de contraseña
/// </summary>
public class ResetTokenDto
{
    /// <example>user123</example>
    [SwaggerParameter(Description = "Nombre de usuario de la cuenta de la que se quiere obtener el token")]
    public required string Username { get; set; }
}