using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Group;

public class AddMembers(DiscordCloneContext dbContext) : Endpoint<AddMembers.Request>
{
    public override void Configure()
    {
        Put("{GroupId:guid}/add-members");
        Group<Groups>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var group = await dbContext.Groups.SingleOrDefaultAsync(g => g.Id == req.GroupId, ct);

        if (group == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (group.LeaderId != req.UserId)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var members = await dbContext.Users.Where(u => req.MemberIds.Contains(u.Id)).ToListAsync(ct);

        group.Members.AddRange(members);
        await dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }

    public class Request : IHasUserId
    {
        public Guid GroupId { get; set; }
        public required List<Guid> MemberIds { get; set; } = null!;
        [HideFromDocs] public Guid UserId { get; set; }
    }
}