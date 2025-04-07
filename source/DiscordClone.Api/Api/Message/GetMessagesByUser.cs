using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Response.Message;
using DiscordClone.Domain.Entities.Consultation;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Message;

public class GetMessagesByUser(DiscordCloneContext dbContext)
    : Endpoint<GetMessagesByUser.Request, IList<MessageResponseDto>>
{
    public override void Configure()
    {
        Get("/messages/{userId:guid}");
        Group<Messages>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var myMessages = await dbContext.Messages.Where(m =>
                m.Sender == req.UserId && m.Receiver == req.ReceiverId && m.Type == MessageType.PersonalMessage &&
                m.CreatedOn < DateTimeOffset.UtcNow.AddDays(-3))
            .ToListAsync(ct);

        var receiverMessages = await dbContext.Messages.Where(m =>
            m.Sender == req.ReceiverId && m.Receiver == req.UserId && m.Type == MessageType.PersonalMessage &&
            m.CreatedOn < DateTimeOffset.UtcNow.AddDays(-3)).ToListAsync(ct);

        var messages = myMessages.Concat(receiverMessages).OrderBy(m => m.CreatedOn).Select(m => new MessageResponseDto
        {
            Content = m.Content,
            Date = m.CreatedOn,
            Sender = m.Sender
        }).ToList();
        
        await SendOkAsync(messages,ct);
    }


    public class Request : IHasUserId
    {
        public Guid ReceiverId { get; set; }
        public Guid UserId { get; set; }
    }
}