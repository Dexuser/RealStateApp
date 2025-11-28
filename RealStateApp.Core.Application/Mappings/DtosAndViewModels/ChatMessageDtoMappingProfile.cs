using AutoMapper;
using RealStateApp.Core.Application.Dtos.ChatMessage;
using RealStateApp.Core.Application.ViewModels.ChatMessage;

namespace RealStateApp.Core.Application.Mappings.DtosAndViewModels;

public class ChatMessageDtoMappingProfile :  Profile
{
    public ChatMessageDtoMappingProfile()
    {
        CreateMap<ChatMessageDto, ChatMessageViewModel>().ReverseMap();
    }
}