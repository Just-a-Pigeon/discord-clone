using FastEndpoints;

namespace DiscordClone.Api.Api.Messages.Send;

public class Send : SubGroup<Messages>
{
    public Send()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Configure("send", ep =>
        {
            ep.Description(x =>
            {
                x.RequireAuthorization(); 
            });
        });
    }
}