using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;

namespace DiscordClone.Api.Api.Servers.Roles;

public class GetRoles(DiscordCloneContext dbContext) : Endpoint<GetRoles.Request>
{
    public override void Configure()
    {
        Get("");
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