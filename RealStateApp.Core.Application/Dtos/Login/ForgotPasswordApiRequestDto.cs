using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Dtos.Login
{
    /// <summary>
    /// Parámetros requeridos para obtener un token de cambio de contraseña
    /// </summary>
    public class ForgotPasswordApiRequestDto
    {
        /// <example>juanp</example>
        [SwaggerParameter(Description = "The username of the account requesting password reset")]
        public required string UserName { get; set; }
    }
}
