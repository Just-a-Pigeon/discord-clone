using CSharpFunctionalExtensions;

namespace DiscordClone.Domain.Entities.Consultation.ServerEntities;

public class ServerMember
{
    private ServerMember()
    {
    }

    public bool IsOwner { get; private set; }
    public Guid UserId { get; private set; }
    public ApplicationUser User { get; private set; } = null!;
    public Guid ServerId { get; private set; }
    public Server Server { get; private set; } = null!;
    public ICollection<ServerRole> Roles { get; private set; } = null!;


    public static Result<ServerMember, ValidationError> Create(Guid userId, Guid serverId)
    {
        return new ServerMember
        {
            UserId = userId,
            ServerId = serverId,
            IsOwner = false,
            Roles = Array.Empty<ServerRole>()
        };
    }

    public static Result<ServerMember, ValidationError> CreateOwner(Guid userId)
    {
        return new ServerMember
        {
            UserId = userId,
            IsOwner = true,
            Roles = Array.Empty<ServerRole>()
        };
    }

    public ServerRolePermissions GetPermissions()
    {
        var generalPermissionsCombine = ServerPermission.None;
        var serverPermissionsCombine = ServerPermissionServer.None;
        var textChannelPermissionsCombine = ServerPermissionTextChannel.None;
        var voiceChannelPermissionsCombine = ServerPermissionVoiceChannel.None;

        foreach (var role in Roles)
        {
            var perms = role.Permissions;

            generalPermissionsCombine |= perms.GeneralPermissions;
            serverPermissionsCombine |= perms.ServerPermissions;
            textChannelPermissionsCombine |= perms.TextChannelPermissions;
            voiceChannelPermissionsCombine |= perms.VoiceChannelPermissions;
        }

        return ServerRolePermissions.Create(generalPermissionsCombine, serverPermissionsCombine,
            textChannelPermissionsCombine, voiceChannelPermissionsCombine);
    }

    public bool CanManageServer()
    {
        if (IsOwner)
            return true;

        if (Roles.Any(r =>
                (r.Permissions.GeneralPermissions & ServerPermission.Administrator) != 0 ||
                (r.Permissions.ServerPermissions & ServerPermissionServer.ManageServer) != 0))
            return true;

        return false;
    }

    public bool CanManageChannels()
    {
        if (IsOwner)
            return true;

        if (Roles.Any(r =>
                (r.Permissions.GeneralPermissions & ServerPermission.Administrator) != 0 ||
                (r.Permissions.ServerPermissions & ServerPermissionServer.ManageChannels) != 0 ||
                (r.Permissions.ServerPermissions & ServerPermissionServer.ManageServer) != 0))
            return true;

        return false;
    }

    public bool CanCreateInvites()
    {
        if (IsOwner)
            return true;

        if (Roles.Any(r =>
                (r.Permissions.GeneralPermissions & ServerPermission.Administrator) != 0 ||
                (r.Permissions.ServerPermissions & ServerPermissionServer.CreateInvites) != 0 ||
                (r.Permissions.ServerPermissions & ServerPermissionServer.ManageServer) != 0))
            return true;

        return false;
    }

    public bool CanBanMembers()
    {
        if (IsOwner)
            return true;

        if (Roles.Any(r =>
                (r.Permissions.GeneralPermissions & ServerPermission.Administrator) != 0 ||
                (r.Permissions.ServerPermissions & ServerPermissionServer.ManageServer) != 0 ||
                (r.Permissions.ServerPermissions & ServerPermissionServer.BanMembers) != 0))
            return true;

        return false;
    }

    public bool CanKickMembers()
    {
        if (IsOwner)
            return true;

        if (Roles.Any(r =>
                (r.Permissions.GeneralPermissions & ServerPermission.Administrator) != 0 ||
                (r.Permissions.ServerPermissions & ServerPermissionServer.ManageServer) != 0 ||
                (r.Permissions.ServerPermissions & ServerPermissionServer.KickMembers) != 0))
            return true;

        return false;
    }

    public bool CanManageRoles()
    {
        if (IsOwner)
            return true;

        if (Roles.Any(r =>
                (r.Permissions.GeneralPermissions & ServerPermission.Administrator) != 0 ||
                (r.Permissions.ServerPermissions & ServerPermissionServer.ManageServer) != 0 ||
                (r.Permissions.ServerPermissions & ServerPermissionServer.ManageRoles) != 0))
            return true;

        return false;
    }
}