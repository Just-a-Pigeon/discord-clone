using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Response.Friendships;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.FriendRequests;

public class Friends(DiscordCloneContext dbContext) : Endpoint<Friends.Request, FriendsResponseDto[]>
{
    public override void Configure()
    {
        Get("friends");
        Group<Api.FriendRequests.FriendRequests>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var friends = await dbContext.Friendships
            .Include(f => f.Friend)
            .Include(f => f.User)
            .Where(f => f.UserId == req.UserId || f.FriendId == req.UserId).ToListAsync(ct);

        var result = friends.Select(f =>
        {
            var user = f.UserId != req.UserId ? f.User : f.Friend;

            return new FriendsResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName!
            };
        }).ToArray();

        await SendOkAsync(result, ct);
    }

    public class Request : IHasUserId
    {
        [HideFromDocs]
        public Guid UserId { get; set; }
    }
}