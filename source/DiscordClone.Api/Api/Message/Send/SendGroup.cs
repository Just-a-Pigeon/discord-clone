﻿using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Request.Message;
using DiscordClone.Persistence;
using FastEndpoints;

namespace DiscordClone.Api.Api.Message.Send;

public class SendGroup(DiscordCloneContext dbContext) : Endpoint<SendGroup.Request>
{
    public override void Configure()
    {
        Post("group");
        Group<Send>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var message = Domain.Entities.Consultation.Message.CreateGroup(req.UserId,req.ReceiverId,req.Content,req.CreatedOn);
        
        dbContext.Add(message);
        await dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }


    public class Request : SendRequestDto, IHasUserId
    {
        public Guid UserId { get; set; }
    }
}