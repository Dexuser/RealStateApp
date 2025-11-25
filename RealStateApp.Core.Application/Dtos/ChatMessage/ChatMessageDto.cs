using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Dtos.ChatMessage;

public class ChatMessageDto
{
    public required int Id { get; set; }
    public required int PropertyId { get; set; }
    public PropertyDto? Property { get; set; }
    public required string SenderId { get; set; }
    public required string ReceiverId { get; set; }
    public required string Message { get; set; }
    public DateTime SentAt { get; set; }
    
    public UserDto? Sender { get; set; }
    public UserDto? Receiver { get; set; }
}
