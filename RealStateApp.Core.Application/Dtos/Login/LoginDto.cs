using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Dtos.Login;

/// <summary>
/// Parámetros necesarios para identificar a un usuario.
/// </summary>
public class LoginDto
{
    /// <example>admin</example>
    [SwaggerParameter(Description = "Nombre de usuario de la cuenta")]
    public required string UserName { get; set; }
    
    /// <example>123Pa$$word!</example>
    [SwaggerParameter(Description = "Contraseña de la cuenta")]
    public required string Password { get; set; }
}