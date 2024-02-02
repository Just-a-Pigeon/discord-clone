
using signalRchat.Core.Entities;

namespace signalRchat.Core.Services.Interfaces;

public interface IFriendshipService
{
    public Task SendFriendRequest(Guid userId, Guid friendId);
    public Task<ICollection<Friendship>> GetPendingFriendRequest(Guid userId);
    public Task AcceptFriendRequest(Guid userId, Guid friendId);
    public Task RejectFriendRequest(Guid userId, Guid friendId);
    public Task<ICollection<ApplicationUser>> GetFriends(Guid userId);
    
}