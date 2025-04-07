using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Response.Friendship;
using DiscordClone.Domain.Entities.Consultation;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.FriendRequest;

public class Pending(DiscordCloneContext dbContext) : Endpoint<Pending.Request, PendingResponseDto[]>
{
    public override void Configure()
    {
        Get("pending");
        Group<FriendRequests>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var pendingFriendShip =
            await dbContext.Friendships.Include(f => f.Friend)
                .Where(f => f.FriendId == req.UserId && f.Status == FriendshipStatus.Pending)
                .ToListAsync(ct);

        var response = pendingFriendShip.Select(f => new PendingResponseDto
        {
            UserName = f.User.UserName!
        }).ToArray();
        
        await SendOkAsync(response, ct);
    }

    public class Request : IHasUserId
    {
        [HideFromDocs] public Guid UserId { get; set; }
    }
}