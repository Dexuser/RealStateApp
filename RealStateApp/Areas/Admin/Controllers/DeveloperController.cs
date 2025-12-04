using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Developer;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Extensions;

namespace RealStateApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{nameof(Roles.Admin)}")] 
public class DeveloperController(
    IDeveloperService agentService,
    IMapper mapper,
    IAccountServiceForWebApp accountServiceForWebApp)
    : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var agents = await agentService.GetAllDevelopers();
        var agentsViewModels = mapper.Map<List<UserViewModel>>(agents);
        return View(agentsViewModels);
    }

    public IActionResult CreateDeveloper()
    {
        var viewModel = new CreateDeveloperViewModel() 
        {
            FirstName = "",
            LastName = "",
            IdentityCardNumber = "",
            Email = "",
            UserName = "",
            Password = "",
            ConfirmPassword = "",
            Role = nameof(Roles.Developer)
        };
        return View(viewModel);
    }
 
    [HttpPost]
    public async Task<IActionResult> CreateDeveloper(CreateDeveloperViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var userSave = mapper.Map<UserSaveDto>(model);
        var origin = HttpContext.Request.Headers.Origin.FirstOrDefault() ?? "";
        var createResult = await agentService.Create(userSave, origin);

        if (createResult.IsFailure)
        {
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> EditDeveloper(string userId)
    {
        
        var model = new EditDevViewModel()
        {
            FirstName = "",
            LastName = "",
            IdentityCardNumber = "",
            Email = "",
            UserName = "",
            Password = "",
            ConfirmPassword = "",
            Role = "",
            Id = "",
        };

        var user = await accountServiceForWebApp.GetUserById(userId);
        if (user == null)
        {
            ViewBag.Message = "No se encontro un usuario con este ID";
            return View(model);
        }

        if (user.Role != nameof(Roles.Developer))
        {
            ViewBag.Message = "El usuario no es un desarrollador";
            return View(model);
        }
        
        model.FirstName = user.FirstName;
        model.LastName = user.LastName;
        model.IdentityCardNumber = user.IdentityCardNumber;
        model.Email = user.Email;
        model.UserName= user.UserName;
        model.Role= user.Role;
        model.Id= user.Id;
        
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditDeveloper(EditDevViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model); 
        }
        
        var userSave = mapper.Map<UserSaveDto>(model);
        var origin = HttpContext.Request.Headers.Origin.FirstOrDefault() ?? "";
        await agentService.Edit(userSave, origin);
        return RedirectToAction(nameof(Index));
    }

    
    
    public async Task<IActionResult> ChangeDeveloperState(string userId, bool state)
    {
        var user = await accountServiceForWebApp.GetUserById(userId);
        if (user is null)
        {
            ViewBag.Message = "No se encontro al usuario";
        }
        
        return View(new ChangeUserStateViewModel
        {
            UserId = user!.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            State = state
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> ChangeDeveloperState(ChangeUserStateViewModel model)
    {
        var stateResult = await agentService.SetStateAsync(model.UserId, model.State);
        if (stateResult.IsFailure)
        {
            this.SendValidationErrorMessages(stateResult);
            return View(model);
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    public async Task<IActionResult> DeletePost(DeleteUserViewModel model)
    {
        await agentService.DeleteAsync(model.UserId);
        return RedirectToAction(nameof(Index));
    }
}