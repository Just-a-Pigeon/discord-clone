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

        if (parentNode == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (!Enum.TryParse(req.Type, out ServerNodeType serverNodeType))
        {
            await SendNotFoundAsync(ct);
            return;
        }

        ServerNode? serverNode = null;
        ValidationError? validationError = null;
        var isFailure = false;
        
        switch (serverNodeType)
        {
            case ServerNodeType.Category:
            {
                (_, isFailure, serverNode, validationError) = ServerNode.CreateCategory(
                    new ServerNode.CreateServerNodeCommand
                    {
                        Name = req.Name,
                        Parent = parentNode?.Id,
                        Server = req.ServerId,
                        IsPrivate = req.IsPrivate,
                        IsAgeRestricted = req.IsAgeRestricted,
                    });
                break;
            }
            case ServerNodeType.Text:
            {
                (_, isFailure, serverNode, validationError) = ServerNode.CreateTextChannel(
                    new ServerNode.CreateServerNodeCommand
                    {
                        Name = req.Name,
                        Parent = parentNode?.Id,
                        Server = req.ServerId,
                        IsPrivate = req.IsPrivate,
                        IsAgeRestricted = req.IsAgeRestricted,
                    });
                break;
            }
            case ServerNodeType.Voice:
            {
                (_, isFailure, serverNode, validationError) = ServerNode.CreateVoiceChannel(
                    new ServerNode.CreateServerNodeCommand
                    {
                        Name = req.Name,
                        Parent = parentNode?.Id,
                        Server = req.ServerId,
                        IsPrivate = req.IsPrivate,
                        IsAgeRestricted = req.IsAgeRestricted,
                    });
                break;
            }
            default:
                ThrowError("Node type not supported");
                break;
        }

        if (isFailure)
            ThrowError(validationError.Reason);
        
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

    public class Reqeust : CreateNodeRequestDto, IHasUserId
    {
        public Guid ServerId { get; set; }

        [HideFromDocs] public Guid UserId { get; set; }
    }
}