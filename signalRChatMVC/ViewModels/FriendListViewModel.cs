using Microsoft.Build.Evaluation;
using signalRChatMVC.Models;

namespace signalRChatMVC.ViewModels;

public class FriendListViewModel
{
    public string SearchTerm { get; set; }
    public List<UserModel> Friends { get; set; } = new List<UserModel>();
    public List<UserModel> SearchResults { get; set; } = new List<UserModel>();
}