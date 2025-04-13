using DiscordClone.Api.Api.Binders;
using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers.Node;

public class DeleteNode(DiscordCloneContext dbContext) : Endpoint<DeleteNode.Reqeust>
{
    public override void Configure()
    {
        Delete("{NodeId:guid}");
        Group<Nodes>();
    }

    //TODO: Create consumer to delete all messages
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
        
        dbContext.ServerNodes.Remove(node);
        await dbContext.ServerNodes.ExecuteDeleteAsync(ct);
        await dbContext.SaveChangesAsync(ct);
        
        await SendOkAsync(ct);
    }

    public class Reqeust : IHasUserId
    {
        public Guid NodeId { get; set; }
        public Guid ServerId { get; set; }

        [HideFromDocs] public Guid UserId { get; set; }
    }
}