using FastEndpoints;

namespace DiscordClone.Api.Api.Servers.Roles;

public class Roles : SubGroup<Servers>
{
    public Roles()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Configure("{ServerId:Guid}/roles", ep =>
        {
            ep.Description(x =>
            {
                x.RequireAuthorization(); 
            });
        });
    }
}