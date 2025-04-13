using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Response.Servers;
using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers;

public class GetServer(DiscordCloneContext dbContext) : Endpoint<GetServer.Request, GetServerResponseDto>
{
    public override void Configure()
    {
        Get("{ServerId:guid}");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = dbContext.Users.SingleOrDefault(u => u.Id == req.UserId);
        var server = await dbContext.Servers.SingleOrDefaultAsync(s => s.Id == req.ServerId, ct);
        var nodes = await dbContext.ServerNodes.Where(sn => sn.Server == server).ToListAsync(ct);
        var members = await dbContext.ServerMembers
            .Include(sm => sm.User)
            .Where(m => m.Server == server)
            .ToListAsync(ct);

        if (server is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (server.Banned.Any(b => b == user))
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var result = new GetServerResponseDto
        {
            Id = server.Id,
            Name = server.Name,
            ImagePath = server.ImagePath,
            Description = server.Description,
            BannerImage = server.BannerImagePath,
            Nodes = BuildTree(nodes),
            Members = members.Select(m => new MemberDto
            {
                Id = m.User.Id,
                Name = m.User.UserName!
            }).ToList()
        };

        await SendOkAsync(result, ct);
    }

    private static List<ServerNodesDto> BuildTree(List<ServerNode> flatList)
    {
        var lookup = flatList.ToDictionary(x => x.Id, x => new ServerNodesDto
        {
            Id = x.Id,
            Name = x.Name
        });

        var rootNodes = new List<ServerNodesDto>();

        foreach (var entity in flatList)
            if (entity.Parent == null)
                rootNodes.Add(lookup[entity.Id]);
            else if (lookup.TryGetValue(entity.Parent.Id, out var parent)) parent.Children.Add(lookup[entity.Id]);

        return rootNodes;
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