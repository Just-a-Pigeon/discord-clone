using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers;

//TODO: Documentation
//TODO: Add consumer to delete all messages
public class DeleteServer(DiscordCloneContext dbContext) : Endpoint<DeleteServer.Request>
{
    public override void Configure()
    {
        Delete("{ServerId:guid}");
        Group<Servers>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var server = await dbContext.Servers.Include(s => s.Members).ThenInclude(m => m.Roles)
            .SingleOrDefaultAsync(s => s.Id == req.ServerId, ct);

        if (server is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var member = server.Members.SingleOrDefault(m => m.UserId == req.UserId);

        if (member is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (server.Members.Count > 1) ThrowError("Server has more than one member");

        if (!member.IsOwner) ThrowError("You must be owner to delete this server");

        await SendOkAsync(ct);
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
            RuleFor(x => x.ServerId)
                .NotNull()
                .NotEmpty()
                .WithMessage("ServerId is required");
        }
    }
    
    public class Documentation : Summary<DeleteServer>
    {
        public Documentation()
        {
            Summary = "Delete your server";
            Description = "Delete your server";
            
            Response(200, "Server has been successfully deleted.");
            Response(400, "Client side error.");
            Response(401, "Unauthorized");
            Response(404, "NotFound");
        }
    }
}