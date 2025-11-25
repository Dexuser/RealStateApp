using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.Login
{
    public class ForgotPasswordRequestViewModel
    {
        [Required(ErrorMessage = "El campo nombre de usuario es requerido")]
        [DataType(DataType.Text)]
        public required string UserName { get; set; }      
    }
}
