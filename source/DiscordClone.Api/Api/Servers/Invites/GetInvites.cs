﻿using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Response.Servers.Invites;
using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers.Invites;

public class GetInvites(DiscordCloneContext dbContext) : Endpoint<GetInvites.Request, GetInvitesResponseDto[]>
{
    public override void Configure()
    {
        Get("");
        Group<Invites>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var member = await dbContext.ServerMembers.Include(sm => sm.Roles)
            .SingleOrDefaultAsync(sm => sm.UserId == req.UserId && sm.ServerId == req.ServerId, ct);

        if (member is null)
            ThrowError("You must be a member to see invites.");
        
        var permissions = member.GetPermissions();
        if (!member.IsOwner && (permissions.ServerPermissions & ServerPermissionServer.ManageServer) == 0)
            ThrowError("You must be a member to see invites.");
        
        var invites = await dbContext.ServerInviteUrls.Where(siu => siu.ServerId == req.ServerId).ToListAsync(ct);

        var result = invites.Select(i => new GetInvitesResponseDto
        {
            Id = i.Id,
            UriParameter = i.UriParameter,
            Name = i.Name,
            AmountOfUses = i.AmountOfUses,
            Uses = i.Uses,
            ValidTill = i.ValidTill,
        }).ToArray();

        await SendOkAsync(result, ct);
    }

    public class Request : IHasUserId
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