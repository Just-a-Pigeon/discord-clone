using signalRChatMVC.Models;

namespace signalRChatMVC.ViewModels;

public class FriendListViewModel
{
    public List<UserModel> Friends { get; set; } = new List<UserModel>();
}