﻿using DiscordClone.Api.Api.Binders;
using DiscordClone.Domain.Entities.Consultation;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.FriendRequest;

public class Send(DiscordCloneContext dbContext) : Endpoint<Send.Request>
{
    public override void Configure()
    {
        Post("/send");
        Group<FriendRequests>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var existingFriendship =
            await dbContext.Friendships
                .AnyAsync(f => (f.UserId == req.SendTo && f.FriendId == req.UserId) ||
                     (f.UserId == req.UserId && f.FriendId == req.SendTo), ct);

        if (existingFriendship) ThrowError("Friend request already exists");

        var friendship = Friendship.Create(req.UserId, req.SendTo);

        if (friendship.IsFailure)
        {
            ThrowError(friendship.Error.Reason);
        }
        
        dbContext.Friendships.Add(friendship.Value);
        await dbContext.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }

    public class Request : IHasUserId
    {
        [HideFromDocs] public Guid UserId { get; set; }

        public Guid SendTo { get; set; }
    }
}