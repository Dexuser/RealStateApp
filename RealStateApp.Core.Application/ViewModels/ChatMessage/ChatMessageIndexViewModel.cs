namespace RealStateApp.Core.Application.ViewModels.ChatMessage;

public class ChatMessageIndexViewModel
{
    public required int PropertyId { get; set; }
    public required string PropertyCode { get; set; }
    public required string SenderId { get; set; }
    public required string ReceiverId { get; set; }
    public List<ChatMessageViewModel> Messages { get; set; } = [];
}