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
    [SwaggerTag("Endpoints for account registration, authentication and recovery")]
    public class AccountController(IAccountServiceForWebApi accountServiceForWebApi) : BaseApiController
    {
        private readonly IAccountServiceForWebApi _accountServiceForWebApi = accountServiceForWebApi;

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Authenticates a user",
            Description = "Validates the provided credentials and returns a JWT if they are correct"
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

            return Ok(new { token = result.Value });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Registers a new administrator in the system",
            Description = "Creates a new administrator in the system. An image can be uploaded for this user"
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
            save.ProfileImagePath = FileHandler.Upload(dto.ProfileImage, save.Id, "users");

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
            Summary = "Registers a new developer",
            Description = "Creates a new developer in the system. An image can be uploaded for this user"
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
            save.ProfileImagePath = FileHandler.Upload(dto.ProfileImage, save.Id, "users");

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
            Summary = "Generates a new password reset token",
            Description = "Generates a new token to reset the account password. This token is sent via email"
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
            Summary = "Resets the user's password",
            Description = "Resets the user's password using the provided token"
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
