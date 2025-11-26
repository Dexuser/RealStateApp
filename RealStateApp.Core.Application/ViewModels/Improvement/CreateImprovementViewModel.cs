using System.ComponentModel.DataAnnotations;
using RealStateApp.Core.Application.ViewModels.PropertyImprovement;

namespace RealStateApp.Core.Application.ViewModels.Improvement;

public class CreateImprovementViewModel
{
    [Required(ErrorMessage = "El campo nombre es requerido")]
    [DataType(DataType.Text)]
    public required string Name { get; set; }
    [Required(ErrorMessage = "El campo descripción es requerido")]
    [DataType(DataType.MultilineText)]
    public required string Description { get; set; }
}
