using DiscordClone.Api.Api.Binders;
using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers.Members;

public class BanMember(DiscordCloneContext dbContext) : Endpoint<BanMember.Request> 
{
    public override void Configure()
    {
        Put("{MemberId:guid}/ban");
        Group<Members>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var server = await dbContext.Servers.Include(s => s.Banned).SingleOrDefaultAsync(s => s.Id == req.ServerId, ct);
        if (server == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        var member = await dbContext.ServerMembers.SingleOrDefaultAsync(sm => sm.ServerId == req.ServerId && sm.UserId == req.UserId, ct);
        if (member == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        var permissions = member.GetPermissions();
        if (!member.IsOwner && (permissions.GeneralPermissions & ServerPermission.Administrator) == 0 &&
            (permissions.ServerPermissions & ServerPermissionServer.BanMembers) == 0)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        var memberToBan = await dbContext.ServerMembers.SingleOrDefaultAsync(sm => sm.ServerId == req.ServerId && sm.UserId == req.UserId, ct);
        if (memberToBan == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        dbContext.ServerMembers.Remove(memberToBan);
        await dbContext.ServerMembers.ExecuteDeleteAsync(ct);
        server.Banned.Add(memberToBan.User);
        await dbContext.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }

    public class Request : IHasUserId
    {
        public Guid ServerId { get; set; }
        public Guid MemberId { get; set; }
        [HideFromDocs]
        public Guid UserId { get; set; }
    }
}