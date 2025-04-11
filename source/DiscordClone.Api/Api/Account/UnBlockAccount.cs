using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Account;

public class UnBlockAccount(DiscordCloneContext dbContext) : Endpoint<UnBlockAccount.Request>
{
    public override void Configure()
    {
        Get("{UnBlockUserId:Guid}/unblock-account");
        Group<Accounts>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = dbContext.Users
            .Include(u => u.BlockedUsers)
            .FirstOrDefault(u => u.Id == req.UserId);

        if (user == null)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        
        var unBlockUser = user.BlockedUsers.FirstOrDefault(u => u.Id == req.UnBlockUserId);

        if (unBlockUser == null)
        {
            await SendOkAsync(cancellation: ct);
            return;
        }
        
        user.BlockedUsers.Remove(unBlockUser);
        await dbContext.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }

    public class Request : IHasUserId
    {
        public Guid UnBlockUserId { get; set; }
        [HideFromDocs] public Guid UserId { get; set; }
    }

    public class MyValidator : Validator<Request>
    {
        public MyValidator()
        {
            RuleFor(x => x.UnBlockUserId)
                .NotEmpty()
                .WithMessage("UnBlockUserId is required");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required")
                .NotEqual(x => x.UnBlockUserId)
                .WithMessage("You cannot unblock yourself");
        }
    }

    public class Documentation : Summary<UnBlockAccount>
    {
        public Documentation()
        {
            Summary = "Unblock a user by Id";
            Description = "Unblock a user by Idd";
            ExampleRequest = new Request
            {
                UnBlockUserId = Guid.NewGuid()
            };
            Response(200, "User is blocked");
        }
    }
}