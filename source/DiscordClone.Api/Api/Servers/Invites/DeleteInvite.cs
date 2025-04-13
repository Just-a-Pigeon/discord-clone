using DiscordClone.Api.Api.Binders;
using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers.Invites;

public class DeleteInvite(DiscordCloneContext dbContext) : Endpoint<DeleteInvite.Request>
{
    public override void Configure()
    {
        Put("{InviteId:guid}");
        Group<Invites>();
    }

    //TODO: Consumer to delete analytics
    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var server = await dbContext.Servers.SingleOrDefaultAsync(s => s.Id == req.ServerId, ct);

        if (server is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        var member = await dbContext.ServerMembers.SingleOrDefaultAsync(sm => sm.UserId == req.UserId && sm.ServerId == req.ServerId, ct);

        if (member is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        var permissions = member.GetPermissions();

        if (!member.IsOwner && (permissions.GeneralPermissions & ServerPermission.Administrator) == 0)
        {
            await SendForbiddenAsync(ct);
            return;
        }
        
        var invite = await dbContext.ServerInviteUrls.SingleOrDefaultAsync(siu => siu.Id == req.InviteId && siu.ServerId == req.ServerId, ct);

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