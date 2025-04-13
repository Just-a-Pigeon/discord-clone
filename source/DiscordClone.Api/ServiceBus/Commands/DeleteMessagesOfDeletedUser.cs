namespace DiscordClone.Api.ServiceBus.Commands;

public record DeleteMessagesOfDeletedUser
{
    public Guid UserId { get; init; }
}