using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Account;

public class BlockAccount(DiscordCloneContext dbContext) : Endpoint<BlockAccount.Request>
{
    public override void Configure()
    {
        Get("{UnBlockUserId:Guid}/block-account");
        Group<Accounts>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = dbContext.Users
            .Include(u => u.Friends)
            .Include(u => u.FriendOf)
            .Include(u => u.BlockedUsers)
            .FirstOrDefault(u => u.Id == req.UserId);

        var blockedUser = dbContext.Users
            .FirstOrDefault(u => u.Id == req.BlockedUserId);

        if (user == null)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        if (blockedUser == null)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        if (user.BlockedUsers.Any(u => u.Id == req.BlockedUserId))
        {
            await SendOkAsync(ct);
            return;
        }

        if (user.Friends.Any(f => f.UserId == req.UserId))
        {
            var toRemove = user.Friends.FirstOrDefault(f => f.UserId == req.UserId);
            dbContext.Remove(toRemove!);
        }

        if (user.Friends.Any(f => f.FriendId == req.UserId))
        {
            var toRemove = user.Friends.FirstOrDefault(f => f.FriendId == req.UserId);
            dbContext.Remove(toRemove!);
        }

        user.BlockedUsers.Add(blockedUser);
        await dbContext.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }

    public class Request : IHasUserId
    {
        public Guid BlockedUserId { get; set; }
        [HideFromDocs] public Guid UserId { get; set; }
    }

    public class MyValidator : Validator<Request>
    {
        public MyValidator()
        {
            RuleFor(x => x.BlockedUserId)
                .NotEmpty()
                .WithMessage("UnBlockUserId is required");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required")
                .NotEqual(x => x.BlockedUserId)
                .WithMessage("You cannot block yourself");
            
        }
    }

    public class Documentation : Summary<BlockAccount>
    {
        public Documentation()
        {
            Summary = "Block a user by Id";
            Description = "Block a user by Idd";
            ExampleRequest = new Request
            {
                BlockedUserId = Guid.NewGuid()
            };
            Response(200, "User is blocked");
        }
    }
}