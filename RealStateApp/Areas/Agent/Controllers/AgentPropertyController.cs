using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.PropertyImage;
using RealStateApp.Core.Application.Dtos.PropertyImprovement;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Extensions;
using RealStateApp.Handlers;

namespace RealStateApp.Areas.Agent.Controllers;
[Area("Agent")]
[Authorize(Roles = $"{nameof(Roles.Agent)}")] // Recuerda leer el apartado de seguridad de los requerimientos
public class AgentPropertyController(
    IPropertyService propertyService,
    IPropertyImprovementService propertyImprovementService,
    IPropertyTypeService propertyTypeService,
    ISaleTypeService saleTypeService,
    IPropertyImageService propertyImageService, 
    IMapper mapper)
    : Controller
{
    private readonly IPropertyService _propertyService = propertyService;
    private readonly IPropertyImprovementService _propertyImprovementService = propertyImprovementService;
    private readonly IPropertyTypeService _propertyTypeService = propertyTypeService;
    private readonly ISaleTypeService _saleTypeService = saleTypeService;
    private readonly IPropertyImageService _propertyImageService = propertyImageService;
    private readonly IMapper _mapper = mapper;
    // GET

    public async Task<IActionResult> Index()
    {
        var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _propertyService.GetPropertiesForMaintenanceAsync(agentId!);

        
        var vm = _mapper.Map<List<PropertyViewModel>>(result);
        
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

        var dto = new PropertyDto
        {
            Id = 0,
            Code = "",
            PropertyTypeId = vm.PropertyTypeId,
            SaleTypeId = vm.SaleTypeId,
            Price = vm.Price,
            SizeInMeters = vm.SizeInMeters,
            Rooms = vm.Rooms,
            Bathrooms = vm.Bathrooms,
            Description = vm.Description,
            CreatedAt = DateTime.Now,
            AgentId = agentId!
        };
        var result = await _propertyService.CreatePropertyAsync(dto);

        if (result.IsFailure)
        {
            this.SendValidationErrorMessages(result);
            return RedirectToAction("Index");
        }
        var propertyCreatedId = result.Value;
        
        // MAIN IMAGE
        if (vm.MainImage != null)
        {
            string imagePath = FileHandler.Upload(vm.MainImage, propertyCreatedId.ToString(), "properties")!;

            var mainImage = new PropertyImageDto
            {
                PropertyId = propertyCreatedId,
                ImagePath = imagePath,
                IsMain = true,
                Id = 0
            };

            await _propertyImageService.AddAsync(mainImage);
        }

        // ADDITIONAL IMAGES
        if (vm.AdditionalImages != null)
        {
            foreach (var img in vm.AdditionalImages)
            {
                string path = FileHandler.Upload(img, propertyCreatedId.ToString(), "properties")!;

                var imgEntity = new PropertyImageDto
                {
                    PropertyId = propertyCreatedId,
                    ImagePath = path,
                    IsMain = false,
                    Id = 0
                };

                await _propertyImageService.AddAsync(imgEntity);
            }
        }

        // IMPROVEMENTS
        if (vm.SelectedImprovements.Any())
        {
            foreach (var improvementId in vm.SelectedImprovements)
            {
                await _propertyImprovementService.AddAsync(new PropertyImprovementDto
                {
                    PropertyId = propertyCreatedId,
                    ImprovementId = improvementId,
                    Id = 0
                });
            }
        }
        
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _propertyService.GetByIdForEditAsync(id);
        
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
        
        return RedirectToAction("Index");
    }
}