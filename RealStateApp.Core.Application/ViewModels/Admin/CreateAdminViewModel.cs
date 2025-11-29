using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.Admin
{
    public class CreateAdminViewModel
    {
        [Required(ErrorMessage = "El campo nombre es requerido")]
        [DataType(DataType.Text)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "El campo apellido es requerido")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "El campo cedula es requerido")]
        [DataType(DataType.Text)]
        public required string IdentityCardNumber { get; set; }
        
        [Required(ErrorMessage = "El campo email es requerido")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "El campo nombre de usuario es requerido")]
        [DataType(DataType.Text)]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "El campo contraseña es requerido")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Compare(nameof(Password),ErrorMessage = "Las contraseñas deben de coincidir")]
        [Required(ErrorMessage = "El campo confirmar contraseña es requerido")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Tiene que seleccionar un rol para el usuario")]
        public required string Role { get; set; }
    }
}
