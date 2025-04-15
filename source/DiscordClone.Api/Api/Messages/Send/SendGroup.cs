using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Request.Messages;
using DiscordClone.Contract.Rest.Response.Messages;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using DomainMessage = DiscordClone.Domain.Entities.Consultation.Message;

namespace DiscordClone.Api.Api.Messages.Send;

public class SendGroup(DiscordCloneContext dbContext) : Endpoint<SendGroup.Request>
{
    public override void Configure()
    {
        Post("group");
        Group<Api.Messages.Send.Send>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var message = DomainMessage.CreateGroup(req.UserId, req.ReceiverId, req.Content, req.CreatedOn);

        if (message.IsFailure)
            ThrowError(message.Error.Reason);
        
        dbContext.Add(message.Value);
        await dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }

    public class Request : SendRequestDto, IHasUserId
    {
        [HideFromDocs] public Guid UserId { get; set; }
    }

    public class MyValidator : Validator<Request>
    {
        public MyValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UseId is required");

            RuleFor(x => x.ReceiverId)
                .NotEmpty()
                .WithMessage("SenderId is required");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required")
                .MaximumLength(2000)
                .WithMessage("Content is too long");
        }
    }

    public class Documentation : Summary<SendGroup>
    {
        public Documentation()
        {
            Summary = "Send message to a group";
            Description = "Send message to a group";
            ExampleRequest = new MessageResponseDto
            {
                Content = "This is a message send to a group",
                CreatedOn = DateTimeOffset.Now,
                SenderId = Guid.NewGuid()
            };
            Response(200, "message was sent to the group");

            Response<ErrorResponse>(401, "cloud not sent the message to the group");
            Response<ErrorResponse>(400, "Client side error");
        }
    }
}