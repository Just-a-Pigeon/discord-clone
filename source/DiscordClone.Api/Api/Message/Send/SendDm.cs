using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Request.Message;
using DiscordClone.Contract.Rest.Response.Message;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using DomainMessage = DiscordClone.Domain.Entities.Consultation.Message;

namespace DiscordClone.Api.Api.Message.Send;

public class SendDm(DiscordCloneContext dbContext) : Endpoint<SendDm.Request>
{
    public override void Configure()
    {
        Post("personal-message");
        Group<Send>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = await dbContext.Users
            .Include(u => u.BlockedUsers)
            .FirstOrDefaultAsync(u => u.Id == req.UserId, ct);
        var receiver = await dbContext.Users
            .Include(u => u.BlockedUsers)
            .FirstOrDefaultAsync(u => u.Id == req.ReceiverId, ct);
        
        if (user == null || receiver == null)
            ThrowError("User/Receiver Not Found");
        
        if (user.BlockedUsers.Any(b => b.Id == req.ReceiverId) || 
            receiver.BlockedUsers.Any(b => b.Id == req.UserId))
            ThrowError("User/Receiver Not Blocked");
        
        var message =
            DomainMessage.CreateDm(req.UserId, req.ReceiverId, req.Content, req.CreatedOn);

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

    public class Documentation : Summary<SendDm>
    {
        public Documentation()
        {
            Summary = "Send message to the user dm";
            Description = "Send message to the user dm";
            ExampleRequest = new MessageResponseDto
            {
                Content = "This is a message send to the user dm",
                CreatedOn = DateTimeOffset.Now,
                SenderId = Guid.NewGuid()
            };
            Response(200, "message was sent to the user dm");

            Response<ErrorResponse>(401, "cloud not send the message to user dm");
            Response<ErrorResponse>(400, "Client side error");
        }
    }
}