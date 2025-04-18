﻿using DiscordClone.Contract.Rest.Response.Message;
using DiscordClone.Domain.Entities.Consultation;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Message;

public class GetMessages(DiscordCloneContext dbContext) : Endpoint<GetMessages.Request, MessageResponseDto[]>
{
    public override void Configure()
    {
        Get("{roomId:guid}");
        Group<Messages>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var messages = await dbContext.Messages.Where(m => m.Type == MessageType.Group && m.Receiver == req.RoomId)
            .ToListAsync(ct);

        var result = messages.Select(m => new MessageResponseDto
        {
            Content = m.Content,
            CreatedOn = m.CreatedOn,
            SenderId = m.Receiver
        }).ToArray();

        await SendOkAsync(result, ct);
    }

    public class Request
    {
        [HideFromDocs] public Guid UserId { get; set; }

        public required Guid RoomId { get; set; }
    }

    public class MyValidator : Validator<Request>
    {
        public MyValidator()
        {
            RuleFor(x => x.RoomId)
                .NotEmpty()
                .WithMessage("RoomId is required");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("userId is required");
        }
    }

    public class Documentation : Summary<GetMessages>
    {
        public Documentation()
        {
            Summary = "Get all messages";
            Description = "Get all messages";
            ExampleRequest = new Request
            {
                RoomId = Guid.NewGuid()
            };
            Response(200, "Got all messages");

            Response<ErrorResponse>(401, "cloud not get all messages");
            Response<ErrorResponse>(400, "Client side error");
        }
    }
}