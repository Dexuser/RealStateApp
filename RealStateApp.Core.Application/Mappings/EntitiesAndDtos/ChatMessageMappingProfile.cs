using AutoMapper;
using RealStateApp.Core.Application.Dtos.ChatMessage;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Mappings.EntitiesAndDtos;

public class ChatMessageMappingProfile :  Profile
{
    public ChatMessageMappingProfile()
    {
        CreateMap<ChatMessage, ChatMessageDto>();
    }
}