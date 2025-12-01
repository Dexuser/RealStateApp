using Microsoft.AspNetCore.Http;
using RealStateApp.Core.Domain.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Dtos.User
{
    /// <summary>
    /// Parameters required to register a new user
    /// </summary>
    public class CreateUserDto
    {
        /// <example>Juan</example>
        [SwaggerParameter(Description = "The user's first name")]
        public required string FirstName { get; set; }

        /// <example>Pérez</example>
        [SwaggerParameter(Description = "The user's last name")]
        public required string LastName { get; set; }

        
        /// <example>0000000001</example>
        [SwaggerParameter(Description = "The user's last name")]
        public required string IdentityCardNumber { get; set; }
        
        /// <example>juan.perez@example.com</example>
        [SwaggerParameter(Description = "The user's email address")]
        public required string Email { get; set; }

        /// <example>juanp</example>
        [SwaggerParameter(Description = "The username for login")]
        public required string UserName { get; set; }

        /// <example>P@ssw0rd!</example>
        [SwaggerParameter(Description = "The password for login")]
        public required string Password { get; set; }

        /// <example>+18095551234</example>
        [SwaggerParameter(Description = "The user's phone number (optional)")]
        public string? Phone { get; set; }

        [SwaggerParameter(Description = "Profile image file to upload")]
        public required IFormFile ProfileImage { get; set; }

    }
}
