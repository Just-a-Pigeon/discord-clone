using signalRChatMVC.Models;

namespace signalRChatMVC.Services.Interfaces;

public interface IApiService 
{
    Task<string> Login(string username,string password);
    Task<bool> Register(string username, string firstname, string lastname, string password,string email);
    Task SendMessage(string sender, string roomName, string content, DateTime timestamp,string token);
    Task<List<MessageModel>> GetMessages(string roomId,string token);
}