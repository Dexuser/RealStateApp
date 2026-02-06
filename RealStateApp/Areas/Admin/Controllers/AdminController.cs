using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Admin;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Extensions;

namespace RealStateApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{nameof(Roles.Admin)}")] 
public class AdminController : Controller
{
    private readonly IAccountServiceForWebApp _accountServiceForWebApp;
    private readonly IAdminService _adminService;
    private readonly IMapper _mapper;

    public AdminController(IAdminService agentService, IMapper mapper, IAccountServiceForWebApp accountServiceForWebApp)
    {
        _adminService = agentService;
        _mapper = mapper;
        _accountServiceForWebApp = accountServiceForWebApp;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var agents = await _adminService.GetAllAdmins();
        var agentsViewModels = _mapper.Map<List<UserViewModel>>(agents);
        return View(agentsViewModels);
    }

    public IActionResult CreateAdmin()
    {
        var viewModel = new CreateAdminViewModel
        {
            FirstName = "",
            LastName = "",
            IdentityCardNumber = "",
            Email = "",
            UserName = "",
            Password = "",
            ConfirmPassword = "",
            Role = nameof(Roles.Admin)
        };
        return View(viewModel);
    }
 
    [HttpPost]
    public async Task<IActionResult> CreateAdmin(CreateAdminViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var userSave = _mapper.Map<UserSaveDto>(model);
        var origin = HttpContext.Request.Headers.Origin.FirstOrDefault() ?? "";
        var createResult = await _adminService.Create(userSave, origin);

        if (createResult.IsFailure)
        {
            this.SendValidationErrorMessages(createResult);
            return View(model);
            
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> EditAdmin(string userId)
    {
        
        var model = new EditAdminViewModel
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

        var user = await _accountServiceForWebApp.GetUserById(userId);
        if (user == null)
        {
            ViewBag.Message = "No se encontro un usuario con este ID";
            return View(model);
        }

        if (user.Role != nameof(Roles.Admin))
        {
            ViewBag.Message = "El usuario no es un administrador";
            return View(model);
        }
        
        model.FirstName = user.FirstName;
        model.LastName = user.LastName;
        model.IdentityCardNumber = user.IdentityCardNumber;
        model.Email = user.Email;
        model.UserName= user.UserName;
        model.Role= user.Role.ToString();
        model.Id= user.Id;
        
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditAdmin(EditAdminViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model); 
        }
        
        var userSave = _mapper.Map<UserSaveDto>(model);
        var origin = HttpContext.Request.Headers.Origin.FirstOrDefault() ?? "";
        await _adminService.Edit(userSave, origin);
        return RedirectToAction(nameof(Index));
    }

    
    
    public async Task<IActionResult> ChangeAdminState(string userId, bool state)
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
    public async Task<IActionResult> ChangeAdminState(ChangeUserStateViewModel model)
    {
        var stateResult = await _adminService.SetStateAsync(model.UserId, model.State);
        if (stateResult.IsFailure)
        {
            this.SendValidationErrorMessages(stateResult);
            return View(model);
        }
        return RedirectToRoute(new {controller = "Admin", action = "Index"});
    }
    
}