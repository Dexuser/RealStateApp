using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RealStateApp.Core.Application.ViewModels.Property.Actions;

public class PropertyCreateViewModel
{
    
    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Tipo de propiedad")]
    public int PropertyTypeId { get; set; }

    [Required]
    [Display(Name = "Tipo de venta")]
    public int SaleTypeId { get; set; }

    [Required]
    [Range(1, 100000000)]
    public decimal Price { get; set; }

    [Required]
    [Display(Name = "Metros cuadrados")]
    public double SizeInMeters { get; set; }

    [Required]
    [Range(1, 50)]
    public int Rooms { get; set; }

    [Required]
    [Range(1, 50)]
    public int Bathrooms { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;
    

    [Required]
    [Display(Name = "Imagen principal")]
    public IFormFile? MainImage { get; set; }

    [Display(Name = "Imágenes adicionales")]
    public List<IFormFile>? AdditionalImages { get; set; }
    
    [Display(Name = "Seleccionar mejoras")]
    public List<int> SelectedImprovements { get; set; } = new();
    
    //esto es solo para las vistas
    public List<SelectListItem> PropertyTypes { get; set; } = new();
    public List<SelectListItem> SaleTypes { get; set; } = new();
    public List<SelectListItem> Improvements { get; set; } = new();
}