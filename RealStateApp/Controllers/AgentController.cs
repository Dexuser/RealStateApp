using Microsoft.AspNetCore.Mvc;

namespace RealStateApp.Controllers;

public class AgentController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}