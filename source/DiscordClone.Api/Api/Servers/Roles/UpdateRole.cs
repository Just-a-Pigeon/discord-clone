using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;

namespace DiscordClone.Api.Api.Servers.Roles;

public class UpdateRole(DiscordCloneContext dbContext) : Endpoint<UpdateRole.Request>
{
    public override void Configure()
    {
        Put("{RoleId:guid}");
        Group<Roles>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public class Request : IHasUserId
    {
        public Guid RoleId { get; set; }
        public Guid ServerId { get; set; }

        [HideFromDocs] public Guid UserId { get; set; }
    }
}