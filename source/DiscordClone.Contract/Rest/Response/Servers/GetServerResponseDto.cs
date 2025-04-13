namespace DiscordClone.Contract.Rest.Response.Servers;

public class GetServerResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ImagePath { get; set; }
    public string? Description { get; set; }
    public string? BannerImage { get; set; }
    public IList<ServerNodesDto> Nodes { get; set; } = null!;
    public IList<MemberDto> Members { get; set; } = null!;

}

public class ServerNodesDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ChannelTopic { get; set; }
    public string Type { get; set; } = null!;
    public ServerNodesDto? Parent { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsAgeRestricted { get; set; }
    public IList<ServerNodesDto> Children { get; set; } = null!;
}

public class MemberDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public IList<RoleDto> Roles { get; set; } = null!;
}

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}