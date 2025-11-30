using RealStateApp.Core.Application.Dtos.ChatMessage;

namespace RealStateApp.Core.Application.Interfaces;

public interface IChatMessageService : IGenericService<ChatMessageDto>
{
    Task<List<ChatMessageDto>> GetChatMessagesOfThisProperty(string clientId, string agentId, int propertyId);
}