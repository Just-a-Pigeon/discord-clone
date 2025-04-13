namespace DiscordClone.Domain.Entities.Consultation.ServerEntities;

[Flags]
/// <summary>
///     General permissions
/// </summary>
public enum ServerPermission : ulong
{
    None = 0,

    /// <summary>
    ///     Master permission that overrides every other permission
    /// </summary>
    Administrator = 1 << 0
}

[Flags]
/// <summary>
///     Permissions for server level
/// </summary>
public enum ServerPermissionServer : ulong
{
    None = 0,

    /// <summary>
    ///     Able to view channels, channels are like text and voice channels
    /// </summary>
    ViewChannels = 1 << 0,

    /// <summary>
    ///     Able to manage channels inside the server
    /// </summary>
    ManageChannels = 1 << 1,

    /// <summary>
    ///     Able to manage roles inside the server
    /// </summary>
    ManageRoles = 1 << 2,

    /// <summary>
    ///     Able to view the logs of the server
    /// </summary>
    ViewAuditLog = 1 << 3,

    /// <summary>
    ///     Able to manage the server
    /// </summary>
    ManageServer = 1 << 4,

    /// <summary>
    ///     Able to create invites for the server
    /// </summary>
    CreateInvites = 1 << 5,

    /// <summary>
    ///     Able to change nickname of yourself
    /// </summary>
    ChangeNickname = 1 << 6,

    /// <summary>
    ///     Able to change nickname of others in the server
    /// </summary>
    ManageNicknames = 1 << 7,

    /// <summary>
    ///     Able to kick other members from the server
    /// </summary>
    KickMembers = 1 << 8,

    /// <summary>
    ///     Able to ban members  from the server
    /// </summary>
    BanMembers = 1 << 9,

    /// <summary>
    ///     Able to time out others from the server
    /// </summary>
    TimeOutMembers = 1 << 10
}

[Flags]
/// <summary>
///     Permissions for text channels level
/// </summary>
public enum ServerPermissionTextChannel : ulong
{
    None = 0,

    /// <summary>
    ///     Able to send messages inside text channels
    /// </summary>
    SendMessages = 1 << 0,

    /// <summary>
    ///     Able to attach files to message inside text channels
    /// </summary>
    AttachFiles = 1 << 1,

    /// <summary>
    ///     Able to add emoji reactions to messages inside text channels
    /// </summary>
    AddReactions = 1 << 2,

    /// <summary>
    ///     Able to mention @everybody, @here and all roles
    /// </summary>
    MentionRoles = 1 << 3,

    /// <summary>
    ///     Able to delete, pin other members messages
    /// </summary>
    ManageMessages = 1 << 4
}

[Flags]
/// <summary>
///     Permissions for voice channels level
/// </summary>
public enum ServerPermissionVoiceChannel : ulong
{
    None = 0,

    /// <summary>
    ///     Able to connect voice channels
    /// </summary>
    Connect = 1 << 0,

    /// <summary>
    ///     Able to speak in voice channels
    /// </summary>
    Speak = 1 << 1,

    /// <summary>
    ///     Able to use webcam, screen share or stream a game
    /// </summary>
    Video = 1 << 2,

    /// <summary>
    ///     Able to use voice activity function (if false then need push-to-talk)
    /// </summary>
    UseVoiceActivity = 1 << 3,

    /// <summary>
    ///     Able to mute other members for everybody
    /// </summary>
    MuteMembers = 1 << 4,

    /// <summary>
    ///     Able to deafen other members for everybody
    /// </summary>
    DeafenMembers = 1 << 5,

    /// <summary>
    ///     Able to move other members to other voice channels
    /// </summary>
    MoveMembers = 1 << 6
}