using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApi.Handlers;
using RealStateApp.Core.Application.Dtos.Login;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Endpoints para el registro, autenticación y recuperación de cuentas")]
    public class AccountController(IAccountServiceForWebApi accountServiceForWebApi) : BaseApiController
    {
        private readonly IAccountServiceForWebApi _accountServiceForWebApi = accountServiceForWebApi;

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Autentica un usuario",
            Description = "Valida las credenciales recibidas y devuelve un JWT en caso de que sean correctas"
        )]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _accountServiceForWebApi.AuthenticateAsync(dto);
            if (result.IsFailure)
            {
                return BadRequest400WithErrorMessagesFromResult(result);
            }

            return Ok(new { token = result.Value});
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Registra un nuevo administrador en el sistema",
            Description = "Crea un nuevo administrador en el sistema. Se puede enviar una imagen para este usuario"
        )]
        public async Task<IActionResult> RegisterAdmin([FromForm] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            var save = new UserSaveDto
            {
                Id = "",
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.UserName,
                Password = dto.Password,
                IdentityCardNumber = dto.IdentityCardNumber,
                PhoneNumber = dto.Phone,
                Role = nameof(Roles.Admin),
            };

            var result = await _accountServiceForWebApi.RegisterUser(save, null, true);

            if (result.IsFailure)
            {
                return BadRequest400WithErrorMessagesFromResult(result);
            }

            save.Id = result.Value!.Id;
            save.ProfileImagePath = FileHandler.Upload(dto.ProfileImage, save.Id , "users");

            var resultEdit = await _accountServiceForWebApi.EditUser(save, null, true);

            if (resultEdit.IsFailure)
            {
                return BadRequest400WithErrorMessagesFromResult(result);
            }


            return StatusCode(StatusCodes.Status201Created);
        }
        
        [HttpPost("register-dev")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Registra un nuevo desarrollador",
            Description = "Crea un nuevo desarrollador en el sistema. Se puede enviar una imagen para el usuario"
        )]
        public async Task<IActionResult> RegisterDeveloper([FromForm] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var save = new UserSaveDto
            {
                Id = "",
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.UserName,
                Password = dto.Password,
                IdentityCardNumber = dto.IdentityCardNumber,
                PhoneNumber = dto.Phone,
                Role = nameof(Roles.Developer),
            };

            var result = await _accountServiceForWebApi.RegisterUser(save, null, true);

            if (result.IsFailure)
            {
                return BadRequest400WithErrorMessagesFromResult(result);
            }

            save.Id = result.Value!.Id;
            save.ProfileImagePath = FileHandler.Upload(dto.ProfileImage, save.Id , "users");

            var resultEdit = await _accountServiceForWebApi.EditUser(save, null, true);

            if (resultEdit.IsFailure)
            {
                return BadRequest400WithErrorMessagesFromResult(result);
            }


            return StatusCode(StatusCodes.Status201Created);
        }
        


        [Authorize(Roles = "Admin")]
        [HttpPost("get-reset-token")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Genera un nuevo token con el que resetear la contraseña",
            Description = "Genera un nuevo token para cambiar la contraseña de una cuenta. Esta se envia por Email"
        )]
        public async Task<IActionResult> GetResetToken([FromBody] ForgotPasswordApiRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _accountServiceForWebApi.ForgotPasswordAsync(
                new ForgotPasswordRequestDto
                {
                    UserName = dto.UserName,
                    Origin = ""
                }, true);

            if (result.IsFailure)
            {
                return BadRequest400WithErrorMessagesFromResult(result);
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Resetea la contraseña del usuario",
            Description = "Resetea la contraseña del usuario con el token enviado"
        )]
        public async Task<IActionResult> ChangePassword([FromBody] ResetPasswordRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _accountServiceForWebApi.ResetPasswordAsync(dto);

            if (result.IsFailure)
            {
                return BadRequest400WithErrorMessagesFromResult(result);
            }

            return NoContent();
        }
    }
}
