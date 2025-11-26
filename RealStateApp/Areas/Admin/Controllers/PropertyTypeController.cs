using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.PropertyType;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Extensions;

namespace RealStateApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = $"{nameof(Roles.Admin)}")] 
public class PropertyTypeController : Controller
{
    private readonly IPropertyTypeService _propertyTypeService;
    private readonly IMapper _mapper;
    // GET
    public PropertyTypeController(IPropertyTypeService propertyTypeService, IMapper mapper)
    {
        _propertyTypeService = propertyTypeService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var properties = await _propertyTypeService.GetAllPropertyTypesWithCount();
        var model = _mapper.Map<List<PropertyTypeWithCountViewModel>>(properties);
        return View(model);
    }

    public IActionResult CreatePropertyType()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreatePropertyType(CreatePropertyTypeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = _mapper.Map<PropertyTypeDto>(model);
        var createResult = await _propertyTypeService.AddAsync(dto);
        if (createResult.IsFailure)
        {
            this.SendValidationErrorMessages(createResult);
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> EditPropertyType(int id)
    {
        var model = new EditPropertyTypeViewModel() 
        {
            Id = 0,
            Name = "",
            Description = "",
        };
        
        var propertyType = await _propertyTypeService.GetByIdAsync(id);
        if (propertyType == null)
        {
            ViewBag.Message = "No se encontro algun tipo de propiedad con ese ID";
            return View(model);
        }
        model = _mapper.Map<EditPropertyTypeViewModel>(propertyType);
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditPropertyType(EditPropertyTypeViewModel model)
    {

        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var dto = _mapper.Map<PropertyTypeDto>(model);
        var updateResult = await _propertyTypeService.UpdateAsync(dto.Id, dto);
        if (updateResult.IsFailure)
        {
            this.SendValidationErrorMessages(updateResult);
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> DeletePropertyType(int id)
    {
        var model = new PropertyTypeViewModel
        {
            Id = 0,
            Name = "",
            Description = ""
        };
        
       var propertyType = await _propertyTypeService.GetByIdAsync(id);
        if (propertyType == null)
        {
            ViewBag.Message = "No se encontro algun tipo de propiedad con ese ID";
            return View(model);
        }
        
        model = _mapper.Map<PropertyTypeViewModel>(propertyType);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeletePropertyType(PropertyTypeViewModel model)
    {
        await _propertyTypeService.DeleteAsync(model.Id);
        return RedirectToAction(nameof(Index));
    }
}