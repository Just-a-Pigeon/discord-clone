using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers.Members;

//TODO: Documentation
public class BanMember(DiscordCloneContext dbContext) : Endpoint<BanMember.Request>
{
    public override void Configure()
    {
        Put("{MemberId:guid}/ban");
        Group<Members>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var server = await dbContext.Servers.Include(s => s.Banned)
            .Include(s => s.Members)
            .ThenInclude(sm => sm.Roles)
            .SingleOrDefaultAsync(s => s.Id == req.ServerId, ct);
        if (server == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var member = server.Members.SingleOrDefault(sm => sm.UserId == req.UserId);
        if (member == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (!member.CanBanMembers())
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var memberToBan = server.Members.SingleOrDefault(sm => sm.UserId == req.MemberId);
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

        [HideFromDocs] public Guid UserId { get; set; }
    }
}