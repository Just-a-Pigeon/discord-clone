using FastEndpoints;

namespace DiscordClone.Api.Api.Servers.Members;

public class Members : SubGroup<Servers>
{
    public Members()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Configure("{ServerId:Guid}/members", ep =>
        {
            ep.Description(x =>
            {
                x.RequireAuthorization(); 
            });
        });
    }
}