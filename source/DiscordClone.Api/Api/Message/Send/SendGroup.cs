using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Request.Message;
using DiscordClone.Contract.Rest.Response.Message;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;

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
        var message =
            Domain.Entities.Consultation.Message.CreateGroup(req.UserId, req.ReceiverId, req.Content, req.CreatedOn);

        dbContext.Add(message);
        await dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
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
                .WithMessage("ReceiverId is required");

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


    public class Request : SendRequestDto, IHasUserId
    {
        public Guid UserId { get; set; }
    }
}