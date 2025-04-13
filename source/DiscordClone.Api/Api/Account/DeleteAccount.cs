using DiscordClone.Api.Api.Binders;
using DiscordClone.Api.ServiceBus.Commands;
using DiscordClone.Persistence;
using FastEndpoints;
using MassTransit;

namespace DiscordClone.Api.Api.Account;

public class DeleteAccount(DiscordCloneContext dbContext, IPublishEndpoint publishEndpoint) : Endpoint<DeleteAccount.Request>
{
    public override void Configure()
    {
        Delete("");
        Group<Accounts>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await publishEndpoint.Publish(new DeleteMessagesOfDeletedUser
        {
            UserId = req.UserId,
        });
        

    }

    public class Request : IHasUserId
    {
        [HideFromDocs] public Guid UserId { get; set; }
    }
}