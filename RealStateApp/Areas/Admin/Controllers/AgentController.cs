using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Agent;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Extensions;

namespace RealStateApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{nameof(Roles.Admin)}")] // Recuerda leer el apartado de seguridad de los requerimientos
public class AgentController : Controller
{
    private readonly IAccountServiceForWebApp _accountServiceForWebApp;
    private readonly IAgentService _agentService;
    private readonly IMapper _mapper;

    public AgentController(IAgentService agentService, IMapper mapper, IAccountServiceForWebApp accountServiceForWebApp)
    {
        _agentService = agentService;
        _mapper = mapper;
        _accountServiceForWebApp = accountServiceForWebApp;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var agents = await _agentService.GetAllAgentsWithCount();
        var agentsViewModels = _mapper.Map<List<AgentWithPropertyCountViewModel>>(agents);
        return View(agentsViewModels);
    }
    
    
    public async Task<IActionResult> ChangeAgentState(string userId, bool state)
    {
        var user = await _accountServiceForWebApp.GetUserById(userId);
        if (user is null)
        {
            ViewBag.Message = "No se encontro al usuario";
        }
        
        return View(new ChangeUserStateViewModel
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            State = state
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> ChangeAgentState(ChangeUserStateViewModel model)
    {
        var stateResult = await _agentService.SetStatus(model.UserId, model.State);
        if (stateResult.IsFailure)
        {
            this.SendValidationErrorMessages(stateResult);
            return View(model);
        }
        
        return RedirectToAction(nameof(Index));
    }
    

    
    public async Task<IActionResult> Delete(string userId,bool status)
    {
        var user = await _accountServiceForWebApp.GetUserById(userId);
        if (user is null)
        {
            ViewBag.Message = "No se encontro al usuario";
        }
        return View(new DeleteUserViewModel 
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> DeletePost(DeleteUserViewModel model)
    {
        await _agentService.DeleteAsync(model.UserId);
        return RedirectToAction(nameof(Index));
    }
}