namespace signalRchat.Core.Entities;

public class Friendship
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid FriendId { get; set; }
    public FriendshipStatus Status { get; set; }
    public DateTime DateOfFriendship { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual ApplicationUser User { get; set; }
    public virtual ApplicationUser Friend { get; set; }
}

public enum FriendshipStatus
{
    Pending,
    Accepted,
    Rejected
}