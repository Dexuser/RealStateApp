using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.Property.Actions;
using RealStateApp.Core.Domain.Common;

namespace RealStateApp.Areas.Agent.Controllers;
[Area("Agent")]
[Authorize(Roles = $"{nameof(Roles.Agent)}")] // Recuerda leer el apartado de seguridad de los requerimientos
public class AgentPropertyController(
    IPropertyService propertyService,
    IPropertyImprovementService propertyImprovementService,
    IPropertyTypeService propertyTypeService,
    ISaleTypeService saleTypeService,
    IMapper mapper)
    : Controller
{
    private readonly IPropertyService _propertyService = propertyService;
    private readonly IPropertyImprovementService _propertyImprovementService = propertyImprovementService;
    private readonly IPropertyTypeService _propertyTypeService = propertyTypeService;
    private readonly ISaleTypeService _saleTypeService = saleTypeService;
    private readonly IMapper _mapper = mapper;
    // GET

    public async Task<IActionResult> Index()
    {
        var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _propertyService.GetPropertiesForMaintenanceAsync(agentId!);

        if (result.IsFailure)
        {
            var msg = result.GeneralError ?? string.Join("; ", result.Errors!);
            return Problem(msg);
        }
        
        var vm = _mapper.Map<List<PropertyViewModel>>(result.Value);
        
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var vm = new PropertyCreateViewModel
        {
            PropertyTypes = await _propertyTypeService.GetSelectListAsync(),
            SaleTypes = await _saleTypeService.GetSelectListAsync(),
            Improvements = await _propertyImprovementService.GetSelectListAsync()
        };
        
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PropertyCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.PropertyTypes = await _propertyTypeService.GetSelectListAsync();
            vm.SaleTypes = await _saleTypeService.GetSelectListAsync();
            vm.Improvements = await _propertyImprovementService.GetSelectListAsync();
            return View(vm);
        }
        var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _propertyService.CreatePropertyAsync(vm, agentId!);
        
        if (result.IsFailure)
        {
            var msg = result.GeneralError ?? string.Join("; ", result.Errors!);
            return Problem(msg);
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _propertyService.GetByIdForEditAsync(id);
        
        if (result.IsFailure)
        {
            var msg = result.GeneralError ?? string.Join("; ", result.Errors!);
            return Problem(msg);
        }
        
        var vm = _mapper.Map<PropertyEditViewModel>(result.Value);

        vm.PropertyTypes = await _propertyTypeService.GetSelectListAsync();
        vm.SaleTypes = await _saleTypeService.GetSelectListAsync();
        vm.Improvements  = await _propertyImprovementService.GetSelectListAsync();
        
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PropertyEditViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.PropertyTypes = await _propertyTypeService.GetSelectListAsync();
            vm.SaleTypes = await _saleTypeService.GetSelectListAsync();
            vm.Improvements = await _propertyImprovementService.GetSelectListAsync();
            return View(vm);
        }

        var result = await _propertyService.EditPropertyAsync(vm);
       
        if (result.IsFailure)
        {
            var msg = result.GeneralError ?? string.Join("; ", result.Errors!);
            return Problem(msg);
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _propertyService.GetByIdForDeleteAsync(id);
        if (result.IsFailure)
        {
            var msg = result.GeneralError ?? string.Join("; ", result.Errors!);
            return Problem(msg);
        }
        var vm = _mapper.Map<PropertyDeleteViewModel>(result.Value);
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _propertyService.DeletePropertyAsync(id);
        
        if (result.IsFailure)
        {
            var msg = result.GeneralError ?? string.Join("; ", result.Errors!);
            return Problem(msg);
        }
        
        return RedirectToAction("Index");
    }
}