using FastEndpoints;

namespace DiscordClone.Api.Api.Servers.Node;

public class Nodes : SubGroup<Servers>
{
    public Nodes()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Configure("{ServerId:Guid}/nodes", ep =>
        {
            ep.Description(x =>
            {
                x.RequireAuthorization(); 
            });
        });
    }
}