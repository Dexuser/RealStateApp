using ArtemisBanking.Core.Application.Dtos.Login;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Login;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Extensions;
using RealStateApp.Infrastructure.Identity.Entities;

namespace RealStateApp.Controllers;

public class LoginController(
    IMapper mapper,
    IAccountServiceForWebApp accountServiceForWebApp,
    UserManager<AppUser> userManager)
    : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user != null)
        {
            var role =  await userManager.GetRolesAsync(user);
            return await RedirectToHomeByRole(role[0]);
        }
        
        return View(new LoginViewModel() {UserName = "",  Password = ""});
    }
    
    [HttpPost]
    public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("Index",loginViewModel);
        }
        
        var loginDto = mapper.Map<LoginDto>(loginViewModel);
        var result = await accountServiceForWebApp.AuthenticateAsync(loginDto);
        
        if (result.IsFailure)
        {
            this.SendValidationErrorMessages(result);
            return View("Index",loginViewModel);
        }

        var user = result.Value!;
        return await RedirectToHomeByRole(user.Role);
    }

    public async Task<IActionResult> LogOut()
    {
        await accountServiceForWebApp.SignOutAsync();
        return RedirectToRoute(new { controller = "Login", action = "Index" });
    }


    public IActionResult ForgotPassword()
    {
        return View(new ForgotPasswordRequestViewModel() {UserName = ""} );
    }
    
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var origin = Request.Headers?.Origin.ToString() ?? string.Empty;
        ForgotPasswordRequestDto dto = new() { UserName = vm.UserName,Origin = origin};
        var result = await accountServiceForWebApp.ForgotPasswordAsync(dto);

        if (result.IsFailure)
        {
            this.SendValidationErrorMessages(result);
            return View(vm);
        }          

        return RedirectToRoute(new { controller = "Login", action = "Index" });
    }

    public IActionResult ResetPassword(string userId, string token)
    {           
        return View(new ResetPasswordRequestViewModel()
        {
            Id = userId,
            Token = token,
            Password = "",
            ConfirmPassword = "",
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequestViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }  

        ResetPasswordRequestDto dto = new()
        {
            UserId = vm.Id,
            Password = vm.Password,
            Token = vm.Token
        };

        var result = await accountServiceForWebApp.ResetPasswordAsync(dto);

        if (result.IsFailure)
        {
            this.SendValidationErrorMessages(result);
            return View(vm);
        }

        return RedirectToRoute(new { controller = "Login", action = "Index" });
    }



    private async Task<IActionResult> RedirectToHomeByRole(string role)
    {
        if (Enum.TryParse(role, out Roles userRole))
        {
            switch (userRole)
            {
                case Roles.Admin:
                    return RedirectToRoute(new { area="Admin" ,controller = "Home", action = "Index" });

                case Roles.Agent:
                    return RedirectToRoute(new { area="Agent" ,controller = "Home", action = "Index" });
                
                case Roles.Client:
                    return RedirectToRoute(new { area="Client" , controller = "Home", action = "Index" });
           }
        }
        
        await accountServiceForWebApp.SignOutAsync();
        ViewBag.Message = "Los usuarios de rol desarrollador solamente pueden acceder por la API";
        return View("Index", new LoginViewModel
        {
            UserName = "",
            Password = ""
        });
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
    

    
    public async Task<IActionResult> ConfirmEmail(string userId,string token)
    {
        var result = await accountServiceForWebApp.ConfirmAccountAsync(userId, token);
        if (result.IsFailure)
        {
            return View("ConfirmEmail", result.GeneralError);
        }
        return View("ConfirmEmail", "Tu cuenta ha sido activada correctamente. Ya puedes iniciar sesión");
    }

}