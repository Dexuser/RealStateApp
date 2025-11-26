using System.ComponentModel.DataAnnotations;

namespace RealStateApp.Core.Application.ViewModels.PropertyType;

public class EditPropertyTypeViewModel
{
    public required int Id { get; set; }
    [Required(ErrorMessage = "El campo nombre es requerido")]
    public required string Name { get; set; }
    [Required(ErrorMessage = "El campo descripción es requerido")]
    [DataType(DataType.MultilineText)]
    public required string Description { get; set; }
}
