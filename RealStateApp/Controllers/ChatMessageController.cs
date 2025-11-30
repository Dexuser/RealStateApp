using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.ChatMessage;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.ChatMessage;
using RealStateApp.Core.Domain.Common;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Extensions;

namespace RealStateApp.Controllers;

[Authorize(Roles = $"{nameof(Roles.Agent)},{nameof(Roles.Client)}")]
public class ChatMessageController : Controller
{
    private readonly IChatMessageService _chatMessageService;
    private readonly IMapper _mapper;

    public ChatMessageController(IChatMessageService chatMessageService, IMapper mapper)
    {
        _chatMessageService = chatMessageService;
        _mapper = mapper;
    }

    // GET
    public async Task<IActionResult> Index(string clientId, string agentId, int propertyId, string propertyCode)
    {
        var messages = await _chatMessageService.GetChatMessagesOfThisProperty(clientId, agentId, propertyId);
        var model = new ChatMessageIndexViewModel
        {
            PropertyId = propertyId,
            SenderId = clientId,
            ReceiverId = agentId,
            PropertyCode = propertyCode,
            Messages = _mapper.Map<List<ChatMessageViewModel>>(messages),
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(ChatMessageViewModel messageViewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Message = "Escribe un mensaje para poder enviarlo";
            return View("index");
        }
        var dto = _mapper.Map<ChatMessageDto>(messageViewModel);
        dto.SentAt = DateTime.Now;
        var result = await _chatMessageService.AddAsync(dto);
        if (result.IsFailure)
        {
            this.SendValidationErrorMessages(result);
            return View("Index");
        }
        
        return RedirectToRoute(new 
        { 
            controller = "ChatMessage", 
            action = "Index", 
            clientId = messageViewModel.SenderId, 
            agentId = messageViewModel.ReceiverId, 
            propertyId = messageViewModel.PropertyId 
        });
    }
}