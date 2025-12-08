using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealStateApp.Core.Application.ViewModels.Agent;


public class EditAgentViewModel
{
    public required string Id { get; set; }
    [Required(ErrorMessage = "El campo nombre es requerido")]
    [DataType(DataType.Text)]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "El campo apellido es requerido")]
    [DataType(DataType.Text)]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "El campo telefono es requerido")]
    [DataType(DataType.Text)]
    public required string PhoneNumber { get; set; }

    [DataType(DataType.Upload)]
    public IFormFile? ImageProfile { get; set; }
    
    public string? ProfileImagePath { get; set; }
}