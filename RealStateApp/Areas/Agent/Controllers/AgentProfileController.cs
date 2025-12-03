using Microsoft.AspNetCore.Mvc;

namespace RealStateApp.Areas.Agent.Controllers;

public class AgentProfileController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}