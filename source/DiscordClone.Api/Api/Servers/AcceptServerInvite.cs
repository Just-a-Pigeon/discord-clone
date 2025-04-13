using DiscordClone.Api.Api.Binders;
using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers;

//TODO: See who accepted and when? Server insights 
//TODO: Documentation
public class AcceptServerInvite(DiscordCloneContext dbContext) : Endpoint<AcceptServerInvite.Request>
{
    public override void Configure()
    {
        Put("invites/{UriParameter}/accept");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var invite =
            await dbContext.ServerInviteUrls.SingleOrDefaultAsync(siu => siu.UriParameter == req.UriParameter, ct);
        if (invite == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var server = await dbContext.Servers
            .Include(s => s.Banned)
            .Include(s => s.Members)
            .SingleOrDefaultAsync(s => s.Id == invite.ServerId, ct);

        if (server == null || server.Banned.Any(b => b.Id == req.UserId))
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (server.Members.Any(m => m.UserId == req.UserId))
            ThrowError("You are already in this server.");

        if (invite.AmountOfUses <= invite.Uses)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var member = ServerMember.Create(req.UserId, server.Id);

        if (member.IsFailure)
            ThrowError(member.Error.Reason);

        dbContext.ServerMembers.Add(member.Value);
        invite.Accept();

        await dbContext.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }

    public class Request : IHasUserId
    {
        public string UriParameter { get; set; } = null!;

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

            RuleFor(x => x.UriParameter)
                .NotNull()
                .NotEmpty()
                .WithMessage("UriParameter is required");
        }
    }

    public class Documentation : Summary<AcceptServerInvite>
    {
        public Documentation()
        {
            Summary = "Update server with a specified id";
            Description = "Update server with a specified id";
            ExampleRequest = new Request
            {
                UriParameter = "qwefq625qg355wef6q",
            };
            
            Response(200, "User is now member of this server.");
            Response(400, "Client side error.");
            Response(401, "User is not permitted to join this server.");
            Response(404, "Invite, user or server was not found.");
        }
    }
}