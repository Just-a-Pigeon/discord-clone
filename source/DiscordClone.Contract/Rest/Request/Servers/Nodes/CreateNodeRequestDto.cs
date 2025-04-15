namespace DiscordClone.Contract.Rest.Request.Servers.Nodes;

public class CreateNodeRequestDto
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool IsPrivate { get; set; }
    public bool IsAgeRestricted { get; set; }
    public Guid? Parent { get; set; }
}