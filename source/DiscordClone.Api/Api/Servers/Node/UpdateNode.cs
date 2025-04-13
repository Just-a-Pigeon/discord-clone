using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers.Node;

public class UpdateNode(DiscordCloneContext dbContext) : Endpoint<UpdateNode.Reqeust>
{
    public override void Configure()
    {
        Put("{NodeId:guid}");
        Group<Nodes>();
    }

    public override async Task HandleAsync(Reqeust req, CancellationToken ct)
    {
        var member = await dbContext.ServerMembers
            .Include(sm => sm.Roles)
            .SingleOrDefaultAsync(sm => sm.UserId == req.UserId && sm.ServerId == req.ServerId, ct);

        if (member == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (!member.CanManageChannels())
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var node = await dbContext.ServerNodes
            .SingleOrDefaultAsync(sn => sn.Id == req.NodeId && sn.ServerId == req.ServerId, ct);

        if (node == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(ct);
    }

    public class Reqeust : IHasUserId
    {
        public Guid NodeId { get; set; }
        public Guid ServerId { get; set; }

        [HideFromDocs] public Guid UserId { get; set; }
    }
}