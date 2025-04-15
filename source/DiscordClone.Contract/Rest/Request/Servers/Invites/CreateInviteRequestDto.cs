namespace DiscordClone.Contract.Rest.Request.Servers.Invites;

public class CreateInviteRequestDto
{
    public string? Name { get; set; }
    public int AmountOfUses { get; set; }
    public DateTimeOffset? ValidTill { get; set; }
}