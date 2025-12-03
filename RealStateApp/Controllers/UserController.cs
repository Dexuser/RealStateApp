using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Login;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Extensions;
using RealStateApp.Handlers;

namespace RealStateApp.Controllers;

public class UserController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IAccountServiceForWebApp _accountServiceForWebApp;

    public UserController(IMapper mapper, RoleManager<IdentityRole> roleManager, IAccountServiceForWebApp accountServiceForWebApp)
    {
        _mapper = mapper;
        _roleManager = roleManager;
        _accountServiceForWebApp = accountServiceForWebApp;
    }

    // GET
    public async Task<IActionResult> CreateUser()
    { 
        await FillRolesViewBag(); 
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateClientOrAgentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await FillRolesViewBag();
            return View(model);
        }
        var userToCreate = _mapper.Map<UserSaveDto>(model);
        var origin = HttpContext.Request.Headers.Origin.FirstOrDefault() ?? "";
        var createUserResult = await _accountServiceForWebApp.RegisterUser(userToCreate, origin);
        
        if (createUserResult.IsFailure)
        {
            await FillRolesViewBag(); 
            this.SendValidationErrorMessages(createUserResult);
            return View(model);
        }
        
        UserSaveDto returnUser =_mapper.Map<UserSaveDto>( createUserResult.Value!);
        returnUser.ProfileImagePath = FileHandler.Upload(model.ProfileImage, returnUser.Id, "users");
        await _accountServiceForWebApp.EditUser(returnUser, origin, true);

        return RedirectToRoute(new {controller="Login", action="Index"});
    }
    
    private async Task FillRolesViewBag()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        roles.RemoveAll(idtRole => idtRole.Name == nameof(Roles.Developer));
        roles.RemoveAll(idtRole => idtRole.Name == nameof(Roles.Admin));
        ViewBag.Roles = new SelectList(roles,  "Name", "NormalizedName");
    }
}