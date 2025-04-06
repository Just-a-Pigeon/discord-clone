using FastEndpoints;

namespace DiscordClone.Api.Api.Message;

public class Messages : Group
{
    public Messages()
    {
        Configure("messages", ep => { });
    }
}