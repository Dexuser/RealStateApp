using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Agent;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Areas.Agent.Controllers;
[Area("Agent")]
[Authorize(Roles = $"{nameof(Roles.Agent)}")]
public class AgentProfileController(IAgentService agentService,IBaseAccountService baseAccountService, IMapper mapper) : Controller
{
    public async Task<IActionResult> Index()
    {
        var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await baseAccountService.GetUserByIdResult(agentId);

        if (result.IsFailure)
        {
            var msg = result.GeneralError ?? string.Join("; ", result.Errors!);
            return Problem(msg);
        }


        var vm = mapper.Map<UserViewModel>(result.Value);

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Index(AgentProfileViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var result = await baseAccountService.UpdateAgentProfileAsync(vm);

        return RedirectToAction("Index");
    }
}