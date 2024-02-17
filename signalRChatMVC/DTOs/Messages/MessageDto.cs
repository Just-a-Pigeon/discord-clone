namespace signalRChatMVC.DTOs.Messages;

public class MessageDto
{
    public string Sender { get; set; }
    public string RoomName { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
}