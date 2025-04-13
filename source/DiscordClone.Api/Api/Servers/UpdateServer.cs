using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Request.Servers;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers;

//TODO: Documentation
//TODO: Permission check
public class UpdateServer(DiscordCloneContext dbContext) : Endpoint<UpdateServer.Request>
{
    public override void Configure()
    {
        Put("{ServerId:guid}");
        Group<Servers>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var server = await dbContext.Servers.Include(s => s.Members)
            .SingleOrDefaultAsync(s => s.Id == req.ServerId, ct);
        
        if (server == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var result = server.Update(req.Name, null, null, req.Description, req.UserId);

        if (result.IsFailure)
            ThrowError(result.Error.Reason);

        await SendOkAsync(ct);
    }

    public class Request : UpdateServerRequestDto, IHasUserId
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

    public class Documentation : Summary<UpdateServer>
    {
        public Documentation()
        {
            var id = Guid.NewGuid();

            Summary = "Update server with a specified id";
            Description = "Update server with a specified id";
            ExampleRequest = new Request
            {
                Name = null,
                Image = null,
                Description = null,
                BannerImagePath = null,
                ServerId = id
            };
            Response(200, "Server is updated successfully with a specified id");
        }
    }
}