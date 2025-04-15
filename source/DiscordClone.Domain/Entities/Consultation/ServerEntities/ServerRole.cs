using CSharpFunctionalExtensions;

namespace DiscordClone.Domain.Entities.Consultation.ServerEntities;

public class ServerRole
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsDefault { get; private set; }
    
    public Guid ServerId { get; private set; }
    public Server Server { get; private set; } = null!;
    public ServerRolePermissions Permissions { get; private set; } = null!;

    public static ServerRole CreateDefault()
    {
        return new ServerRole
        {
            Name = "@everyone",
            Description = "@everyone",
            IsDefault = true,
            Permissions = ServerRolePermissions.CreateDefault()
        };
    }

    public static ServerRole Create()
    {
        return new ServerRole
        {
            Name = "new role",
            Description = "new role",
            IsDefault = false,
            Permissions = ServerRolePermissions.Create()
        };
    }

    public UnitResult<ValidationError> Update(string? name, string? description)
    {
        if (name is not null && string.IsNullOrWhiteSpace(name))
            Name = name;
        if (description is not null && string.IsNullOrWhiteSpace(description))
            Description = description;

        return UnitResult.Success<ValidationError>();
    }
}