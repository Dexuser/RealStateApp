/*using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Areas.Agent.Controllers;
[Area("Agent")]
[Authorize(Roles = $"{nameof(Roles.Agent)}")]
public class AgentProfileController(IAgentService agentService, IMapper mapper) : Controller
{
    public async Task<IActionResult> Index()
    {
        var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await agentService.GetByIdAsync(agentId);

        if (result.IsFailure)
            return Problem(result.Error);

        var vm = mapper.Map<AgentProfileViewModel>(result.Value);

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Index(AgentProfileViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var result = await agentService.UpdateProfileAsync(vm);

        if (result.IsFailure)
            return Problem(result.Error);

        return RedirectToAction("Index");
    }
}*/ //este controlador se queda para mañana. 