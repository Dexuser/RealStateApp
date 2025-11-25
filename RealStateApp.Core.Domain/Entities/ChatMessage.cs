namespace RealStateApp.Core.Domain.Entities;

public class ChatMessage
{
    public required int Id { get; set; }
    public required int PropertyId { get; set; }
    public Property? Property { get; set; }
    public required string SenderId { get; set; }
    public required string ReceiverId { get; set; }
    public required string Message { get; set; }
    public DateTime SentAt { get; set; }
}
