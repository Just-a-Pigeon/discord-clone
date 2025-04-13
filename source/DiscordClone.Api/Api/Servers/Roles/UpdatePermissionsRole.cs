using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;

namespace DiscordClone.Api.Api.Servers.Roles;

public class UpdatePermissionsRole(DiscordCloneContext dbContext) : Endpoint<UpdatePermissionsRole.Request>
{
    public override void Configure()
    {
        Put("{RoleId:guid}/permissions");
        Group<Roles>();
    }

    public override Task HandleAsync(Request req, CancellationToken ct)
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