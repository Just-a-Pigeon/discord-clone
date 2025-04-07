using DiscordClone.Contract.Rest.Response.Message;
using DiscordClone.Domain.Entities.Consultation;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Message;

public class GetMessages(DiscordCloneContext dbContext) : Endpoint<GetMessages.Request, IList<MessageResponseDto>>
{
    public override void Configure()
    {
        Get("messages");
        Group<Messages>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var messages = await dbContext.Messages.Where(m => m.Type == MessageType.Group && m.Receiver == req.RoomId)
            .ToListAsync(ct);

        var result = messages.Select(m => new MessageResponseDto
        {
            Content = m.Content,
            Date = m.CreatedOn,
            Sender = m.Sender
        }).ToList();

        await SendOkAsync(result, ct);
    }


    public class Request
    {
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
    }
}