namespace DiscordClone.Contract.Rest.Response.Message;

public class MessageResponseDto
{
    public Guid SenderId { get;  set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string Content { get; set; }
}