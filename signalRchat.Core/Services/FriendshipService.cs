using signalRchat.Core.Entities;
using signalRchat.Core.Repositories.Interfaces;
using signalRchat.Core.Services.Interfaces;

namespace signalRchat.Core.Services;

public class FriendshipService : IFriendshipService
{
    private readonly IFriendshipRepository _friendshipRepository;

    public FriendshipService(IFriendshipRepository friendshipRepository)
    {
        _friendshipRepository = friendshipRepository;
    }

    public async Task SendFriendRequest(Guid userId, Guid friendId)
    {
        await _friendshipRepository.SendRequest(userId, friendId);
    }

    public async Task<ICollection<Friendship>> GetPendingFriendRequest(Guid userId)
    {
        return await _friendshipRepository.GetPendingRequest(userId);
    }

    public async Task AcceptFriendRequest(Guid userId, Guid friendId)
    {
         await _friendshipRepository.AcceptRequest(userId, friendId);
    }

    public async Task RejectFriendRequest(Guid userId, Guid friendId)
    {
        await _friendshipRepository.RejectRequest(userId, friendId);
    }

    public async Task<ICollection<ApplicationUser>> GetFriends(Guid userId)
    {
        return await _friendshipRepository.GetFriends(userId);
    }
}