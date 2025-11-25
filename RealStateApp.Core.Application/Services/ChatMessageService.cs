using AutoMapper;
using RealStateApp.Core.Application.Dtos.ChatMessage;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Services;

public class ChatMessageService : GenericServices<ChatMessage,ChatMessageDto> , IChatMessageService
{
    public ChatMessageService(IChatMessageRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }
}