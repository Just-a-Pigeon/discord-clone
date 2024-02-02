using signalRchat.Core.Entities;

namespace signalRchat.Core.Repositories.Interfaces;

public interface IFriendshipRepository
{
     Task SendRequest(Guid userId, Guid friendId);
     Task<ICollection<Friendship>> GetPendingRequest(Guid userId);
     Task AcceptRequest(Guid userId, Guid friendId);
     Task RejectRequest(Guid userId, Guid friendId);
     Task<ICollection<ApplicationUser>> GetFriends(Guid userId);
}