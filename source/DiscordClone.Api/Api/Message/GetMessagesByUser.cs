using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Response.Message;
using DiscordClone.Domain.Entities.Consultation;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Message;

public class GetMessagesByUser(DiscordCloneContext dbContext)
    : Endpoint<GetMessagesByUser.Request, IList<MessageResponseDto>>
{
    public override void Configure()
    {
        Get("/user/{userId:guid}");
        Group<Messages>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var myMessages = await dbContext.Messages.Where(m =>
                m.Sender == req.UserId && m.Receiver == req.ReceiverId && m.Type == MessageType.PersonalMessage &&
                m.CreatedOn < DateTimeOffset.UtcNow.AddDays(-3))
            .ToListAsync(ct);

        var receiverMessages = await dbContext.Messages.Where(m =>
            m.Sender == req.ReceiverId && m.Receiver == req.UserId && m.Type == MessageType.PersonalMessage &&
            m.CreatedOn < DateTimeOffset.UtcNow.AddDays(-3)).ToListAsync(ct);

        var messages = myMessages.Concat(receiverMessages).OrderBy(m => m.CreatedOn).Select(m => new MessageResponseDto
        {
            Content = m.Content,
            CreatedOn = m.CreatedOn,
            ReceiverId = m.Sender
        }).ToList();

        await SendOkAsync(messages, ct);
    }


    public class MyValidator : Validator<Request>
    {
        public MyValidator()
        {
            RuleFor(x => x.ReceiverId)
                .NotEmpty()
                .WithMessage("RoomId is required");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("userId is required");
        }
    }

    public class Documentation : Summary<GetMessagesByUser>
    {
        public Documentation()
        {
            Summary = "Get Messages by User";
            Description = "Get all messages by user";
            ExampleRequest = new MessageResponseDto
            {
                Content = "Hello World!",
                ReceiverId = Guid.NewGuid(),
                CreatedOn = DateTime.Now
            };
            Response(200, "Got all messages by user");

            Response<ErrorResponse>(401, "cloud not get all messages");
            Response<ErrorResponse>(400, "Client side error");
        }
    }

    public class Request : IHasUserId
    {
        public Guid ReceiverId { get; set; }
        public Guid UserId { get; set; }
    }
}