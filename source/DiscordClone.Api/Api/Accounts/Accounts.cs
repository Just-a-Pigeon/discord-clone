using FastEndpoints;

namespace DiscordClone.Api.Api.Accounts;

public class Accounts : Group
{
    public Accounts()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Configure("accounts", ep =>
        {
            ep.Description(x =>
            {
                x.RequireAuthorization();
            });
        });
    }
}