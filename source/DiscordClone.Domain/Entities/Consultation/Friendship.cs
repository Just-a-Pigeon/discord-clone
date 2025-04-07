using CSharpFunctionalExtensions;

namespace DiscordClone.Domain.Entities.Consultation;

public class Friendship
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid FriendId { get; private set; }
    public FriendshipStatus Status { get; private set; }
    public DateTime DateOfFriendship { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public virtual ApplicationUser User { get; private set; }
    public virtual ApplicationUser Friend { get; private set; }

    // ReSharper disable once UnusedMember.Local
    private Friendship()
    {
        // Required by EFCore
    }
    
    public static Result<Friendship, ValidationError> Create(Guid userId, Guid friendId)
    {
        return new Friendship
        {
            UserId = userId,
            FriendId = friendId,
            Status = FriendshipStatus.Pending,
            DateOfFriendship = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };
    }

    public Result<Friendship, ValidationError> Accept()
    {
        Status = FriendshipStatus.Accepted;
        DateOfFriendship = DateTime.UtcNow;

        return this;
    }

    public Result<Friendship, ValidationError> Reject()
    {
        Status = FriendshipStatus.Rejected;
        DateOfFriendship = DateTime.UtcNow;

        return this;
    }
}


public enum FriendshipStatus
{
    Pending,
    Accepted,
    Rejected
}