namespace DiscordClone.Contract.Rest.Response.Servers.Invites;

public class CreateInviteResponseDto
{
    public string UriParameter { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int AmountOfUses { get; set; }
    public DateTimeOffset? ValidTill { get; set; }
}