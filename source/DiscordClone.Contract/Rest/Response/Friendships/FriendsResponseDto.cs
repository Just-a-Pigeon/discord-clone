namespace DiscordClone.Contract.Rest.Response.Friendships;

public class FriendsResponseDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
}