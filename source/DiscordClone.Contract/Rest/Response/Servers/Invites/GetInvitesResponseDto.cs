namespace DiscordClone.Contract.Rest.Response.Servers.Invites;

public class GetInvitesResponseDto
{
    public Guid Id { get; set; }
    public string UriParameter { get; set; } = null!;
    public string? Name { get; set; }
    public int AmountOfUses { get; set; }
    public int Uses { get; set; }
    public DateTimeOffset? ValidTill { get; set; }
}