using CSharpFunctionalExtensions;
using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Request.Servers.Nodes;
using DiscordClone.Contract.Rest.Response.Servers.Nodes;
using DiscordClone.Domain;
using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Servers.Node;

public class CreateNode(DiscordCloneContext dbContext) : Endpoint<CreateNode.Reqeust>
{
    public override void Configure()
    {
        Post("");
        Group<Nodes>();
    }

    public override async Task HandleAsync(Reqeust req, CancellationToken ct)
    {
        var server = await dbContext.Servers
            .Include(s => s.ServerNodes)
            .Include(s => s.Members)
            .ThenInclude(s => s.Roles)
            .SingleOrDefaultAsync(s => s.Id == req.ServerId, ct);

        if (server == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var member = server.Members.SingleOrDefault(s => s.UserId == req.UserId);
        if (member == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (!member.CanManageChannels())
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var parentNode = server.ServerNodes.SingleOrDefault(s => s.Id == req.Parent);

        if (req.Parent is not null && parentNode is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (parentNode is not null && parentNode.Type != ServerNodeType.Category)
            ThrowError("Cannot attach to this parent node");

        if (!Enum.TryParse(req.Type, out ServerNodeType serverNodeType))
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var createServerNodeCommand = new ServerNode.CreateServerNodeCommand
        {
            Name = req.Name,
            Parent = parentNode?.Id,
            Server = req.ServerId,
            IsPrivate = req.IsPrivate,
            IsAgeRestricted = req.IsAgeRestricted
        };

        var (_, isFailure, serverNode, error) = CreateServerNode(createServerNodeCommand, serverNodeType);
        if (isFailure)
            ThrowError(error.Reason);

        dbContext.ServerNodes.Add(serverNode);
        await dbContext.SaveChangesAsync(ct);

        var result = new CreateNodeResponseDto
        {
            Id = serverNode.Id,
            Name = serverNode.Name,
            Type = serverNode.Type.ToString(),
            IsPrivate = serverNode.IsPrivate,
            IsAgeRestricted = serverNode.IsAgeRestricted
        };

        await SendOkAsync(result, ct);
    }

    private Result<ServerNode, ValidationError> CreateServerNode(
        ServerNode.CreateServerNodeCommand createServerNodeCommand, ServerNodeType serverNodeType)
    {
        switch (serverNodeType)
        {
            case ServerNodeType.Category:
            {
                return ServerNode.CreateCategory(createServerNodeCommand);
            }
            case ServerNodeType.Text:
            {
                return ServerNode.CreateTextChannel(createServerNodeCommand);
            }
            case ServerNodeType.Voice:
            {
                return ServerNode.CreateVoiceChannel(createServerNodeCommand);
            }
            default:
                return ValidationError.InvalidInput("Node type not supported", "type");
        }
    }

    public class Reqeust : CreateNodeRequestDto, IHasUserId
    {
        public Guid ServerId { get; set; }

        [HideFromDocs] public Guid UserId { get; set; }
    }
}