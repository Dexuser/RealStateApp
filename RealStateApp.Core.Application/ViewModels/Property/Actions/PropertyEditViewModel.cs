using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealStateApp.Core.Application.ViewModels.PropertyImage;

namespace RealStateApp.Core.Application.ViewModels.Property.Actions;

public class PropertyEditViewModel
{
    public int Id { get; set; }

    [Required] public string Code { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Tipo de propiedad")]
    public int PropertyTypeId { get; set; }

    [Required]
    [Display(Name = "Tipo de venta")]
    public int SaleTypeId { get; set; }

    [Required] [Range(1, 100000000)] public decimal Price { get; set; }

    [Required]
    [Display(Name = "Metros cuadrados")]
    public double SizeInMeters { get; set; }

    [Required] [Range(1, 50)] public int Rooms { get; set; }

    [Required] [Range(1, 50)] public int Bathrooms { get; set; }

    [Required] public string Description { get; set; } = string.Empty;
    
    public List<PropertyImageViewModel> CurrentImages { get; set; } = new();

    public List<int> ImagesToDelete { get; set; } = new();

    [Display(Name = "Nueva imagen principal (opcional)")]
    public IFormFile? NewMainImage { get; set; }

    [Display(Name = "Nuevas imágenes adicionales")]
    public List<IFormFile>? NewAdditionalImages { get; set; }

    public List<int> SelectedImprovements { get; set; } = new();

    //Repito aqui tambien es solo para las vistas. Alna
    public List<SelectListItem> PropertyTypes { get; set; } = new();
    public List<SelectListItem> SaleTypes { get; set; } = new();
    public List<SelectListItem> Improvements { get; set; } = new();
}

