using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers.Invites;

//TODO: Documentation
public class RevokeInvite(DiscordCloneContext dbContext) : Endpoint<RevokeInvite.Request>
{
    public override void Configure()
    {
        Put("{InviteId:guid}/revoke");
        Group<Invites>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var server = await dbContext.Servers.SingleOrDefaultAsync(s => s.Id == req.ServerId, ct);

        if (server is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var member =
            await dbContext.ServerMembers.SingleOrDefaultAsync(
                sm => sm.UserId == req.UserId && sm.ServerId == req.ServerId, ct);

        if (member is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (!member.CanManageServer())
        {
            await SendForbiddenAsync(ct);
            return;
        }

        var invite =
            await dbContext.ServerInviteUrls.SingleOrDefaultAsync(
                siu => siu.Id == req.InviteId && siu.ServerId == req.ServerId, ct);

        if (invite is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        invite.Revoke();
        await dbContext.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }

    public class Request : IHasUserId
    {
        public Guid ServerId { get; set; }
        public Guid InviteId { get; set; }

        [HideFromDocs] public Guid UserId { get; set; }
    }

    public class MyValidator : Validator<Request>
    {
        public MyValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty()
                .WithMessage("UserId is required");

            RuleFor(x => x.ServerId)
                .NotNull()
                .NotEmpty()
                .WithMessage("ServerId is required");

            RuleFor(x => x.InviteId)
                .NotNull()
                .NotEmpty()
                .WithMessage("InviteId is required");
        }
    }
}