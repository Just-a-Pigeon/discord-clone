using Microsoft.EntityFrameworkCore;
using signalRchat.Core.Entities;
using signalRchat.Core.Repositories.Interfaces;
using signalRchat.Infrastructure.Data;

namespace signalRchat.Infrastructure.Respositories;

public class FriendshipRepository : IFriendshipRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FriendshipRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task SendRequest(Guid userId, Guid friendId)
    {
        if (!await _dbContext.Friendships.AnyAsync(f =>
                (f.UserId == userId && f.FriendId == friendId) ||
                (f.UserId == friendId && f.FriendId == userId)))
        {
            var friendship = new Friendship { UserId = userId, FriendId = friendId, Status = FriendshipStatus.Pending };
            _dbContext.Friendships.Add(friendship);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<ICollection<Friendship>> GetPendingRequest(Guid userId)
    {
        return await _dbContext.Friendships.Where(f => f.FriendId == userId && f.Status == FriendshipStatus.Pending)
            .ToListAsync();
    }

    public async Task AcceptRequest(Guid userId, Guid friendId)
    {
        var friendship = await _dbContext.Friendships
            .SingleOrDefaultAsync(f =>
                f.UserId == friendId && f.FriendId == userId && f.Status == FriendshipStatus.Pending);

        if (friendship != null)
        {
            friendship.Status = FriendshipStatus.Accepted;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RejectRequest(Guid userId, Guid friendId)
    {
        var friendship = await _dbContext.Friendships
            .SingleOrDefaultAsync(f =>
                f.UserId == friendId && f.FriendId == userId && f.Status == FriendshipStatus.Pending);

        if (friendship != null)
        {
            friendship.Status = FriendshipStatus.Rejected;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<ICollection<ApplicationUser>> GetFriends(Guid userId)
    {
        return await _dbContext.Friendships
            .Where(f => (f.UserId == userId || f.FriendId == userId) && f.Status == FriendshipStatus.Accepted)
            .Select(f => f.UserId == userId ? f.Friend : f.User)
            .ToListAsync();
    }
}