namespace DiscordClone.Contract.Rest.Response.Message;

public class MessageResponseDto
{
    public Guid Sender { get;  set; }
    public DateTimeOffset Date { get; set; }
    public string Content { get; set; }
}