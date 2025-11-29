using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.SaleType;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Extensions;

namespace RealStateApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = $"{nameof(Roles.Admin)}")] 
public class SaleTypeController : Controller
{
    private readonly ISaleTypeService _saleTypeService;
    private readonly IMapper _mapper;

    public SaleTypeController(IMapper mapper, ISaleTypeService saleTypeService)
    {
        _mapper = mapper;
        _saleTypeService = saleTypeService;
    }


    public async Task<IActionResult> Index()
    {
        var properties = await _saleTypeService.GetAllSaleTypeWithCountAsync();
        var model = _mapper.Map<List<SaleTypeWithCountViewModel>>(properties);
        return View(model);
    }

    public IActionResult CreateSaleType()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateSaleType(CreateSaleTypeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = _mapper.Map<SaleTypeDto>(model);
        var createResult = await _saleTypeService.AddAsync(dto);
        if (createResult.IsFailure)
        {
            this.SendValidationErrorMessages(createResult);
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> EditSaleType(int id)
    {
        var model = new EditSaleTypeViewModel() 
        {
            Id = 0,
            Name = "",
            Description = "",
        };
        
        var saleTypeDto = await _saleTypeService.GetByIdAsync(id);
        if (saleTypeDto == null)
        {
            ViewBag.Message = "No se encontró tipo de venta con ese ID";
            return View(model);
        }
        model = _mapper.Map<EditSaleTypeViewModel>(saleTypeDto);
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditSaleType(EditSaleTypeViewModel model)
    {

        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var dto = _mapper.Map<SaleTypeDto>(model);
        var updateResult = await _saleTypeService.UpdateAsync(dto.Id, dto);
        if (updateResult.IsFailure)
        {
            this.SendValidationErrorMessages(updateResult);
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> DeleteSaleType(int id)
    {
        var model = new SaleTypeViewModel() 
        {
            Id = 0,
            Name = "",
            Description = ""
        };
        
       var saleTypeDto = await _saleTypeService.GetByIdAsync(id);
        if (saleTypeDto == null)
        {
            ViewBag.Message = "No se encontró algún tipo de venta con ese ID";
            return View(model);
        }
        
        model = _mapper.Map<SaleTypeViewModel>(saleTypeDto);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSaleType(SaleTypeViewModel model)
    {
        await _saleTypeService.DeleteAsync(model.Id);
        return RedirectToAction(nameof(Index));
    }
}