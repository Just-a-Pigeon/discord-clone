using FastEndpoints;

namespace DiscordClone.Api.Api.Account;

public class Accounts: FastEndpoints.Group
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