namespace DiscordClone.Contract.Rest.Response.Servers.Roles;

public class CreateRoleResponseDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ulong GeneralPermissions { get; set; }
    public ulong ServerPermissions { get; set; }
    public ulong TextChannelPermissions { get; set; }
    public ulong VoiceChannelPermissions { get; set; }
}