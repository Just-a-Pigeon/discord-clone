using FastEndpoints;

namespace DiscordClone.Api.Api.Message.Send;

public class Send : Group
{
    public Send()
    {
        Configure("messages/send", ep =>
        {
            ep.Description(x =>
            {
                x.RequireAuthorization(); 
            });
        });
    }
}