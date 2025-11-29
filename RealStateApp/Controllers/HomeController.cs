using System.Diagnostics;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Login;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.PropertyType;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Infrastructure.Identity.Entities;
using RealStateApp.Models;

namespace RealStateApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<AppUser> _userManager;
    private readonly IPropertyService _propertyService;
    private readonly IPropertyTypeService _propertyTypeService;
    private readonly IMapper _mapper;

    public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, IPropertyService propertyService, IPropertyTypeService propertyTypeService, IMapper mapper)
    {
        _logger = logger;
        _userManager = userManager;
        _propertyService = propertyService;
        _propertyTypeService = propertyTypeService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(PropertyViewModelFilters filters)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            return RedirectToRoute(new { area = "", controller = "Login", action = "Index" });
        }

        var filtersDto = new PropertyFiltersDto()
        {
            SelectedPropertyTypeId = filters.SelectedPropertyTypeId,
            Bathrooms = filters.Bathrooms,
            MaxValue = filters.MaxValue,
            MinValue = filters.MinValue,
            Rooms = filters.Rooms,
        };
        
        var model = new HomeIndexViewModel()
        {
            Properties = _mapper.Map<List<PropertyViewModel>>(await _propertyService.GetAllAvailablePropertiesAsync(filtersDto)),
            PropertyTypes = _mapper.Map<List<PropertyTypeViewModel>>(await _propertyTypeService.GetAllAsync()),
            Filters = filters, 
        };
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var model = _mapper.Map<PropertyViewModel>(await _propertyService.GetByIdAsync(id));
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    private IActionResult RedirectToHomeByRole(string role)
    {
        if (Roles.TryParse(role, out Roles userRole))
        {
            switch (userRole)
            {
                case Roles.Admin:
                    return RedirectToRoute(new { area = "Admin", controller = "Home", action = "Index" });

                case Roles.Agent:
                    return RedirectToRoute(new { area = "Agent", controller = "Home", action = "Index" });

                case Roles.Client:
                    return RedirectToRoute(new { area = "Client", controller = "Home", action = "Index" });

                case Roles.Developer:
                    return RedirectToRoute(new { area = "", controller = "Home", action = "Index" });
            }
        }

        return View("Index");
    }
    public IActionResult Error()
    {
        var json = HttpContext.Session.GetString("ProblemDetails");

        // Lo que tarde para encontrar que tenia que usar el Session para manejar este caso...
        if (!string.IsNullOrEmpty(json))
        {
            var problem = JsonSerializer.Deserialize<ProblemDetails>(json);

            // Limpiar para que no se repita accidentalmente
            HttpContext.Session.Remove("ProblemDetails");

            return View(problem);
        }

        return View(new ProblemDetails
        {
            Title = "Error inesperado",
            Detail = "Ha ocurrido un error.",
            Status = 500
        });
    }

}