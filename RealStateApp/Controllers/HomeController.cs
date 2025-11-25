using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.ViewModels.Login;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Infrastructure.Identity.Entities;
using RealStateApp.Models;

namespace RealStateApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<AppUser> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            return RedirectToRoute(new { area = "", controller = "Login", action = "Index" });
        }

        return View();
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