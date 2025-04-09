namespace DiscordClone.Contract.Rest.Response.Message;

public class MessageResponseDto
{
    public Guid ReceiverId { get;  set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string Content { get; set; }
}