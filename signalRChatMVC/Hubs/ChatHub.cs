using Microsoft.AspNetCore.SignalR;

namespace signalRChatMVC.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessageToGroup(string groupName, string message)
    {
        var timeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        await Clients.Group(groupName).SendAsync("ReceiveMessage", Context.User.Identity.Name, message, timeStamp);
        //return Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId}: {message}");
    }

    public async Task AddToGroup(string roomId)
    {
        string groupName = $"{roomId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        await Clients.Caller.SendAsync("RoomJoined", $"You joined room {groupName}");
    }

    public async Task RemoveFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
    }
    
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync($"ReceiveMessage", user, message, DateTime.Today.ToShortDateString());
    }

    public Task SendPrivateMessage(string user, string message)
    {
        return Clients.User(user).SendAsync("ReceiveMessage", message);
    }
    
}