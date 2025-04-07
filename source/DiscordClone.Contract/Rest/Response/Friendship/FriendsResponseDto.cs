namespace DiscordClone.Contract.Rest.Response.Friendship;

public class FriendsResponseDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
}