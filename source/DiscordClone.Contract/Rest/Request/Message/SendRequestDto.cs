
namespace DiscordClone.Contract.Rest.Request.Message;

public class SendRequestDto
{
    public Guid ReceiverId { get; set; }
    public string Content { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}



