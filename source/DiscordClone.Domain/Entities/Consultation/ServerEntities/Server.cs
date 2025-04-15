using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;

namespace DiscordClone.Domain.Entities.Consultation.ServerEntities;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class Server
{
    private Server()
    {
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string? ImagePath { get; private set; }
    public string? Description { get; private set; }
    public string? BannerImagePath { get; private set; }
    public bool ReadyToDelete { get; private set; }
    public IReadOnlyCollection<ServerNode> ServerNodes { get; private set; } = null!;
    public IReadOnlyCollection<ServerInviteUrl> InviteUrls { get; private set; } = null!;
    public IReadOnlyCollection<ServerRole> Roles { get; private set; } = null!;
    public IReadOnlyCollection<ServerMember> Members { get; private set; } = null!;
    public ICollection<ApplicationUser> Banned { get; private set; } = null!;
    
    public static Result<Server, ValidationError> Create(string name, string? imagePath, Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ValidationError.InvalidInput("Name cannot be empty.", "name");
        if (name.Length is > 25 or < 2)
            return ValidationError.InvalidInput("Name must be between 3 and 25 characters.", "name");

        if (string.IsNullOrWhiteSpace(imagePath) && !Path.IsPathRooted(imagePath))
            return ValidationError.InvalidInput("Image file path must be absolute.", "imagePath");

        var server = new Server
        {
            Name = name,
            ImagePath = imagePath,
            Roles = new List<ServerRole> { ServerRole.CreateDefault() },
            Members = new List<ServerMember> { ServerMember.CreateOwner(ownerId).Value },
        };

        return server;
    }

    public UnitResult<ValidationError> Update(string? name, string? imagePath, string? bannerImagePath,
        string? description, Guid userId)
    {
        var member = Members.SingleOrDefault(m => m.UserId == userId);
        if (member is null)
            return ValidationError.InvalidInput("User is not a Member", "user");

        var permissions = member.GetPermissions();
        if (!member.IsOwner && (permissions.GeneralPermissions & ServerPermission.Administrator) != 0 &&
            (permissions.ServerPermissions & ServerPermissionServer.ManageServer) != 0)
            return ValidationError.InvalidInput("User does not have rights to edit this server.", "user");

        if (name is not null && string.IsNullOrWhiteSpace(name))
            return ValidationError.InvalidInput("Name cannot be empty.", "name");
        if (name?.Length is > 25 or < 2)
            return ValidationError.InvalidInput("Name must be between 3 and 25 characters.", "name");
        if (name is not null)
            Name = name;

        if (imagePath is not null && Path.IsPathRooted(imagePath))
            return ValidationError.InvalidInput("Image file path must be absolute.", "imagePath");

        ImagePath = imagePath;

        if (bannerImagePath is not null && Path.IsPathRooted(bannerImagePath))
            return ValidationError.InvalidInput("Image file path must be absolute.", "bannerImagePath");
        BannerImagePath = bannerImagePath;

        if (description?.Length > 1000)
            return ValidationError.InvalidInput("Description cannot be longer than 1000 characters.", "description");

        Description = description;

        return UnitResult.Success<ValidationError>();
    }
}