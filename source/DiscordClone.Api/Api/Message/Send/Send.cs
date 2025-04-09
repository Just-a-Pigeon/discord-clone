using FastEndpoints;
using Microsoft.AspNetCore.DataProtection;

namespace DiscordClone.Api.Api.Message.Send;

public class Send : Group
{
    public Send()
    {
        Configure("send", ep =>
        {
            ep.Group<Messages>();
            ep.Description(x =>
            {
                x.RequireAuthorization(); 
            });
        });
    }
}