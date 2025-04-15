﻿using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;

namespace DiscordClone.Api.Api.Servers.Roles;

//TODO: Documentation
public class GetRole(DiscordCloneContext dbContext) : Endpoint<GetRole.Request>
{
    public override void Configure()
    {
        Get("{RoleId:guid}");
        Group<Roles>();
    }

    public override Task HandleAsync(Request req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public class Request : IHasUserId
    {
        public Guid RoleId { get; set; }
        public Guid ServerId { get; set; }

        [HideFromDocs] public Guid UserId { get; set; }
    }
}