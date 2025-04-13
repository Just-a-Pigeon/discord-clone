using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers;

public class DeleteServer(DiscordCloneContext dbContext) : Endpoint<DeleteServer.Request>
{
    public override void Configure()
    {
        Delete("{ServerId:guid}");
        Group<Servers>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        //TODO: Add consumer to delete all messages
        var server = await dbContext.Servers.Include(s => s.Members).ThenInclude(m => m.Roles).SingleOrDefaultAsync(s => s.Id == req.ServerId, ct);
        
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

        if (server.Members.Count > 1)
        {
            ThrowError("Server has more than one member");
        }
        
        if (!member.IsOwner)
        {
            ThrowError("You must be owner to delete this server");
        }
        
        await SendOkAsync(ct);
    }

    public class Request : IHasUserId
    {
        public Guid ServerId { get; set; }
        [HideFromDocs]
        public Guid UserId { get; set; }
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