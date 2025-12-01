using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Dtos.Login;

/// <summary>
/// Parámetros requeridos para cambiar la contraseña de un usuario usando un token valido
/// </summary>
public class ResetPasswordApiRequestDto
{
    /// <example>8f9d2a3b-5c4a-42a7-b6f7-0e9e8bdb178a</example>
    [SwaggerParameter(Description = "El ID del usuario")]
    public required string UserId { get; set; }
    
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    [SwaggerParameter(Description = "El token de cambio de contraseña")]
    public required string Token { get; set; }
    
    /// <example>NewP@ssword2025!</example>
    [SwaggerParameter(Description = "La nueva contraseña que tendrá la cuenta del usuario")]
    public required string Password { get; set; }    
    
    /// <example>NewP@ssword2025!</example>
    [SwaggerParameter(Description = "La confirmación de nueva contraseña que tendrá la cuenta del usuario")]
    public required string ConfirmPassword { get; set; }    
}
