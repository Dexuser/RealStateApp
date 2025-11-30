using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.ChatMessage;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class ChatMessageService : GenericServices<ChatMessage, ChatMessageDto>, IChatMessageService
{
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IAccountServiceForWebApp _accountServiceForWebApp;
    private readonly IMapper _mapper;

    public ChatMessageService(IChatMessageRepository repository, IMapper mapper,
        IAccountServiceForWebApp accountServiceForWebApp) : base(repository, mapper)
    {
        _chatMessageRepository = repository;
        _mapper = mapper;
        _accountServiceForWebApp = accountServiceForWebApp;
    }


    public async Task<List<ChatMessageDto>> GetChatMessagesOfThisProperty(
        string clientId, string agentId, int propertyId)
    {
        var client = await _accountServiceForWebApp.GetUserById(clientId);
        var agent = await _accountServiceForWebApp.GetUserById(agentId);

        if (client == null || agent == null)
        {
            return [];
        }

        var chatMessages = await _chatMessageRepository.GetAllQueryable()
            .AsNoTracking()
            .Where(chat => chat.PropertyId == propertyId &&
                           (
                               (chat.ReceiverId == clientId && chat.SenderId == agentId) ||
                               (chat.SenderId == clientId && chat.ReceiverId == agentId)
                           ))
            .OrderBy(chat => chat.SentAt)
            .Select(chat => new ChatMessageDto
            {
                Id = chat.Id,
                PropertyId = chat.PropertyId,
                SenderId = chat.SenderId,
                ReceiverId = chat.ReceiverId,
                Message = chat.Message, 
                SentAt = chat.SentAt,
            })
            .ToListAsync();

        chatMessages.ForEach(chatMessage =>
        {
            if (chatMessage.SenderId == clientId)
            {
                chatMessage.Receiver = agent;
                chatMessage.Sender = client;
            }
            else
            {
                chatMessage.Receiver = client;
                chatMessage.Sender = agent;
            }
        });

        return chatMessages;
    }


}