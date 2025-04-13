using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using DomainGroup = DiscordClone.Domain.Entities.Consultation;

namespace DiscordClone.Api.Api.Group;

public class CreateGroup(DiscordCloneContext dbContext) : Endpoint<CreateGroup.Request>
{
    public override void Configure()
    {
        Post("Create");
        Group<Groups>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var members = await dbContext.Users.Where(u => req.MembersId.Contains(u.Id)).ToListAsync(ct);
        var group = DomainGroup.Group.CreateGroup(req.UserId, members);

        if (members.Exists(u => u.Id == req.UserId))
        {
            var leader = dbContext.Users.FirstOrDefault(u => u.Id == req.UserId);
            members.Add(leader);
        }
        
        dbContext.Add(group);
        await dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }

    public class Request : IHasUserId
    {
        public List<Guid> MembersId { get; set; }
        [HideFromDocs] public Guid UserId { get; set; }
    }
}