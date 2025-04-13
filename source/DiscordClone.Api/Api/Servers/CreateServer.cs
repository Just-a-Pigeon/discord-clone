using CSharpFunctionalExtensions;
using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Request.Servers;
using DiscordClone.Contract.Rest.Response.Servers;
using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;

namespace DiscordClone.Api.Api.Servers;

//TODO: Add consumer to delete all messages
//TODO: Look for a file system
//TODO: Hangfire to delete inactive server (0 members)
//TODO: Add permission validation
public class CreateServer(DiscordCloneContext dbContext) : Endpoint<CreateServer.Request, CreateServerResponseDto>
{
    public override void Configure()
    {
        Post("");
        Group<Servers>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var (_, isFailureServer, server, errorServer) = Server.Create(req.Name, null, req.UserId);

        if (isFailureServer)
            ThrowError(errorServer.Reason);

        dbContext.Servers.Add(server);
        await dbContext.SaveChangesAsync(ct);

        var (_, isFailureTextCategory, textCategory, errorTextCategory) = ServerNode.CreateCategory(
            new ServerNode.CreateServerNodeCommand
            {
                Name = "Text channels",
                Parent = null,
                Server = server.Id,
                IsPrivate = false,
                IsAgeRestricted = false
            });

        if (isFailureTextCategory)
            ThrowError(errorTextCategory.Reason);

        dbContext.ServerNodes.Add(textCategory);

        var (_, isFailureVoiceCategory, voiceCategory, errorVoiceCategory) = ServerNode.CreateCategory(
            new ServerNode.CreateServerNodeCommand
            {
                Name = "Voice channels",
                Parent = null,
                Server = server.Id,
                IsPrivate = false,
                IsAgeRestricted = false
            });

        if (isFailureVoiceCategory)
            ThrowError(errorVoiceCategory.Reason);

        dbContext.ServerNodes.Add(voiceCategory);

        await dbContext.SaveChangesAsync(ct);

        var (_, isFailureTextChannel, textChannel, errorTextChannel) = ServerNode.CreateCategory(
            new ServerNode.CreateServerNodeCommand
            {
                Name = "General",
                Parent = textCategory.Id,
                Server = server.Id,
                IsPrivate = false,
                IsAgeRestricted = false
            });

        if (isFailureTextChannel)
            ThrowError(errorTextChannel.Reason);

        dbContext.ServerNodes.Add(textCategory);

        var (_, isFailureVoiceChannel, voiceChannel, errorVoiceChannel) = ServerNode.CreateCategory(
            new ServerNode.CreateServerNodeCommand
            {
                Name = "general",
                Parent = voiceCategory.Id,
                Server = server.Id,
                IsPrivate = false,
                IsAgeRestricted = false
            });

        if (isFailureVoiceChannel)
            ThrowError(errorVoiceChannel.Reason);

        await dbContext.ServerNodes.AddAsync(textChannel, ct);
        await dbContext.ServerNodes.AddAsync(voiceChannel, ct);

        await dbContext.SaveChangesAsync(ct);


        await SendOkAsync(new CreateServerResponseDto
        {
            ServerId = server.Id
        }, ct);
    }

    public class Request : CreateServerRequestDto, IHasUserId
    {
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

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name is required");
        }
    }

    public class Documentation : Summary<CreateServer>
    {
        public Documentation()
        {
            Summary = "Update server with a specified id";
            Description = "Update server with a specified id";
            ExampleRequest = new Request
            {
                Name = "New server",
                ImageStagedPath = $"/stage/{Guid.NewGuid()}.png",
            };
            
            Response(200, "Server has been created.");
            Response(400, "Client side error.");
            Response(401, "User is not permitted to join this server.");
            Response(404, "Invite, user or server was not found.");
        }
    }
}