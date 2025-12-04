
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Application.ViewModels.Property;


namespace RealStateApp.Areas.Agent.Controllers;
[Area("Agent")]
[Authorize(Roles = $"{nameof(Roles.Agent)}")] // Recuerda leer el apartado de seguridad de los requerimientos

public class HomeController
    ( IMapper mapper, IPropertyService propertyService) : Controller
{
    private readonly IPropertyService _propertyService = propertyService;
    private readonly IMapper _mapper = mapper;
    

    public async Task<IActionResult> Index()
    {
        var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _propertyService.GetAllByAgentIdAsync(agentId!);

        if (result.IsFailure)
        {
            var msg = result.GeneralError ?? string.Join("; ", result.Errors!);
            return Problem(msg);
        }
        
        var vm = _mapper.Map<List<AgentPropertiesViewModel>>(result.Value);
        
        return View(vm);
    }



    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
}