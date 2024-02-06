namespace signalRChatMVC.Services.Interfaces;

public interface IApiService
{
    Task<string> Login(string username,string password);
    Task Register(string username, string firstname, string lastname, string password);
    Task SendMessage(string sender, string roomName, string content, DateTime timestamp);
    Task GetMessages(string roomId);
}