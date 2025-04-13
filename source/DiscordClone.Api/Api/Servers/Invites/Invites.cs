using FastEndpoints;

namespace DiscordClone.Api.Api.Servers.Invites;

public class Invites : SubGroup<Servers>
{
    public Invites()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Configure("{ServerId:Guid}/invites", ep =>
        {
            ep.Description(x =>
            {
                x.RequireAuthorization(); 
            });
        });
    }
}