using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Improvement;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Extensions;

namespace RealStateApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = $"{nameof(Roles.Admin)}")] 
public class ImprovementController : Controller
{
    private readonly IImprovementService _improvementService;
    private readonly IMapper _mapper;

    public ImprovementController(IImprovementService improvementService, IMapper mapper)
    {
        _improvementService = improvementService;
        _mapper = mapper;
    }


    public async Task<IActionResult> Index()
    {
        var properties = await _improvementService.GetAllAsync();
        var model = _mapper.Map<List<ImprovementViewModel>>(properties);
        return View(model);
    }

    public IActionResult CreateImprovement()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateImprovement(CreateImprovementViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = _mapper.Map<ImprovementDto>(model);
        var createResult = await _improvementService.AddAsync(dto);
        if (createResult.IsFailure)
        {
            this.SendValidationErrorMessages(createResult);
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> EditImprovement(int id)
    {
        var model = new EditImprovementViewModel() 
        {
            Id = 0,
            Name = "",
            Description = "",
        };
        
        var improvementDto = await _improvementService.GetByIdAsync(id);
        if (improvementDto == null)
        {
            ViewBag.Message = "No se encontró una mejora con ese ID";
            return View(model);
        }
        model = _mapper.Map<EditImprovementViewModel>(improvementDto);
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditImprovement(EditImprovementViewModel model)
    {

        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var dto = _mapper.Map<ImprovementDto>(model);
        var updateResult = await _improvementService.UpdateAsync(dto.Id, dto);
        if (updateResult.IsFailure)
        {
            this.SendValidationErrorMessages(updateResult);
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> DeleteImprovement(int id)
    {
        var model = new ImprovementViewModel() 
        {
            Id = 0,
            Name = "",
            Description = ""
        };
        
       var improvementDto = await _improvementService.GetByIdAsync(id);
        if (improvementDto == null)
        {
            ViewBag.Message = "No se encontró algúna mejora con ese ID";
            return View(model);
        }
        
        model = _mapper.Map<ImprovementViewModel>(improvementDto);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteImprovement(ImprovementViewModel model)
    {
        // Los registros de PropertyImprovement están configurados para borrarse en cascada
        await _improvementService.DeleteAsync(model.Id);
        return RedirectToAction(nameof(Index));
    }
}