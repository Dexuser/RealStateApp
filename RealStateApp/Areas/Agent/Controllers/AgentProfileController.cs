using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Agent;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Extensions;
using RealStateApp.Handlers;

namespace RealStateApp.Areas.Agent.Controllers;
[Area("Agent")]
[Authorize(Roles = $"{nameof(Roles.Agent)}")]
public class AgentProfileController(IAgentService agentService,IBaseAccountService baseAccountService, IMapper mapper) : Controller
{
    public async Task<IActionResult> Index()
    {
        var agentId= User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await baseAccountService.GetUserById(agentId);

        if (result == null)
        {
            ViewBag.Messages = "No se encontro al agente";
            return RedirectToAction(nameof(Index));
        }

        var vm = new EditAgentViewModel
        {
            Id = result.Id,
            FirstName = result.FirstName,
            LastName = result.LastName,
            PhoneNumber = result.PhoneNumber,
            ProfileImagePath = result.ProfileImagePath,
            ImageProfile = null,
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Index(EditAgentViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var user = await baseAccountService.GetUserById(vm.Id);

        if (user == null)
        {
            ViewBag.Messages = "No se encontro al agente";
            return RedirectToAction(nameof(Index));
        }

        var save = mapper.Map<UserSaveDto>(user);
        save.FirstName = vm.FirstName;
        save.LastName = vm.LastName;
        save.PhoneNumber = vm.PhoneNumber;
        save.ProfileImagePath = FileHandler.Upload(vm.ImageProfile, vm.Id, "users", true, vm.ProfileImagePath);
        
        var origin = HttpContext.Request.Headers.Origin.First() ?? "";
        var result = await agentService.Edit(save, origin);
        return RedirectToAction("Index");
    }
}