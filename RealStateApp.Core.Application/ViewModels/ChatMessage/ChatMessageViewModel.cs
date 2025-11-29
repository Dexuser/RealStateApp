using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Dtos.User;
using RealStateApp.Core.Application.ViewModels.Property;
using RealStateApp.Core.Application.ViewModels.User;

namespace RealStateApp.Core.Application.ViewModels.ChatMessage;

public class ChatMessageViewModel
{
    public required int Id { get; set; }
    public required int PropertyId { get; set; }
    public PropertyViewModel? Property { get; set; }
    public required string SenderId { get; set; }
    public required string ReceiverId { get; set; }
    public required string Message { get; set; }
    public DateTime SentAt { get; set; }
    
    public UserViewModel? Sender { get; set; }
    public UserViewModel? Receiver { get; set; }
}
