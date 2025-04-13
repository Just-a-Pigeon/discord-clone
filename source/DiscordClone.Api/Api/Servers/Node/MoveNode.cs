using DiscordClone.Api.Api.Binders;
using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers.Node;

//TODO: Documentation
//TODO: Make less complex
public class MoveNode(DiscordCloneContext dbContext) : Endpoint<MoveNode.Request>
{
    public override void Configure()
    {
        Put("{NodeId:guid}/move/{NewParentId:guid}");
        Group<Nodes>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var member = await dbContext.ServerMembers
            .Include(sm => sm.Roles)
            .SingleOrDefaultAsync(sm => sm.UserId == req.UserId && sm.ServerId == req.ServerId, ct);

        if (member is null || member.CanManageChannels())
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var node = dbContext.ServerNodes.SingleOrDefault(n => n.Id == req.NodeId && n.ServerId == req.ServerId);

        if (node == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var newParentNode =
            dbContext.ServerNodes.SingleOrDefault(n => n.Id == req.NewParentId && n.ServerId == req.ServerId);

        if (req.NewParentId is not null && newParentNode is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (newParentNode is not null && newParentNode.Type != ServerNodeType.Category)
            ThrowError("Cannot attach to this parent node");


        var result = node.MoveNode(newParentNode);

        if (result.IsFailure)
            ThrowError(result.Error.Reason);

        await dbContext.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }

    public class Request : IHasUserId
    {
        public Guid ServerId { get; set; }
        public Guid NodeId { get; set; }
        public Guid? NewParentId { get; set; }

        [HideFromDocs] public Guid UserId { get; set; }
    }
}