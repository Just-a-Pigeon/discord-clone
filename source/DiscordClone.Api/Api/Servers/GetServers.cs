using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Response.Servers;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers;

//TODO: Documentation
//TODO: Implement with solr for fuzzy search
public class GetServers(DiscordCloneContext dbContext) : Endpoint<GetServers.Request, GetServersResponseDto[]>
{
    public override void Configure()
    {
        Get("");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = dbContext.Users.SingleOrDefault(u => u.Id == req.UserId);
        var servers = await dbContext.Servers.Where(s => s.Banned.All(b => b != user)).Skip(req.Page * req.Take)
            .Take(req.Take).ToListAsync(ct);

        var result = servers.Select(s => new GetServersResponseDto
        {
            Id = s.Id,
            Name = s.Name,
            ImagePath = s.ImagePath
        }).ToArray();

        await SendOkAsync(result, ct);
    }

    public class Request : IHasUserId
    {
        [FromBody] public int Take { get; set; } = 10;

        [FromBody] public int Page { get; set; } = 0;

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
        }
    }
}