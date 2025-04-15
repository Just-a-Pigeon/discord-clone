using FastEndpoints;

namespace DiscordClone.Api.Api.Messages;

public class Messages : Group
{
    public Messages()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Configure("messages", ep =>
        {
            ep.Description(x =>
            {
                x.RequireAuthorization();
            });
        });
    }
}