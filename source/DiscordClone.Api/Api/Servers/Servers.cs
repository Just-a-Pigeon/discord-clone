using FastEndpoints;

namespace DiscordClone.Api.Api.Servers;

public class Servers : Group
{
    public Servers()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Configure("servers", ep =>
        {
            ep.Description(x =>
            {
                x.RequireAuthorization(); 
            });
        });
    }
}