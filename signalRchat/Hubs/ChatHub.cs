using Microsoft.AspNetCore.SignalR;

namespace signalRchat.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync($"ReceiveMessage", user, message, DateTime.Today.ToShortDateString());
    }
    public Task SendPrivateMessage(string user, string message)
    {
        return Clients.User(user).SendAsync("ReceiveMessage", message);
    }
}