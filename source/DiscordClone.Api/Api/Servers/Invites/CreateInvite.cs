using CSharpFunctionalExtensions;
using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Request.Servers.Invites;
using DiscordClone.Contract.Rest.Response.Servers.Invites;
using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers.Invites;

public class CreateInvite(DiscordCloneContext dbContext) : Endpoint<CreateInvite.Request, CreateInviteResponseDto>
{
    public override void Configure()
    {
        Post("");
        Group<Invites>();
    }

    //TODO: Also see who made it, server insights
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

        if (!member.CanCreateInvites())
        {
            await SendForbiddenAsync(ct);
            return;
        }

        var uriParameter = Guid.NewGuid().ToString()[..20];

        var (_, isFailure, invite, error) = ServerInviteUrl.Create(uriParameter, req.Name ?? uriParameter,
            req.AmountOfUses, req.ValidTill, req.ServerId);

        if (isFailure)
            ThrowError(error.Reason);
        await dbContext.ServerInviteUrls.AddAsync(invite, ct);
        await dbContext.SaveChangesAsync(ct);

        var result = new CreateInviteResponseDto
        {
            UriParameter = invite.UriParameter,
            Name = invite.Name ?? invite.UriParameter,
            AmountOfUses = invite.AmountOfUses,
            ValidTill = invite.ValidTill
        };

        await SendOkAsync(result, ct);
    }

    public class Request : CreateInviteRequestDto, IHasUserId
    {
        public Guid ServerId { get; set; }
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
        }
    }
}