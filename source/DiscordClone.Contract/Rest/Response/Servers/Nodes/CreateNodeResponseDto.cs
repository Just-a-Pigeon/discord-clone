namespace DiscordClone.Contract.Rest.Response.Servers.Nodes;

public class CreateNodeResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool IsPrivate { get; set; }
    public bool IsAgeRestricted { get; set; }
}