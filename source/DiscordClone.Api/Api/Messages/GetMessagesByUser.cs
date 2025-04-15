using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Response.Messages;
using DiscordClone.Domain.Entities.Consultation;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Messages;

public class GetMessagesByUser(DiscordCloneContext dbContext)
    : Endpoint<GetMessagesByUser.Request, MessageResponseDto[]>
{
    public override void Configure()
    {
        Get("/user/{receiverId:guid}");
        Group<Api.Messages.Messages>();
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
            SenderId = m.Sender
        }).ToArray();

        await SendOkAsync(messages, ct);
    }

    public class Request : IHasUserId
    {
        public required Guid ReceiverId { get; set; }
        [HideFromDocs] public Guid UserId { get; set; }
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
            var response = new MessageResponseDto
            {
                Content = "Hello World!",
                CreatedOn = DateTimeOffset.UtcNow,
                SenderId = Guid.NewGuid()
            };

            Summary = "Get Messages by User";
            Description = "Get all messages by user";
            ExampleRequest = new Request
            {
                ReceiverId = Guid.NewGuid()
            };

            Response(200, "Got message by user", example: response);
            Response<ErrorResponse>(401, "cloud not get all messages");
            Response<ErrorResponse>(400, "Client side error");
        }
    }
}