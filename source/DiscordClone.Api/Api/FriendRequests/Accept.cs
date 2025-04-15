using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.FriendRequests;

public class Accept(DiscordCloneContext dbContext) : Endpoint<Accept.Request>
{
    public override void Configure()
    {
        Post("accept/{FriendshipId:guid}");
        Group<FriendRequests>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var friendRequest = await dbContext.Friendships
            .SingleOrDefaultAsync(f => f.Id == req.FriendshipId && f.FriendId == req.UserId, ct);
        
        if (friendRequest is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var result = friendRequest.Accept();
        
        if (result.IsFailure)
        {
            ThrowError(result.Error.Reason);
        }
        
        await dbContext.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }

    public class Request : IHasUserId
    {
        [HideFromDocs]
        public Guid UserId { get; set; }
        public Guid FriendshipId { get; set; }
    }
}