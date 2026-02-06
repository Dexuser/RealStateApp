using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Offer;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Extensions;

namespace RealStateApp.Controllers;

public class OfferController : Controller
{
    private readonly IOfferService _offerService;
    private readonly IMapper _mapper;

    public OfferController(IOfferService offerService, IMapper mapper)
    {
        _offerService = offerService;
        _mapper = mapper;
    }

    // GET
    public async Task<IActionResult> Index(int propertyId, string clientId)
    {
        var offers = await _offerService.GetAllOffersOfThisClientOnThisProperty(clientId, propertyId);
        var model = new OfferListViewModel
        {
            PropertyId = propertyId,
            ClientId = clientId,
            CanCreateOffer = !offers.Any(o => o.Status == OfferStatus.Pending || o.Status == OfferStatus.Accepted),
            Offers = _mapper.Map<List<OfferViewModel>>(offers),
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OfferViewModel offerViewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Message = "Ocurrio un error";
            return View("Index");
        }
        var dto = _mapper.Map<OfferDto>(offerViewModel);
        dto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        var result = await _offerService.AddAsync(dto);
        if (result.IsFailure)
        {
            this.SendValidationErrorMessages(result);
            return View("Index");
        }
        
        return RedirectToAction(nameof(Index), 
            new
            {
                propertyId = offerViewModel.PropertyId,
                clientId = dto.UserId
            });
    }

    public async Task<IActionResult> RespondToOffers(int propertyId, string clientId)
    {
        var offers = await _offerService.GetAllOffersOfThisClientOnThisProperty(clientId, propertyId);
        var model = new OfferListViewModel
        {
            PropertyId = propertyId,
            ClientId = clientId,
            CanCreateOffer = !offers.Any(o => o.Status == OfferStatus.Pending || o.Status == OfferStatus.Accepted),
            Offers = _mapper.Map<List<OfferViewModel>>(offers),
        };
        return View(model);
    }
    
    [Authorize(Roles = $"{nameof(Roles.Agent)}")]
    [HttpPost]
    public async Task<IActionResult> Decide(int offerId, bool accepted)
    {
        var offers = await _offerService.RespondOffer(offerId, accepted);
       return RedirectToAction(nameof(RespondToOffers));
    }
}