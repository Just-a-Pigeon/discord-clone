using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;

namespace DiscordClone.Api.Api.Servers.Roles;

public class CreateRole(DiscordCloneContext dbContext) : Endpoint<CreateRole.Request>
{
    public override void Configure()
    {
        Post("");
        Group<Roles>();
    }

    public override Task HandleAsync(Request req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public class Request : IHasUserId
    {
        public Guid ServerId { get; set; }

        [HideFromDocs] public Guid UserId { get; set; }
    }
}