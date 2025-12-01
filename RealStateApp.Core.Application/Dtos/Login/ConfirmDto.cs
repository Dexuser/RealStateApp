using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Dtos.Login;

/// <summary>
/// Parametros requeridos para confirmar una cuenta
/// </summary>
public class ConfirmDto
{
    /// <example>8f9d2a3b-5c4a-42a7-b6f7-0e9e8bdb178a</example>
    [SwaggerParameter(description: "El ID del usuario a confirmar")]
    public required string UserId {get;set;}
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    [SwaggerParameter(description: "El ID del usuario a confirmar")]
    public required string Token { get; set; }
}