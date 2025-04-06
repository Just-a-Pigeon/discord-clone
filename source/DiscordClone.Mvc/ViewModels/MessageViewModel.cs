using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using signalRChatMVC.Models;

namespace signalRChatMVC.ViewModels;

public class MessageViewModel
{
    
    public string Sender { get; set; }
    public string RoomName { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }

    public List<MessageModel> Messages { get; set; } = new List<MessageModel>();
}