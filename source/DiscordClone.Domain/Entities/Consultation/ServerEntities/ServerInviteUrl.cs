using CSharpFunctionalExtensions;

namespace DiscordClone.Domain.Entities.Consultation.ServerEntities;

public class ServerInviteUrl
{
    public Guid Id { get; private set; }
    public Guid ServerId { get; private set; }
    public string UriParameter { get; private set; } = null!;
    public string? Name { get; private set; }
    public int AmountOfUses { get; private set; }
    public int Uses { get; private set; }
    public DateTimeOffset? ValidTill { get; private set; }

    public Server Server { get; private set; } = null!;

    public bool Revoked { get; private set; }

    public static Result<ServerInviteUrl, ValidationError> Create(string uriParameter, string? name, int amountOfUses,
        DateTimeOffset? validTill, Guid serverId)
    {
        if (string.IsNullOrWhiteSpace(uriParameter))
            return ValidationError.InvalidInput("uriParameter cannot be empty.", "uriParameter");
        if (uriParameter.Length > 15)
            return ValidationError.InvalidInput("uriParameter must be less than 15 characters.", "uriParameter");
        if (uriParameter.Length < 5)
            return ValidationError.InvalidInput("uriParameter must have more than 5 characters.", "uriParameter");
        
        if (amountOfUses < -1)
            return ValidationError.InvalidInput("amountOfUses cannot be negative.", "amountOfUses");
        if (amountOfUses == 0)
            return ValidationError.InvalidInput("amountOfUses cannot be zero.", "amountOfUses");
        
        if (validTill != null && validTill > DateTimeOffset.UtcNow)
            return ValidationError.InvalidInput("validTill cannot be in the past.", "validTill");
        if (validTill != null && validTill > DateTimeOffset.UtcNow.AddMinutes(15))
            return ValidationError.InvalidInput("validTill must be at least 15min in the future.", "validTill");
        
        return new ServerInviteUrl
        {
            UriParameter = uriParameter,
            Name = name,
            AmountOfUses = amountOfUses,
            ValidTill = validTill,
            Uses = 0,
            ServerId = serverId,
            Revoked = false
        };
    }

    public void Revoke()
    {
        Revoked = true;
    }

    public void Accept()
    {
        Uses++;
    }
}