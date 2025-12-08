using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.PropertyImage;
using RealStateApp.Core.Application.Dtos.PropertyImprovement;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.User;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
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
    IBaseAccountService baseAccountService,
    IOfferService offerService,
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
        var result = await _propertyService.GetAllByAgentIdAsync(agentId!);
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
        var result = await _propertyService.GetByIdAsync(id);

        if (result == null)
        {
            ViewBag.Message = "No se encontro una propiedad con ese ID";
            return RedirectToAction("Index");
        }

        var vm = new PropertyEditViewModel
        {
            Id = result.Id,
            Code = result.Code,
            PropertyTypeId = result.PropertyTypeId,
            SaleTypeId = result.SaleTypeId,
            Price = result.Price,
            SizeInMeters = result.SizeInMeters,
            Rooms = result.Rooms,
            Bathrooms = result.Bathrooms,
            Description = result.Description,
            MainImage = null,
            SelectedImprovements = result.PropertyImprovements.Select(pi => pi.ImprovementId).ToList(),
            PropertyTypes = await _propertyTypeService.GetSelectListAsync(),
            SaleTypes = await _saleTypeService.GetSelectListAsync(),
            Improvements = await _propertyImprovementService.GetSelectListAsync()
        };
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

        var propertyDto = new PropertyDto
        {
            Id = vm.Id,
            Code = vm.Code,
            PropertyTypeId = vm.PropertyTypeId,
            SaleTypeId = vm.SaleTypeId,
            Price = vm.Price,
            SizeInMeters = vm.SizeInMeters,
            Rooms = vm.Rooms,
            Bathrooms = vm.Bathrooms,
            Description = vm.Description,
            CreatedAt = DateTime.MinValue,
            AgentId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "",
        };
        var result = await _propertyService.EditPropertyAsync(propertyDto);
        if (result.IsFailure)
        {
            this.SendValidationErrorMessages(result);
            return RedirectToAction("Index");
        }
        var images = await _propertyImageService.GetAllImagesOfThisProperty(propertyDto.Id);
        

            // NEW MAIN IMAGE
            if (vm.MainImage != null)
            {
                var oldMainImage = images.FirstOrDefault(i => i.IsMain)!;
                oldMainImage.ImagePath = FileHandler.Upload(vm.MainImage, propertyDto.Id.ToString(), "properties", true, oldMainImage.ImagePath) ?? "";
                await _propertyImageService.UpdateAsync(oldMainImage.Id, oldMainImage);
            }

            // NEW ADDITIONAL IMAGES
            if (vm.AdditionalImages != null && vm.AdditionalImages.Any())
            {
                await _propertyImageService.DeleteAllAdditionalImagesOfThisPropertyAsync(propertyDto.Id);
                foreach (var file in vm.AdditionalImages)
                {
                    var path = FileHandler.Upload(file, propertyDto.Id.ToString(), "properties");
                    await _propertyImageService.AddAsync(new PropertyImageDto
                    {
                        PropertyId = propertyDto.Id,
                        ImagePath = path!,
                        IsMain = false,
                        Id = 0
                    });
                }
            }

            // DELETE OLD IMPROVEMENTS
            await _propertyImprovementService.DeleteAllImprovementsOfAPropertyAsync(propertyDto.Id);
            
            // ADD NEW IMPROVEMENTS
            foreach (var impId in vm.SelectedImprovements)
            {
                await _propertyImprovementService.AddAsync(new PropertyImprovementDto
                {
                    PropertyId = propertyDto.Id,
                    ImprovementId = impId,
                    Id = 0
                });
            }
       
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _propertyService.GetByIdForDeleteAsync(id);
        if (result.IsFailure)
        {
            this.SendValidationErrorMessages(result);
            return RedirectToAction("Index");
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
    
    public async Task<IActionResult> Details(int id)
    {
        var vm = new AgentDetailsViewModel
        {
            ChatClients = 
            _mapper.Map<List<UserViewModel>>(await baseAccountService.GetAllUserOfRole(Roles.Client)),
            OffersClients = _mapper.Map<List<UserViewModel>>(await offerService.GetAllUsersWhoHasOfferOnThisProperty(id)),
            Property = _mapper.Map<PropertyViewModel>(await _propertyService.GetByIdAsync(id)),
        };
        return View(vm);
    }
    
}