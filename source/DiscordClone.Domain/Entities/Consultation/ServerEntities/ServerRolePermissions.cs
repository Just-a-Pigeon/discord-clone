namespace DiscordClone.Domain.Entities.Consultation.ServerEntities;

public class ServerRolePermissions
{
    public ServerPermission GeneralPermissions { get; private set; }
    public ServerPermissionServer ServerPermissions { get; private set; }
    public ServerPermissionTextChannel TextChannelPermissions { get; private set; }
    public ServerPermissionVoiceChannel VoiceChannelPermissions { get; private set; }

    private ServerRolePermissions() { }

    /*
     * Grant permission
     * GeneralPermissions |= ServerPermission.Administrator;
     *
     * Remove permission
     * GeneralPermissions &= ServerPermission.Administrator;
     */
    //TODO: Look if able to put in a config file
    public static ServerRolePermissions CreateDefault()
    {
        var permissions = new ServerRolePermissions();
        permissions.GeneralPermissions &= ServerPermission.Administrator;

        permissions.ServerPermissions |= ServerPermissionServer.ViewChannels;
        permissions.ServerPermissions &= ServerPermissionServer.ManageChannels;
        permissions.ServerPermissions &= ServerPermissionServer.ManageRoles;
        permissions.ServerPermissions &= ServerPermissionServer.ViewAuditLog;
        permissions.ServerPermissions &= ServerPermissionServer.ManageServer;
        permissions.ServerPermissions |= ServerPermissionServer.CreateInvites;
        permissions.ServerPermissions |= ServerPermissionServer.ChangeNickname;
        permissions.ServerPermissions &= ServerPermissionServer.ManageNicknames;
        permissions.ServerPermissions &= ServerPermissionServer.KickMembers;
        permissions.ServerPermissions &= ServerPermissionServer.BanMembers;
        permissions.ServerPermissions &= ServerPermissionServer.TimeOutMembers;

        permissions.TextChannelPermissions |= ServerPermissionTextChannel.SendMessages;
        permissions.TextChannelPermissions |= ServerPermissionTextChannel.AttachFiles;
        permissions.TextChannelPermissions |= ServerPermissionTextChannel.AddReactions;
        permissions.TextChannelPermissions |= ServerPermissionTextChannel.MentionRoles;
        permissions.TextChannelPermissions &= ServerPermissionTextChannel.ManageMessages;

        permissions.VoiceChannelPermissions |= ServerPermissionVoiceChannel.Connect;
        permissions.VoiceChannelPermissions |= ServerPermissionVoiceChannel.Speak;
        permissions.VoiceChannelPermissions |= ServerPermissionVoiceChannel.Video;
        permissions.VoiceChannelPermissions |= ServerPermissionVoiceChannel.UseVoiceActivity;
        permissions.VoiceChannelPermissions &= ServerPermissionVoiceChannel.MuteMembers;
        permissions.VoiceChannelPermissions &= ServerPermissionVoiceChannel.DeafenMembers;
        permissions.VoiceChannelPermissions &= ServerPermissionVoiceChannel.MoveMembers;

        return permissions;
    }

    //TODO: Look if it automatically puts everything on false
    public static ServerRolePermissions Create()
    {
        var permissions = new ServerRolePermissions();
        permissions.GeneralPermissions &= ServerPermission.Administrator;

        permissions.ServerPermissions &= ServerPermissionServer.ViewChannels;
        permissions.ServerPermissions &= ServerPermissionServer.ManageChannels;
        permissions.ServerPermissions &= ServerPermissionServer.ManageRoles;
        permissions.ServerPermissions &= ServerPermissionServer.ViewAuditLog;
        permissions.ServerPermissions &= ServerPermissionServer.ManageServer;
        permissions.ServerPermissions &= ServerPermissionServer.CreateInvites;
        permissions.ServerPermissions &= ServerPermissionServer.ChangeNickname;
        permissions.ServerPermissions &= ServerPermissionServer.ManageNicknames;
        permissions.ServerPermissions &= ServerPermissionServer.KickMembers;
        permissions.ServerPermissions &= ServerPermissionServer.BanMembers;
        permissions.ServerPermissions &= ServerPermissionServer.TimeOutMembers;

        permissions.TextChannelPermissions &= ServerPermissionTextChannel.SendMessages;
        permissions.TextChannelPermissions &= ServerPermissionTextChannel.AttachFiles;
        permissions.TextChannelPermissions &= ServerPermissionTextChannel.AddReactions;
        permissions.TextChannelPermissions &= ServerPermissionTextChannel.MentionRoles;
        permissions.TextChannelPermissions &= ServerPermissionTextChannel.ManageMessages;

        permissions.VoiceChannelPermissions &= ServerPermissionVoiceChannel.Connect;
        permissions.VoiceChannelPermissions &= ServerPermissionVoiceChannel.Speak;
        permissions.VoiceChannelPermissions &= ServerPermissionVoiceChannel.Video;
        permissions.VoiceChannelPermissions &= ServerPermissionVoiceChannel.UseVoiceActivity;
        permissions.VoiceChannelPermissions &= ServerPermissionVoiceChannel.MuteMembers;
        permissions.VoiceChannelPermissions &= ServerPermissionVoiceChannel.DeafenMembers;
        permissions.VoiceChannelPermissions &= ServerPermissionVoiceChannel.MoveMembers;

        return permissions;
    }

    public static ServerRolePermissions Create(ServerPermission permission, ServerPermissionServer serverPermission,
        ServerPermissionTextChannel textChannel, ServerPermissionVoiceChannel voiceChannel)
    {
        var permissions = new ServerRolePermissions
        {
            GeneralPermissions = permission,
            ServerPermissions = serverPermission,
            TextChannelPermissions = textChannel,
            VoiceChannelPermissions = voiceChannel
        };
        return permissions;
    }
}