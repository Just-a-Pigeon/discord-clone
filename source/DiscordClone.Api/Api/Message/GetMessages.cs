using DiscordClone.Contract.Rest.Response.Message;
using DiscordClone.Domain.Entities.Consultation;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Message;

public class GetMessages(DiscordCloneContext dbContext) : Endpoint<GetMessages.Request, IList<MessageResponseDto>>
{
    public override void Configure()
    {
        Get("messages");
        Group<Messages>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var messages = await dbContext.Messages.Where(m => m.Type == MessageType.Group && m.Receiver == req.RoomId)
            .ToListAsync(ct);

        var result = messages.Select(m => new MessageResponseDto
        {
            Content = m.Content,
            Date = m.CreatedOn,
            Sender = m.Sender
        }).ToList();

        await SendOkAsync(result, ct);
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
            ExampleRequest = new MessageResponseDto
            {
                Content = "Hello World!",
                Sender = Guid.NewGuid(),
                Date = DateTime.Now
            };
            Response(200, "Got all messages");

            Response<ErrorResponse>(401, "cloud not get all messages");
            Response<ErrorResponse>(400, "Client side error");
        }
    }

    public class Request
    {
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
    }
}