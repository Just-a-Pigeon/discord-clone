using MongoDB.Bson;

namespace signalRchat.Core.Entities;

public class Message
{
    public ObjectId Id { get; set; }
    public string Sender { get; set; }
    public string RoomName { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}